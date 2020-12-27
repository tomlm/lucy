using System;
using System.Collections.Generic;
using System.Text;
using J2N.IO;

namespace Lucy.PatternMatchers
{
    /// <summary>
    /// Represents a match result while processing entities.
    /// </summary>
    public class MatchResult
    {
        public MatchResult(bool matched, PatternMatcher matcher, TokenEntity nextToken = null, int start = 0, int end = 0)
        {
            Matched = matched;
            PatternMatcher = matcher;
            NextToken = nextToken;
            Start = start;
            End = end;
        }

        /// <summary>
        /// True if the matcher was successful
        /// </summary>
        public bool Matched { get; set; }

        /// <summary>
        /// Start of recognized sequence
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// End of recognized sequence.
        /// </summary>
        public int End { get; set; }

        /// <summary>
        /// Next token to process
        /// </summary>
        public TokenEntity NextToken { get; set; }

        /// <summary>
        /// Pattern which matched.
        /// </summary>
        public PatternMatcher PatternMatcher { get; set; }

        /// <summary>
        /// If there is a NextPattern and it was matched, this is the result of that operation.  
        /// To process wildcards we need to look ahead evaluate the nextPattern.
        /// </summary>
        public MatchResult NextPatternMatch { get; set; }

        public override string ToString() => $"[{Start},{End}] {Matched} {PatternMatcher} NextToken:'{NextToken?.Text}'";
    }
}
