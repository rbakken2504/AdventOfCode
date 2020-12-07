using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day7
{
    public static class Part2
    {
        public static void Solve()
        {
            var file = new StreamReader(@"/Users/rbakken/RiderProjects/AdventOfCode/AdventOfCode/Day7/day_7.txt");
            string line;
            var bagDefinitions = new Dictionary<string, ColoredBag>();

            while ((line = file.ReadLine()) != null)
            {
                string[] rulePartitions = line.Split(new[] {"contain"}, StringSplitOptions.None);
                Dictionary<string, int> containedBags = ParseContainedBags(rulePartitions[1]);
                string color = ParseColor(rulePartitions[0], 0, 1);
                ColoredBag bagDefinition = CreateBagDefinition(color, containedBags);

                bagDefinitions.Add(color, bagDefinition);
            }

            Console.WriteLine($"Total Bags: {bagDefinitions["shiny gold"].TotalContainedBags(bagDefinitions)}");
        }

        private static ColoredBag CreateBagDefinition(string color, Dictionary<string, int> containedBags)
        {
            return new ColoredBag
            {
                Color = color,
                ContainedBags = containedBags
            };
        }

        private static string ParseColor(string colorPartition, int firstIdx, int secondIdx)
        {
            return $"{colorPartition.Trim().Split(' ')[firstIdx]} {colorPartition.Trim().Split(' ')[secondIdx]}";
        }

        private static Dictionary<string, int> ParseContainedBags(string bagPartition)
        {
            string[] bagRules = bagPartition.Trim().Split(',');
            return !bagPartition.Contains("no other") ? bagRules.ToDictionary(str => ParseColor(str, 1, 2), ParseCount) : new Dictionary<string, int>();
        }

        private static int ParseCount(string bagPartition)
        {
            return Int32.Parse(bagPartition.Trim().Split(' ')[0]);
        }
    }
}
