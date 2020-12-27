using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Lucene.Net.Analysis;
using Newtonsoft.Json.Linq;

namespace Lucy
{
    /// <summary>
    /// Represents a pattern which is a string, or array of strings
    /// </summary>
    public class Pattern : IEnumerable<string>
    {
        private List<string> patterns = new List<string>();

        public Pattern()
        {
        }

        public Pattern(string patternDefinition)
        {
            this.patterns.Add(patternDefinition.Trim());
        }

        public Pattern(string[] patternDefinitions)
        {
            if (patternDefinitions!= null && patternDefinitions.Any())
            {
                this.patterns.AddRange(patternDefinitions.Select(pattern => pattern.Trim()));
            }
        }

        public IEnumerator<string> GetEnumerator() => this.patterns.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.patterns).GetEnumerator();

        public static implicit operator Pattern(string patternDefinition) => new Pattern(patternDefinition);
        public static implicit operator Pattern(JValue patternDefinition) => new Pattern((string)patternDefinition);

        public static implicit operator Pattern(string[] patternDefinitions) => new Pattern(patternDefinitions);
        public static implicit operator Pattern(JArray patternDefinitions) => new Pattern(patternDefinitions.ToObject<string[]>());

        public override string ToString() => $"[{this.patterns.FirstOrDefault()}, ...]";
    }
}
