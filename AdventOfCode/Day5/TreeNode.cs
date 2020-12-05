using System;

namespace AdventOfCode
{
    public class TreeNode
    {
        public TreeNode Lower { get; set; }
        public TreeNode Upper { get; set; }
        public int LowerLimit { get; set; }
        public int UpperLimit { get; set; }

        public void BuildChildren()
        {
            if (UpperLimit - LowerLimit < 1)
            {
                Lower = this;
                Upper = this;
            }
            else
            {
                Lower = new TreeNode
                {
                    LowerLimit = LowerLimit,
                    UpperLimit = UpperLimit - ((UpperLimit - LowerLimit) / 2 + 1)
                };
                Lower.BuildChildren();
                Upper = new TreeNode
                {
                    LowerLimit = UpperLimit - (UpperLimit - LowerLimit) / 2,
                    UpperLimit = UpperLimit
                };
                Upper.BuildChildren();
            }
        }

        public void PrintTree()
        {
            if (Lower != null && Upper != null)
            {
                Console.WriteLine($"{LowerLimit} - {UpperLimit}");
                Lower.PrintTree();
                Upper.PrintTree();
            }
        }
    }
}
