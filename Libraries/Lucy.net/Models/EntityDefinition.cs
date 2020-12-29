using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lucy
{
    /// <summary>
    /// Represents a entity definition
    /// </summary>
    public class EntityDefinition
    {
        public EntityDefinition()
        {
        }

        /// <summary>
        /// Gets or sets the name of the entity
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the default fuzzy match for text tokens
        /// </summary>
        /// <remarks>
        /// If this is set to true, then fuzzy match will be used for all tokens
        /// in the patterns by default, and ~ modifier will turn OFF fuzzy match.
        /// </remarks>
        [JsonProperty("fuzzyMatch")]
        public bool FuzzyMatch { get; set; } = false;

        /// <summary>
        /// Example utterances for this entity.
        /// </summary>
        [JsonProperty("examples")]
        public List<string> Examples { get; set; } = new List<string>();

        /// <summary>
        /// Ignore tokens.
        /// </summary>
        [JsonProperty("ignore")]
        public List<string> Ignore{ get; set; } = new List<string>();

        /// <summary>
        /// patterns which define the entity
        /// </summary>
        [JsonProperty("patterns")]
        public List<Pattern> Patterns { get; set; }  = new List<Pattern>();

        public override string ToString() => $"{Name}{(FuzzyMatch ? "~" : "")}";
    }
}
