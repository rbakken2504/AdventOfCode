using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day8
{
    public static class Part2
    {
        public static void Solve()
        {
            var file = new StreamReader(@"/Users/rbakken/RiderProjects/AdventOfCode/AdventOfCode/Day8/day_8.txt");
            IEnumerable<Instruction> instructionsList = ParseInstructions(file);
            int accumulator = ReadInstructions(instructionsList.ToList());

            Console.WriteLine($"Acc: {accumulator}");
            file.Close();
        }

        private static int ReadInstructions(List<Instruction> instructions)
        {
            var idx = 0;
            var accumulator = 0;
            Instruction[] instructionArr = instructions.ToArray();
            var changedInstructionArr = new bool[instructions.Count];
            Instruction nextInstruction = instructionArr[idx];
            int instructionCount = instructionArr.Length;
            var opChanged = false;

            while (instructionCount >= 1)
            {
                if (!opChanged && changedInstructionArr[idx] == false && !nextInstruction.Operation.Equals(Operation.Accumulate))
                {
                    nextInstruction = new Instruction
                    {
                        Argument = nextInstruction.Argument,
                        Operation = nextInstruction.Operation.Equals(Operation.Jump) ? Operation.NoOp : Operation.Jump
                    };
                    instructionArr[idx] = nextInstruction;
                    changedInstructionArr[idx] = true;
                    opChanged = true;
                }

                instructionArr[idx] = new Instruction
                {
                    Argument = nextInstruction.Argument,
                    Operation = nextInstruction.Operation,
                    HasBeenUsed = true
                };

                nextInstruction = instructionArr[idx];
                switch (nextInstruction.Operation)
                {
                    case Operation.NoOp:
                        idx++;
                        break;
                    case Operation.Accumulate:
                        accumulator += nextInstruction.Argument;
                        idx++;
                        break;
                    case Operation.Jump:
                        idx += nextInstruction.Argument;
                        break;
                }

                instructionCount--;

                if (idx >= instructionArr.Length)
                {
                    break;
                }

                if (instructionCount > 0)
                {
                    nextInstruction = instructionArr[idx];
                }

                if (nextInstruction.HasBeenUsed)
                {
                    instructionArr = instructions.ToArray();
                    accumulator = 0;
                    idx = 0;
                    opChanged = false;
                    instructionCount = instructionArr.Length;
                    nextInstruction = instructionArr[idx];
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
