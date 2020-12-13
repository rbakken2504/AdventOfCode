using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day11
{
    public static class Part1
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
                            else if (SeatIsOccupied(originalSeatMap, rowNum, seatNum) && FourOrMoreAdjacentSeatsOccupied(originalSeatMap, rowNum, seatNum))
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

        private static bool NoOccupiedAdjacentSeats(IReadOnlyDictionary<int, char[]> seatMap, int rowNum, int seatNum)
        {
            bool topLeftOpen = SeatIsEmpty(seatMap, rowNum - 1, seatNum - 1);
            bool topOpen = SeatIsEmpty(seatMap, rowNum - 1, seatNum);
            bool topRightOpen = SeatIsEmpty(seatMap, rowNum - 1, seatNum + 1);
            bool leftOpen = SeatIsEmpty(seatMap, rowNum, seatNum - 1);
            bool rightOpen = SeatIsEmpty(seatMap, rowNum, seatNum + 1);
            bool bottomLeftOpen = SeatIsEmpty(seatMap, rowNum + 1, seatNum - 1);
            bool bottomOpen = SeatIsEmpty(seatMap, rowNum + 1, seatNum);
            bool bottomRightOpen = SeatIsEmpty(seatMap, rowNum + 1, seatNum + 1);

            return topLeftOpen && topOpen && topRightOpen &&
                   leftOpen && rightOpen &&
                   bottomLeftOpen && bottomOpen && bottomRightOpen;
        }

        private static bool FourOrMoreAdjacentSeatsOccupied(IReadOnlyDictionary<int, char[]> seatMap, int rowNum, int seatNum)
        {
            var occupiedSeats = 0;
            if (SeatIsOccupied(seatMap, rowNum - 1, seatNum - 1))
            {
                occupiedSeats++;
            }
            if (SeatIsOccupied(seatMap, rowNum - 1, seatNum))
            {
                occupiedSeats++;
            }
            if (SeatIsOccupied(seatMap, rowNum - 1, seatNum + 1))
            {
                occupiedSeats++;
            }
            if (SeatIsOccupied(seatMap, rowNum, seatNum - 1))
            {
                occupiedSeats++;
            }
            if (SeatIsOccupied(seatMap, rowNum, seatNum + 1))
            {
                occupiedSeats++;
            }
            if (SeatIsOccupied(seatMap, rowNum + 1, seatNum - 1))
            {
                occupiedSeats++;
            }
            if (SeatIsOccupied(seatMap, rowNum + 1, seatNum))
            {
                occupiedSeats++;
            }
            if (SeatIsOccupied(seatMap, rowNum + 1, seatNum + 1))
            {
                occupiedSeats++;
            }

            return occupiedSeats >= 4;
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
    }
}
