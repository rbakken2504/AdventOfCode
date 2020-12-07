using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day6
{
    public static class Part2
    {
        private static int _answeredQuestions;
        private static Dictionary<char, int> _groupAnsweredQuestions = new Dictionary<char, int>();
        private static int _groupSize;

        public static void Solve()
        {
            var file = new StreamReader(@"/Users/rbakken/RiderProjects/AdventOfCode/AdventOfCode/Day6/day_6.txt");
            string line;

            while ((line = file.ReadLine()) != null)
            {
                if (String.IsNullOrWhiteSpace(line))
                {
                    CountGroupAnswers();
                }
                else
                {
                    char[] yesAnswers = line.ToCharArray();
                    foreach (char c in yesAnswers)
                    {
                        if (!_groupAnsweredQuestions.ContainsKey(c))
                        {
                            _groupAnsweredQuestions.Add(c, 1);
                        }
                        else
                        {
                            _groupAnsweredQuestions[c] = _groupAnsweredQuestions[c] + 1;
                        }
                    }
                    _groupSize++;
                }
            }

            // Last group has no empty line after it, so tally those up as well first
            CountGroupAnswers();
            Console.WriteLine($"Answers: {_answeredQuestions}");
        }

        private static void CountGroupAnswers()
        {
            _answeredQuestions += _groupAnsweredQuestions.Count(kvp => kvp.Value == _groupSize);
            _groupAnsweredQuestions = new Dictionary<char, int>();
            _groupSize = 0;
        }
    }
}
