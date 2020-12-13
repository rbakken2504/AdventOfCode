using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day11
{
    public static class Part2
    {
        public static void Solve()
        {
            Dictionary<int, char[]> originalSeatMap = ParseSeatMap();
            Dictionary<int, char[]> updatedSeatMap = DeepCloneDictionary(originalSeatMap);

            int seatChanges;
            do
            {
                seatChanges = 0;
                originalSeatMap = DeepCloneDictionary(updatedSeatMap);
                for (var rowNum = 0; rowNum < originalSeatMap.Count; rowNum++)
                {
                    char[] row = originalSeatMap[rowNum];
                    for (var seatNum = 0; seatNum < row.Length; seatNum++)
                    {
                        if (originalSeatMap[rowNum][seatNum] != '.')
                        {
                            if (SeatIsEmpty(originalSeatMap, rowNum, seatNum) && NoOccupiedAdjacentSeats(originalSeatMap, rowNum, seatNum))
                            {
                                updatedSeatMap[rowNum][seatNum] = '#';
                                seatChanges++;
                            }
                            else if (SeatIsOccupied(originalSeatMap, rowNum, seatNum) && FiveOrMoreAdjacentSeatsOccupied(originalSeatMap, rowNum, seatNum))
                            {
                                updatedSeatMap[rowNum][seatNum] = 'L';
                                seatChanges++;
                            }
                        }
                    }
                }
            } while (seatChanges != 0);

            Console.WriteLine($"Occupied Seats: {CountOccupiedSeats(updatedSeatMap)}");
        }

        private static int CountOccupiedSeats(Dictionary<int, char[]> seatMap)
        {
            return seatMap.Sum(kvp => kvp.Value.Sum(seat => seat == '#' ? 1 : 0));
        }

        private static Dictionary<int, char[]> DeepCloneDictionary(Dictionary<int, char[]> dictToClone)
        {
            return dictToClone.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Select(c => c).ToArray());
        }

        private static bool IsFirstSeenSeatEmpty(IReadOnlyDictionary<int, char[]> seatMap, int rowNum, int seatNum, SearchDirection dir)
        {
            int rowIdxModifier = GetRowIndexModifier(dir);
            int colIdxModifier = GetColIndexModifier(dir);
            int curRowIdx = rowNum + rowIdxModifier;
            int curColIdx = seatNum + colIdxModifier;
            char? seat = null;
            while (seat == null && EvaluateLoopCondition(dir, curRowIdx, curColIdx, seatMap.Count, seatMap[rowNum].Length))
            {
                char curSeat = seatMap[curRowIdx][curColIdx];
                if (curSeat != '.')
                {
                    seat = curSeat;
                }
                else
                {
                    curRowIdx += rowIdxModifier;
                    curColIdx += colIdxModifier;
                }
            }

            return SeatIsEmpty(seatMap, curRowIdx, curColIdx);
        }

        private static bool IsFirstSeenSeatOccupied(IReadOnlyDictionary<int, char[]> seatMap, int rowNum, int seatNum, SearchDirection dir)
        {
            int rowIdxModifier = GetRowIndexModifier(dir);
            int colIdxModifier = GetColIndexModifier(dir);
            int curRowIdx = rowNum + rowIdxModifier;
            int curColIdx = seatNum + colIdxModifier;
            char? seat = null;
            while (seat == null && EvaluateLoopCondition(dir, curRowIdx, curColIdx, seatMap.Count, seatMap[rowNum].Length))
            {
                char curSeat = seatMap[curRowIdx][curColIdx];
                if (curSeat != '.')
                {
                    seat = curSeat;
                }
                else
                {
                    curRowIdx += rowIdxModifier;
                    curColIdx += colIdxModifier;
                }
            }

            return SeatIsOccupied(seatMap, curRowIdx, curColIdx);
        }

        private static int GetRowIndexModifier(SearchDirection direction)
        {
            switch (direction)
            {
                case SearchDirection.UpLeft:
                case SearchDirection.Up:
                case SearchDirection.UpRight:
                    return -1;
                case SearchDirection.DownRight:
                case SearchDirection.Down:
                case SearchDirection.DownLeft:
                    return 1;
                case SearchDirection.Right:
                case SearchDirection.Left:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        private static int GetColIndexModifier(SearchDirection direction)
        {
            switch (direction)
            {
                case SearchDirection.UpLeft:
                case SearchDirection.Left:
                case SearchDirection.DownLeft:
                    return -1;
                case SearchDirection.DownRight:
                case SearchDirection.Right:
                case SearchDirection.UpRight:
                    return 1;
                case SearchDirection.Up:
                case SearchDirection.Down:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        private static bool EvaluateLoopCondition(SearchDirection dir, int row, int col, int rowCount, int colCount)
        {
            return dir switch
            {
                SearchDirection.Up        => row >= 0,
                SearchDirection.UpRight   => row >=0 && col < colCount,
                SearchDirection.Right     => col < colCount,
                SearchDirection.DownRight => row < rowCount && col < colCount,
                SearchDirection.Down      => row < rowCount,
                SearchDirection.DownLeft  => row < rowCount && col >= 0,
                SearchDirection.Left      => col >= 0,
                SearchDirection.UpLeft    => row >= 0 && col >= 0,
                _                         => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };
        }

        private static bool NoOccupiedAdjacentSeats(IReadOnlyDictionary<int, char[]> seatMap, int rowNum, int seatNum)
        {
            bool topLeftOpen = IsFirstSeenSeatEmpty(seatMap, rowNum, seatNum, SearchDirection.UpLeft);
            bool topOpen = IsFirstSeenSeatEmpty(seatMap, rowNum, seatNum, SearchDirection.Up);
            bool topRightOpen = IsFirstSeenSeatEmpty(seatMap, rowNum, seatNum, SearchDirection.UpRight);
            bool leftOpen = IsFirstSeenSeatEmpty(seatMap, rowNum, seatNum, SearchDirection.Left);
            bool rightOpen = IsFirstSeenSeatEmpty(seatMap, rowNum, seatNum, SearchDirection.Right);
            bool bottomLeftOpen = IsFirstSeenSeatEmpty(seatMap, rowNum, seatNum, SearchDirection.DownLeft);
            bool bottomOpen = IsFirstSeenSeatEmpty(seatMap, rowNum, seatNum, SearchDirection.Down);
            bool bottomRightOpen = IsFirstSeenSeatEmpty(seatMap, rowNum, seatNum, SearchDirection.DownRight);

            return topLeftOpen && topOpen && topRightOpen &&
                   leftOpen && rightOpen &&
                   bottomLeftOpen && bottomOpen && bottomRightOpen;
        }

        private static bool FiveOrMoreAdjacentSeatsOccupied(IReadOnlyDictionary<int, char[]> seatMap, int rowNum, int seatNum)
        {
            var occupiedSeats = 0;
            if (IsFirstSeenSeatOccupied(seatMap, rowNum, seatNum, SearchDirection.DownLeft))
            {
                occupiedSeats++;
            }
            if (IsFirstSeenSeatOccupied(seatMap, rowNum, seatNum, SearchDirection.Down))
            {
                occupiedSeats++;
            }
            if (IsFirstSeenSeatOccupied(seatMap, rowNum, seatNum, SearchDirection.DownRight))
            {
                occupiedSeats++;
            }
            if (IsFirstSeenSeatOccupied(seatMap, rowNum, seatNum, SearchDirection.Right))
            {
                occupiedSeats++;
            }
            if (IsFirstSeenSeatOccupied(seatMap, rowNum, seatNum, SearchDirection.UpRight))
            {
                occupiedSeats++;
                if (occupiedSeats == 5)
                {
                    return true;
                }
            }
            if (IsFirstSeenSeatOccupied(seatMap, rowNum, seatNum, SearchDirection.Up))
            {
                occupiedSeats++;
                if (occupiedSeats >= 5)
                {
                    return true;
                }
            }
            if (IsFirstSeenSeatOccupied(seatMap, rowNum, seatNum, SearchDirection.UpLeft))
            {
                occupiedSeats++;
                if (occupiedSeats >= 5)
                {
                    return true;
                }
            }
            if (IsFirstSeenSeatOccupied(seatMap, rowNum, seatNum, SearchDirection.Left))
            {
                occupiedSeats++;
                if (occupiedSeats >= 5)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool SeatIsEmpty(IReadOnlyDictionary<int, char[]> seatMap, int rowNum, int seatNum)
        {
            if (rowNum >= 0 && rowNum < seatMap.Count)
            {
                char[] row = seatMap[rowNum];
                if (seatNum >= 0 && seatNum < row.Length)
                {
                    return seatMap[rowNum][seatNum] != '#';
                }

                return true;
            }

            return true;
        }

        private static bool SeatIsOccupied(IReadOnlyDictionary<int, char[]> seatMap, int rowNum, int seatNum)
        {
            if (rowNum >= 0 && rowNum < seatMap.Count)
            {
                char[] row = seatMap[rowNum];
                if (seatNum >= 0 && seatNum < row.Length)
                {
                    return seatMap[rowNum][seatNum] == '#';
                }

                return false;
            }

            return false;
        }

        private static void PrintSeatMap(IReadOnlyDictionary<int, char[]> seatMap)
        {
            foreach (KeyValuePair<int,char[]> keyValuePair in seatMap)
            {
                Console.WriteLine(keyValuePair.Value);
            }
        }

        private static Dictionary<int, char[]> ParseSeatMap()
        {
            var seatMap = new Dictionary<int, char[]>();
            var file = new StreamReader(@"/Users/rbakken/RiderProjects/AdventOfCode/AdventOfCode/Day11/day_11.txt");
            string line;
            var rowNum = 0;

            while ((line = file.ReadLine()) != null)
            {
                seatMap.Add(rowNum, line.ToCharArray());
                rowNum++;
            }

            file.Close();
            return seatMap;
        }

        private enum SearchDirection
        {
            Up,
            UpRight,
            Right,
            DownRight,
            Down,
            DownLeft,
            Left,
            UpLeft
        }
    }
}
