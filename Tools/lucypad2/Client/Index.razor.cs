// #define embedded
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;
using Blazorise;
using BlazorMonacoYaml;
using Lucy;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace LucyPad2.Client
{
    public partial class Index
    {
        private IDeserializer yamlDeserializer = new DeserializerBuilder()
                                                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                                                    .Build();
        private ISerializer yamlToJsonSerializer = new SerializerBuilder()
                                                .JsonCompatible()
                                                .Build();

        private MonacoEditorYaml yamlEditor;
        private Alert alertBox;
        private string selectedExample;

        private LucyEngine engine = null;
        private string lucyModel = null;

        public Index()
        {
        }

        [Inject]
        public HttpClient Http { get; set; }

        public bool ShowJson = false;

        public bool TopResultVisible = true;

        public bool AllResultsVisible = false;

        public string Yaml { get; set; }

        public string TopResult { get; set; }

        public string EntityResults { get; set; }

        public string Text { get; set; }

        public string Error { get; set; } = "---";

        public string Elapsed { get; set; }

        protected override void OnInitialized()
        {
            this.selectedExample = "simple";
            this.Yaml = LoadResource("LucyPad2.Client.Samples.simple.lucy.yaml");
        }

        async Task OnSelectedExampleChanged(string value)
        {
            this.ShowJson = false;
            this.selectedExample = value;
            this.Yaml = LoadResource($"LucyPad2.Client.Samples.{value}.lucy.yaml");
            await yamlEditor.SetValue(this.Yaml);
        }

        async Task OnJsonSwitch(bool value)
        {
            this.ShowJson = value;
            var text = await this.yamlEditor.GetValue();
            bool isJson = text.Trim().StartsWith("{");
            try
            {

                if (this.ShowJson == true)
                {
                    if (!isJson)
                    {
                        var doc = yamlDeserializer.Deserialize<LucyDocument>(new StringReader(text));
                        await yamlEditor.SetValue(JsonConvert.SerializeObject(doc, Formatting.Indented));
                    }
                }
                else
                {
                    if (isJson)
                    {
                        var doc = JsonConvert.DeserializeObject<LucyDocument>(text);
                        var yaml = new SerializerBuilder()
                            .WithNamingConvention(CamelCaseNamingConvention.Instance)
                            .Build()
                            .Serialize(doc);
                        await yamlEditor.SetValue(yaml);
                    }
                }
            }
            catch (SemanticErrorException err)
            {
                this.Error = err.Message;
                this.alertBox.Show();
                //this.editor.ScrollToLine(err.Start.Line);
                //var line = this.editor.Document.GetLineByNumber(err.Start.Line - 1);
                //this.editor.Select(line.Offset, line.Length);
            }
            catch (SyntaxErrorException err)
            {
                this.Error = err.Message;
                this.alertBox.Show();
                //this.error.Visibility = Visibility.Visible;
                //this.editor.ScrollToLine(err.Start.Line);
                //var line = this.editor.Document.GetLineByNumber(err.Start.Line - 1);
                //this.editor.Select(line.Offset, line.Length);
            }
            catch (Exception err)
            {
                this.Error = err.Message;
                this.alertBox.Show();
                //this.error.Content = err.Message;
                //this.error.Visibility = Visibility.Visible;
            }
        }

        private string LoadResource(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(name))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }

        }

        async Task OnTextChanged(string value)
        {
            try
            {
                var yaml = await yamlEditor.GetValue();
                var text = value.Trim() ?? string.Empty;
                IEnumerable<LucyEntity> results = null;
                if (text.Length == 0)
                {
                    return;
                }
#if embedded
                LoadModel(yaml);
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                results = engine.MatchEntities(text);
                sw.Stop();
                this.Elapsed = $"{sw.Elapsed.TotalMilliseconds} ms";
#else
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                var result = await Http.PostAsJsonAsync("entities", new EntitiesRequest
                {
                    yaml = yaml,
                    text = text
                });
                sw.Stop();
                // this.Elapsed = $"Network: {sw.Elapsed.TotalMilliseconds} ms";
                var entityResponse = JsonConvert.DeserializeObject<EntitiesResponse>(await result.Content.ReadAsStringAsync());

                this.Elapsed = $"{entityResponse.elapsed} ms";

                if (!String.IsNullOrEmpty(entityResponse.message))
                {
                    this.Error = entityResponse.message;
                    this.alertBox.Show();
                }
                else
                {
                    this.alertBox.Hide();
                }
                results = entityResponse.entities;
#endif

                this.TopResultVisible = true;
                this.AllResultsVisible = false;
                this.TopResult = LucyEngine.VisualEntities(text, results);
                this.EntityResults = String.Join("\n\n", results.Select(entity => LucyEngine.VisualizeEntity(text, entity)));
            }
            catch (SemanticErrorException err)
            {
                this.Error = err.Message;
                this.alertBox.Show();
                //this.editor.ScrollToLine(err.Start.Line);
                //var line = this.editor.Document.GetLineByNumber(err.Start.Line - 1);
                //this.editor.Select(line.Offset, line.Length);
            }
            catch (SyntaxErrorException err)
            {
                this.Error = err.Message;
                this.alertBox.Show();
                //this.error.Visibility = Visibility.Visible;
                //this.editor.ScrollToLine(err.Start.Line);
                //var line = this.editor.Document.GetLineByNumber(err.Start.Line - 1);
                //this.editor.Select(line.Offset, line.Length);
            }
            catch (Exception err)
            {
                this.Error = err.Message;
                this.alertBox.Show();
                //this.error.Content = err.Message;
                //this.error.Visibility = Visibility.Visible;
            }
        }

        private void LoadModel(string yaml)
        {
            if (lucyModel != yaml)
            {
                // Trace.TraceInformation("Loading model");
                engine = new LucyEngine(yaml, useAllBuiltIns: false);
                this.lucyModel = yaml;

                // this.examplesBox.Text = sb.ToString();

                if (engine.Warnings.Any())
                {
                    this.Error = String.Join("\n", engine.Warnings);
                    this.alertBox.Show();
                }
                else
                {
                    this.Error = string.Empty;
                    this.alertBox.Hide();
                }
            }
        }
    }
}
