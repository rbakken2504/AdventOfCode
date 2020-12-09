using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day9
{
    public static class Part2
    {
        public static void Solve()
        {
            long numToFind = 26134589;
            long curSum = 0;

            long[] numbers = ParseNumbers().ToArray();
            var summedNumbers = new List<long>();
            while (curSum < numToFind)
            {
                for (int i = 0; i < numbers.Length && curSum != numToFind; i++)
                {
                    curSum += numbers[i];
                    summedNumbers.Add(numbers[i]);
                    for (int j = i + 1; j < numbers.Length; j++)
                    {
                        curSum += numbers[j];
                        summedNumbers.Add(numbers[j]);
                        if (curSum == numToFind && summedNumbers.Count > 1)
                        {
                            summedNumbers.Sort();
                            Console.WriteLine($"Weakness: {summedNumbers.First() + summedNumbers.Last()}");
                            break;
                        }

                        if (curSum > numToFind)
                        {
                            curSum = 0;
                            summedNumbers = new List<long>();
                            break;
                        }
                    }
                }
            }
        }

        private static IEnumerable<long> ParseNumbers()
        {
            var file = new StreamReader(@"/Users/rbakken/RiderProjects/AdventOfCode/AdventOfCode/Day9/day_9.txt");
            string line;
            var numbers = new List<long>();

            while ((line = file.ReadLine()) != null)
            {
                numbers.Add(Int64.Parse(line));
            }

            file.Close();

            return numbers;
        }
    }
}
