using System;

namespace AdventOfCode
{
    public class SeatMap
    {
        private readonly string[,] _seatMap = new string[128,8];

        public void MarkOccupied(int row, int col)
        {
            _seatMap[row, col] = "X";
        }

        /**
         * Could update the TreeNode to do a binary search for the open seat, instead of creating this class, but fuck it.
         */
        public int FindSeatId()
        {
            var firstSeatFound = false;
            for (var row = 1; row < 128; row++)
            {
                for (var column = 0; column < 8; column++)
                {
                    if (_seatMap[row, column] == "X")
                    {
                        if (!firstSeatFound)
                        {
                            firstSeatFound = true;
                        }
                    }

                    if (_seatMap[row, column] != "X")
                    {
                        if (firstSeatFound)
                        {
                            return row * 8 + column;
                        }
                    }
                }
            }

            return -1;
        }

        public void PrintSeatMap()
        {
            for (var row = 0; row < 128; row++)
            {
                for (var column = 0; column < 8; column++)
                {
                    Console.Write(_seatMap[row, column] == null ? "O" : "X");
                }
                Console.WriteLine();
            }
        }
    }
}
