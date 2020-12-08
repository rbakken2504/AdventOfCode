using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day8
{
    public static class Part1
    {
        public static void Solve()
        {
            var file = new StreamReader(@"/Users/rbakken/RiderProjects/AdventOfCode/AdventOfCode/Day8/day_8.txt");
            string line;
            IEnumerable<Instruction> instructionsList = ParseInstructions(file);
            int accumulator = ReadInstructions(instructionsList);

            Console.WriteLine($"Acc: {accumulator}");
            file.Close();
        }

        private static int ReadInstructions(IEnumerable<Instruction> instructions)
        {
            var idx = 0;
            var accumulator = 0;
            Instruction[] instructionArr = instructions.ToArray();
            Instruction nextInstruction = instructionArr[idx];

            while (nextInstruction != null)
            {
                nextInstruction.HasBeenUsed = true;
                switch (nextInstruction.Operation)
                {
                    case Operation.NoOp:
                        idx++;
                        nextInstruction = instructionArr[idx];
                        break;
                    case Operation.Accumulate:
                        accumulator += nextInstruction.Argument;
                        idx++;
                        nextInstruction = instructionArr[idx];
                        break;
                    case Operation.Jump:
                        idx += nextInstruction.Argument;
                        nextInstruction = instructionArr[idx];
                        break;
                }

                if (nextInstruction.HasBeenUsed)
                {
                    break;
                }
            }

            return accumulator;
        }

        private static IEnumerable<Instruction> ParseInstructions(TextReader file)
        {
            var result = new List<Instruction>();
            string line;
            while ((line = file.ReadLine()) != null)
            {
                string[] operationAndArg = line.Split(' ');
                var operation = Operation.NoOp;
                switch (operationAndArg[0])
                {
                    case "nop":
                        operation = Operation.NoOp;
                        break;
                    case "acc":
                        operation = Operation.Accumulate;
                        break;
                    case "jmp":
                        operation = Operation.Jump;
                        break;
                }
                int arg = Int32.Parse(operationAndArg[1]);

                result.Add(new Instruction { Argument = arg, Operation = operation });
            }

            return result;
        }
    }
}
