using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Day10
{
    public class VoltageTreeNode
    {
        private readonly Dictionary<int, VoltageTreeNode> _children = new Dictionary<int, VoltageTreeNode>();

        public int Joltage { get; set; } = 0;
        public bool IsLeaf => !_children.Any();

        public void AddChild(VoltageTreeNode child)
        {
            _children.Add(child.Joltage, child);
        }

        public void AddChildren(SortedList<int, int> sortedJoltages)
        {
            sortedJoltages
                .Where(joltageKvp => joltageKvp.Key - Joltage > 0 && joltageKvp.Key - Joltage < 4)
                .ToList()
                .ForEach(joltageKvp =>
                {
                     var joltageNode = new VoltageTreeNode { Joltage = joltageKvp.Key };
                     _children.Add(joltageNode.Joltage, joltageNode);
                     joltageNode.AddChildren(sortedJoltages);
                });
        }

        public int CountLeaves()
        {
            return IsLeaf ? 1 : _children.Sum(node => node.Value.CountLeaves());
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            Print(builder, "", "");
            return builder.ToString();
        }

        private void Print(StringBuilder builder, String prefix, String childrenPrefix)
        {
            builder.Append(prefix);
            builder.Append(Joltage);
            builder.Append('\n');
            //Dictionary<int, VoltageTreeNode>.Enumerator enumerator = _children.GetEnumerator();

            foreach (KeyValuePair<int,VoltageTreeNode> voltageTreeNode in _children)
            {
                if (voltageTreeNode.Value.IsLeaf)
                {
                    voltageTreeNode.Value.Print(builder, $"{childrenPrefix} └──", "     ");
                }
                else
                {
                    voltageTreeNode.Value.Print(builder, $"{childrenPrefix} |--", $"{childrenPrefix} |  ");
                }
            }
        }
    }
}
