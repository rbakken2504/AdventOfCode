namespace AdventOfCode.Day18
{
    public class ExpressionTreeNode
    {
        public ExpressionTreeNode Left { get; set; }
        public ExpressionTreeNode Right { get; set; }
        public string Operand { get; set; }
        public bool IsLeaf => Left == null && Right == null;
    }
}
