using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day19
{
    public static class Part2
    {
        public static void Solve()
        {
            (string regex, List<string> messages) = ParseRulesAndMessages();
            Console.WriteLine($"Regex: {regex}");
            //messages.ForEach(Console.WriteLine);

            var validMessages = messages.Count(message => Regex.Match(message, regex).Success);
            Console.WriteLine($"Valid Messages: {validMessages}");
        }

        private static Tuple<string, List<string>> ParseRulesAndMessages()
        {
            var file = new StreamReader(@"/Users/rbakken/RiderProjects/AdventOfCode/AdventOfCode/Day19/day_19.txt");
            string line;
            var ruleDictionary = new SortedDictionary<int, string[]>();
            var messages = new List<string>();
            var rulesParsed = false;

            while ((line = file.ReadLine()) != null)
            {
                if (String.IsNullOrEmpty(line))
                {
                    rulesParsed = true;
                }
                else if (!rulesParsed)
                {
                    string[] splitLine = line.Split(':');
                    string[] rules = new string(splitLine[1].Trim().ToCharArray().Where(c => c != '\"').ToArray()).Split(' ');
                    ruleDictionary.Add(Int32.Parse(splitLine[0]), rules);
                }
                else
                {
                    messages.Add(line.Trim());
                }
            }

            file.Close();

            return new Tuple<string, List<string>>(ParseRulesRegex(ruleDictionary), messages);
        }

        private static string ParseRulesRegex(IReadOnlyDictionary<int, string[]> ruleDictionary)
        {
            Dictionary<int, string[]> copiedRulesDict = ruleDictionary.ToDictionary(x => x.Key, x => x.Value);
            for (int i = ruleDictionary.Count - 1; i >= 0; i--)
            {
                KeyValuePair<int, string[]> element = ruleDictionary.ElementAt(i);
                int key = element.Key;
                string[] ruleArr = element.Value;

                if (ruleArr.Length != 1 || Int32.TryParse(ruleArr[0], out _))
                {
                    /*if (key == 8)
                    {
                        ruleArr = new[] {ruleArr[0], "+"};
                    }
                    else if (key == 11)
                    {
                        ruleArr = new[] {ruleArr[0], "+", ruleArr[1], "+"};
                    }*/

                    string ruleRegex = ReduceRule(ruleArr, copiedRulesDict);
                    copiedRulesDict[key] = new []{ ruleRegex };
                }
            }

            return $"^{String.Join("", copiedRulesDict[0])}$";
        }

        private static string ReduceRule(IEnumerable<string> rules, IReadOnlyDictionary<int, string[]> rulesDictionary)
        {
            var ruleRegex = "";
            foreach (string rule in rules)
            {
                if (Int32.TryParse(rule, out int subRuleKey))
                {
                    if (rulesDictionary[subRuleKey].All(IsCharacterString))
                    {
                        ruleRegex += $"{String.Join("" ,rulesDictionary[subRuleKey])}";
                    }
                    else
                    {
                        ruleRegex += $"({ReduceRule(rulesDictionary[subRuleKey], rulesDictionary)})";
                    }
                }
                else
                {
                    ruleRegex += rule;
                }
            }

            return ruleRegex;
        }

        private static bool IsCharacterString(string str)
        {
            return str.ToCharArray().All(Char.IsLetter);
        }
    }
}
