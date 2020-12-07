using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day6
{
    public static class Part1
    {
        public static void Solve()
        {
            var file = new StreamReader(@"/Users/rbakken/RiderProjects/AdventOfCode/AdventOfCode/Day6/day_6.txt");
            string line;

            var answeredQuestions = new Dictionary<char, int>();
            var groupAnsweredQuestions = new Dictionary<char, bool>();
            while ((line = file.ReadLine()) != null)
            {
                if (String.IsNullOrWhiteSpace(line))
                {
                    groupAnsweredQuestions = new Dictionary<char, bool>();
                }
                else
                {
                    char[] yesAnswers = line.ToCharArray();
                    foreach (char c in yesAnswers)
                    {
                        if (!groupAnsweredQuestions.ContainsKey(c))
                        {
                            groupAnsweredQuestions.Add(c, true);
                            if (!answeredQuestions.ContainsKey(c))
                            {
                                answeredQuestions.Add(c, 1);
                            }
                            else
                            {
                                answeredQuestions[c] = answeredQuestions[c] + 1;
                            }
                        }
                    }
                }
            }
            Console.WriteLine($"Answers: {answeredQuestions.Sum(kvp => kvp.Value)}");
        }
    }
}
