using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day18
{
    public static class Part2
    {
        public static void Solve()
        {
            List<string> postfixExpressions = ParseFileToPostfix();
            postfixExpressions.ForEach(Console.WriteLine);
            IEnumerable<long> solvedExpressions = SolveExpressions(postfixExpressions);

            Console.WriteLine($"Answer: {solvedExpressions.Sum()}");
        }

        private static List<string> ParseFileToPostfix()
        {
            var file = new StreamReader(@"/Users/rbakken/RiderProjects/AdventOfCode/AdventOfCode/Day18/day_18.txt");
            string line;
            var postfixExpressions = new List<string>();

            while ((line = file.ReadLine()) != null)
            {
                var expressionParts = new List<string>();
                var curPart = "";
                var addExpressionPart = new Action<string>(expPart =>
                {
                    if (!String.IsNullOrEmpty(expPart))
                    {
                        expressionParts.Add(expPart);
                        curPart = "";
                    }
                });


                foreach (char c in line)
                {
                    if (c == '+' || c == '*' || Char.IsDigit(c))
                    {
                        curPart += $" {c}";
                    }
                    else if (c == '(' || c == ')')
                    {
                        addExpressionPart(curPart);
                        expressionParts.Add(c.ToString());
                    }
                    else
                    {
                        addExpressionPart(curPart);
                    }
                }

                if (!String.IsNullOrEmpty(curPart))
                {
                    addExpressionPart(curPart);
                }

                postfixExpressions.Add(InfixToPostfix(expressionParts).Trim());
            }

            file.Close();

            return postfixExpressions;
        }

        private static string InfixToPostfix(IEnumerable<string> expressionParts)
        {
            var operatorStack = new Stack<string>();
            var postfixExpression = "";
            var operatorPrecedence = new Dictionary<string, int>
            {
                {"(", 3},
                {"+", 2},
                {"*", 1}
            };

            foreach (string expressionPart in expressionParts)
            {
                if (IsOperand(expressionPart))
                {
                    postfixExpression += $" {expressionPart}";
                }
                else if (expressionPart == ")")
                {
                    string oper = operatorStack.Pop();
                    while (oper != "(")
                    {
                        postfixExpression += oper;
                        oper = operatorStack.Pop();
                    }
                }
                else
                {
                    if (operatorStack.Count == 0 || operatorPrecedence[expressionPart.Trim()] >= operatorPrecedence[operatorStack.Peek().Trim()])
                    {
                        operatorStack.Push(expressionPart);
                    }
                    else
                    {
                        var stackElement = operatorStack.Any() ? operatorStack.Peek().Trim() : null;
                        while (stackElement != null && stackElement != "(")
                        {
                            if (operatorPrecedence[expressionPart.Trim()] <= operatorPrecedence[stackElement])
                            {
                                postfixExpression += $" {operatorStack.Pop()}";
                                stackElement = operatorStack.Any() ? operatorStack.Peek().Trim() : null;
                            }
                        }
                        operatorStack.Push(expressionPart);
                    }
                }
            }

            return operatorStack.Aggregate(postfixExpression, (current, s) => current + $" {s}");
        }

        private static IEnumerable<long> SolveExpressions(IEnumerable<string> postfixExpressions)
        {
            var stack = new Stack<string>();
            var results = new List<long>();
            foreach (string postfixExpression in postfixExpressions)
            {
                string[] terms = postfixExpression.Split(' ').Where(t => !String.IsNullOrEmpty(t)).ToArray();
                foreach (string term in terms)
                {
                    if (IsOperand(term.Trim()))
                    {
                        stack.Push(term.Trim());
                    }
                    else
                    {
                        long t1 = Int64.Parse(stack.Pop());
                        long t2 = Int64.Parse(stack.Pop());
                        long result = term == "*" ? t1 * t2 : t1 + t2;
                        stack.Push(result.ToString());
                    }
                }
                results.Add(Int64.Parse(stack.Pop()));
            }

            return results;
        }

        private static bool IsOperator(string str)
        {
            return str == "*" || str == "+";
        }

        private static bool IsOperand(string str)
        {
            return Int32.TryParse(str, out _);
        }
    }
}
