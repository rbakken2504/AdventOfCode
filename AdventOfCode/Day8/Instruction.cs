namespace AdventOfCode.Day8
{
    public class Instruction
    {
        public Operation Operation { get; set; }
        public int Argument { get; set; }
        public bool HasBeenUsed { get; set; }
    }

    public enum Operation
    {
        NoOp,
        Accumulate,
        Jump
    }
}
