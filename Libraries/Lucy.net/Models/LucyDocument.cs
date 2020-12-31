using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lucy
{
    public class LucyDocument
    {
        public LucyDocument()
        { }

        /// <summary>
        /// The locale for this model (default:en)
        /// </summary>
        [JsonProperty("locale")]
        public string Locale { get; set; } = "en";

        /// <summary>
        /// The names of any external entities that may be passed in.
        /// </summary>
        [JsonProperty("externalEntities", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<string> ExternalEntities { get; set; }

        /// <summary>
        /// Macros
        /// </summary>
        [JsonProperty("macros", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<string, string> Macros { get; set; }

        /// <summary>
        /// Entity definitions
        /// </summary>
        [JsonProperty("entities", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<EntityDefinition> Entities { get; set; } = new List<EntityDefinition>();
    }
}
