using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day9
{
    public static class Part1
    {
        public static void Solve()
        {
            var preambleLen = 25;
            var preambleStart = 0;

            List<long> numbers = ParseNumbers().ToList();

            for (int i = preambleLen; i < numbers.Count; i++)
            {
                IEnumerable<long> preamble = numbers.GetRange(preambleStart, preambleLen).ToList();
                long curVal = numbers[i];
                var matchFound = false;
                for (var j = 0; j < preamble.Count(); j++)
                {
                    long first = preamble.ElementAt(j);
                    for (int k = j + 1; k < preamble.Count(); k++)
                    {
                        long second = preamble.ElementAt(k);
                        if (curVal == first + second)
                        {
                            matchFound = true;
                            break;
                        }
                        Console.WriteLine($"{curVal}: {first} + {second} = {first + second}");
                    }

                    if (matchFound) break;
                }

                if (!matchFound)
                {
                    Console.WriteLine($"Invalid: {curVal}");
                    break;
                }

                preambleStart++;
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
