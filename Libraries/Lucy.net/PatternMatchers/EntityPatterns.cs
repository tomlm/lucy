using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lucy.PatternMatchers
{
    /// <summary>
    /// Collection of patterns for recognizing entities.
    /// </summary>
    public class EntityPatterns
    {
        /// <summary>
        /// Builtin entities that are being used.
        /// </summary>
        public HashSet<string> BuiltIn { get; set; } = new HashSet<string>();

        /// <summary>
        /// Patterns to match
        /// </summary>
        public List<EntityPattern> Simple { get; set; } = new List<EntityPattern>();

        /// <summary>
        /// Wildcard Patterns to match
        /// </summary>
        public List<EntityPattern> Wildcard { get; set; } = new List<EntityPattern>();

        /// <summary>
        /// Regex patterns to match
        /// </summary>
        public List<RegexEntityRecognizer> Regex { get; set; } = new List<RegexEntityRecognizer>();

        /// <summary>
        /// Add patterns to collections.
        /// </summary>
        /// <param name="patterns"></param>
        public void AddPatterns(EntityPatterns patterns)
        {
            BuiltIn.Union(patterns.BuiltIn);
            Simple.AddRange(patterns.Simple);
            Wildcard.AddRange(patterns.Wildcard);
            Regex.AddRange(patterns.Regex);
        }

        public bool Any() => BuiltIn.Any() || Simple.Any() || Wildcard.Any() || Regex.Any();
    }
}
