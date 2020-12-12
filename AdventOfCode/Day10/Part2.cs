using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day10
{
    public static class Part2
    {
        public static void Solve()
        {
            SortedList<int, int> sortedJoltages = ParseFile();
            /*
             * After duking it out with building a tree for hours (and my tree being correct for example 1, but wrong for example 2), I painstakingly
             * used pen a paper to find a pattern.  Was pretty certain I could use combinatorics, but I was always shitty at building those formulae, so thought it would be
             * easier to see if I could recognize a pattern in the first example, then test it out on the second.  I noticed that if you count consecutive joltages, there is a
             * distinct number of possibilities for that group. 3 consecutive has two possibilities, 4 consecutive has four possibilities, 5 consecutive has seven possibilities.
             * 2 consecutive is always only one possibility, so we can ignore that one.  Effectively, I'm counting the groups of sub-trees and multiplying their possibilities
             * together.
             *
             * So we end up with a formula:
             * 2^(number of 3 consecutive groups) * 4^(number of 4 consecutive groups) + 7^(number of 5 consecutive groups)
             */
            var consecutiveJoltagesLookup = new Dictionary<int, int>
            {
                {3, 0}, {4, 0}, {5, 0}
            };

            // Start from zero, else you will have a bad time.  This was also my problem with the tree, since I was getting 10976 for example 2
            var idx = -1;
            int nextJoltage = 0;
            while (nextJoltage != -1)
            {
                if (idx + 4 < sortedJoltages.Count && sortedJoltages.ElementAt(idx + 4).Value == nextJoltage + 4)
                {
                    consecutiveJoltagesLookup[5] = consecutiveJoltagesLookup[5] + 1;
                    idx += 5;
                }
                else if (idx + 3 < sortedJoltages.Count && sortedJoltages.ElementAt(idx + 3).Value == nextJoltage + 3)
                {
                    consecutiveJoltagesLookup[4] = consecutiveJoltagesLookup[4] + 1;
                    idx += 4;
                }
                else if (idx + 2 < sortedJoltages.Count && sortedJoltages.ElementAt(idx + 2).Value == nextJoltage + 2)
                {
                    consecutiveJoltagesLookup[3] = consecutiveJoltagesLookup[3] + 1;
                    idx += 3;
                }
                else
                {
                    idx++;
                }

                nextJoltage = idx < sortedJoltages.Count ? sortedJoltages.ElementAt(idx).Value : -1;
            }

            double result = Math.Pow(2, consecutiveJoltagesLookup[3]) * Math.Pow(4, consecutiveJoltagesLookup[4]) * Math.Pow(7, consecutiveJoltagesLookup[5]);
            Console.WriteLine($"Answer: {result}");
        }

        private static SortedList<int, int> ParseFile()
        {
            var file = new StreamReader(@"/Users/rbakken/RiderProjects/AdventOfCode/AdventOfCode/Day10/day_10.txt");
            string line;
            var result = new SortedList<int, int>();

            while ((line = file.ReadLine()) != null)
            {
                int joltage = Int32.Parse(line);
                result.Add(joltage, joltage);
            }

            file.Close();
            return result;
        }
    }
}
