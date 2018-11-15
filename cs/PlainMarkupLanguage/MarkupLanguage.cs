using System;
using System.Collections.Generic;
using System.Linq;

namespace PlainMarkupLanguage
{
    public static class MarkupLanguage
    {
        public static object Parse(string toParse) =>
            Parse(
                toParse
                    .Split(new string[] { "\r\n" }, StringSplitOptions.None)
                    .Select(i => (
                        Indentation: i.TakeWhile(c => c == '\t').Count(),
                        Predicate: i.Split(' ')[0].TrimStart(),
                        Remainder: i.Contains(' ')
                            ? string.Concat(i.SkipWhile(c => c != ' ')).TrimStart()
                            : ""
                        )
                    ),
                0
            );

        public static object Parse(IEnumerable<(int Indentation, string Predicate, string Remainder)> lines, int indentation)
        {
            object toReturn = new Dictionary<string, object>();

            void AddToReturn(string key, object value)
            {
                if (toReturn is Dictionary<string, object> dictionary)
                {
                    dictionary.Add(key, value);
                }
                else
                {
                    (toReturn as List<Dictionary<string, object>>).Last().Add(key, value);
                }
            }

            for (int i = 0; i < lines.Count(); i++)
            {
                var line = lines.ElementAt(i);

                if (line.IsEmpty())
                {
                    if (toReturn is Dictionary<string, object> dictionary)
                    {
                        toReturn = new List<Dictionary<string, object>>() { dictionary };
                    }

                    (toReturn as List<Dictionary<string, object>>).Add(new Dictionary<string, object>());
                }
                else if (string.IsNullOrEmpty(line.Remainder))
                {
                    var linesToParse = lines.Skip(i + 1).TakeWhile(l => l.Indentation > indentation | l.IsEmpty()).ToList();
                    if (linesToParse.Count > 0 && linesToParse.Last().IsEmpty()) linesToParse.RemoveAt(linesToParse.Count - 1);

                    AddToReturn(line.Predicate, Parse(linesToParse, indentation + 1));

                    i += linesToParse.Count;
                }
                else
                {
                    AddToReturn(line.Predicate, line.Remainder);
                }
            }

            return toReturn;
        }

        public static bool IsEmpty(this (int, string, string) item) =>
            string.IsNullOrEmpty(item.Item2) && string.IsNullOrEmpty(item.Item3);
    }
}
