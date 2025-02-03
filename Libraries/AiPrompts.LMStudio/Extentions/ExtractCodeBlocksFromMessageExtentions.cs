using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AiPrompts
{
    public static class ExtractCodeBlocksFromMessageExtentions
    {
        public static IEnumerable<string> ExtractCodeBlocksFromMessage(this string message, string codeBlockPrefix, string codeBlockSuffix)
        {
            string text = message ?? string.Empty;

            if (string.IsNullOrWhiteSpace(text))
                yield break;

            string pattern
                = (codeBlockPrefix + "([\\s\\S]*?)" + codeBlockSuffix)
                    .Trim();

            IEnumerable<string> matchCollection
                = Regex
                    .Matches(text, pattern)
                        .Select(
                            match =>
                                match.Groups[0].Value
                                    .TrimStart(codeBlockPrefix.ToCharArray())
                                        .TrimEnd(codeBlockSuffix.ToCharArray())
                        );

            foreach (string match in matchCollection)
                yield return match;
        }
    }
}
