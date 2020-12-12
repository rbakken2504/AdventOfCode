using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Day10

{
    public static class Part1
    {
        public static void Solve()
        {
            SortedList<int, int> sortedJoltages = ParseFile();
            // 3 => 1 to account for device adapter
            var joltageDifferences = new Dictionary<int, int> { {1, 0}, {2, 0}, {3, 1} };
            var outletEffectiveRating = 0;

            foreach (KeyValuePair<int,int> keyValuePair in sortedJoltages)
            {
                int currentJoltage = keyValuePair.Key;
                int difference = currentJoltage - outletEffectiveRating;
                if (difference <= 3 && difference > 0)
                {
                    outletEffectiveRating += difference;
                    joltageDifferences[difference] = joltageDifferences[difference] + 1;
                }
            }

            Console.WriteLine($"Answer: {joltageDifferences[1] * joltageDifferences[3]}");
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
