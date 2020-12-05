using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day5
{
    public static class Part1
    {
        public static void Solve()
        {
            var rowRoot = new TreeNode {LowerLimit = 0, UpperLimit = 127};
            rowRoot.BuildChildren();

            var colRoot = new TreeNode {LowerLimit = 0, UpperLimit = 7};
            colRoot.BuildChildren();

            var file = new StreamReader(@"/Users/rbakken/RiderProjects/AdventOfCode/AdventOfCode/Day5/day_5.txt");
            string line;
            var seatIds = new SortedList<int, int>();
            while ((line = file.ReadLine()) != null)
            {
                char[] rowStr = line.Trim().Substring(0, 7).ToCharArray();
                char[] colStr = line.Trim().Substring(7).ToCharArray();
                TreeNode row = rowRoot;
                TreeNode col = colRoot;
                int rowNum = GetRow(row, rowStr);
                int colNum = GetColumn(col, colStr);


                int seatId = rowNum * 8 + colNum;
                seatIds.Add(seatId, seatId);
            }

            Console.WriteLine($"Highest Seat ID: {seatIds.Last()}");
        }

        private static int GetRow(TreeNode row, IReadOnlyList<char> rowStr)
        {
            for (var i = 0; i < rowStr.Count; i++)
            {
                if (i == rowStr.Count - 1)
                {
                    return rowStr[i] == 'B' ? row.UpperLimit : row.LowerLimit;
                }

                row = rowStr[i] == 'B' ? row.Upper : row.Lower;
            }

            return -1;
        }

        private static int GetColumn(TreeNode column, IReadOnlyList<char> colStr)
        {
            for (var i = 0; i < colStr.Count; i++)
            {
                if (i == colStr.Count - 1)
                {
                    return colStr[i] == 'R' ? column.UpperLimit : column.LowerLimit;
                }

                column = colStr[i] == 'R' ? column.Upper : column.Lower;
            }

            return -1;
        }
    }
}
