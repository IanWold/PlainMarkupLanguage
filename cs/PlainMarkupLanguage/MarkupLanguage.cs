﻿using System;
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
            var toReturn =
                lines.FindSet(i => i.Indentation == indentation, i => i.IsEmpty(), i => i.Indentation == indentation)
                    ? new List<Dictionary<string, object>>() { new Dictionary<string, object>() }
                    : new Dictionary<string, object>() as object;

            Dictionary<string, object> GetDictionaryForAdd() =>
                toReturn is Dictionary<string, object> dictionary
                    ? dictionary
                    : (toReturn as List<Dictionary<string, object>>).Last();

            for (int i = 0; i < lines.Count(); i++)
            {
                var line = lines.ElementAt(i);

                if (line.IsEmpty() && toReturn is List<Dictionary<string, object>> list)
                {
                    list.Add(new Dictionary<string, object>());
                }
                else if (string.IsNullOrEmpty(line.Remainder))
                {
                    var linesToParse = lines.Skip(i + 1).TakeWhile(l => l.Indentation > indentation | l.IsEmpty()).ToList();
                    if (linesToParse.Count > 0 && linesToParse.Last().IsEmpty()) linesToParse.RemoveAt(linesToParse.Count - 1);

                    GetDictionaryForAdd().Add(line.Predicate, Parse(linesToParse, indentation + 1));

                    i += linesToParse.Count;
                }
                else
                {
                    GetDictionaryForAdd().Add(line.Predicate, line.Remainder);
                }
            }

            return toReturn;
        }

        public static bool FindSet<T>(this IEnumerable<T> enumerable, params Func<T, bool>[] predicates)
        {
            foreach (var item in enumerable.Where(i => predicates[0](i)))
            {
                var index = enumerable.ToList().IndexOf(item);
                var mismatch = false;

                for (int i = 1; i < predicates.Count(); i++)
                {
                    if (enumerable.Count() <= i + index || !predicates[i](enumerable.ElementAt(i + index)))
                    {
                        mismatch = true;
                        break;
                    }
                }

                if (!mismatch) return true;
            }

            return false;
        }

        public static bool IsEmpty(this (int, string, string) item) =>
            string.IsNullOrEmpty(item.Item2) && string.IsNullOrEmpty(item.Item3);
    }
}
