using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lucy.PatternMatchers
{
    /// <summary>
    /// PatternMatcher which uses structural constraints and phrases to match.
    /// </summary>
    public class ValidationPatternMatcher : PatternMatcher
    {
        /// <summary>
        /// Uses a sequence of matchers to validate a structure.
        /// </summary>
        /// <param name="patternMatchers"></param>
        public ValidationPatternMatcher(IEnumerable<PatternMatcher> patternMatchers)
        {
            PatternMatchers.AddRange(patternMatchers);
        }

        public List<PatternMatcher> PatternMatchers { get; set; } = new List<PatternMatcher>();

        public override string GenerateExample(LucyEngine engine)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> GenerateExamples(LucyEngine engine)
        {
            throw new NotImplementedException();
        }

        public override MatchResult Matches(MatchContext matchContext, TokenEntity tokenEntity, PatternMatcher nextPatternMatcher)
        {
            throw new NotImplementedException();
            /*
            var tokenEntity = startToken;
            // try to match each element in the sequence.
            int start = startToken?.Start ?? 0;
            int end = 0;
            for (int iPattern = 0; iPattern < PatternMatchers.Count; iPattern++)
            {
                var matchResult = new MatchResult(false, this, tokenEntity);
                var patternMatcher = PatternMatchers[iPattern];
                if (patternMatcher.ContainsWildcard())
                {
                    // look ahead to non wild card
                    nextPatternMatcher = PatternMatchers.Skip(iPattern).Where(pm => !pm.ContainsWildcard()).FirstOrDefault();

                    // run wildcard pattern matcher
                    matchResult = patternMatcher.Matches(context, tokenEntity, nextPatternMatcher);

                    // if the match was not the wildcard pattern, then advance to that.
                    if (matchResult.NextPatternMatch != null)
                    {
                        Debug.Assert(matchResult.NextPatternMatch.PatternMatcher == nextPatternMatcher);
                        matchResult = matchResult.NextPatternMatch;
                        iPattern = PatternMatchers.IndexOf(matchResult.PatternMatcher);
                        Debug.Assert(iPattern >= 0);
                    }
                }
                else
                {
                    matchResult = patternMatcher.Matches(context, tokenEntity, nextPatternMatcher);
                }

                // if the element did not match, then sequence is bad, return failure
                if (matchResult.Matched == false)
                {
                    return new MatchResult(false, this, tokenEntity);
                }

                tokenEntity = matchResult.NextToken;
                end = Math.Max(matchResult.End, end);
            }

            return new MatchResult(true, this, tokenEntity, start, end);
            */
        }
    }
}
