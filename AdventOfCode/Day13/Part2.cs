using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day13
{
    public static class Part2
    {
        public static void Solve()
        {
            Dictionary<int, int> busOffsets = ParseBusOffsets();
            Dictionary<int, long> busTimestampFound = busOffsets.ToDictionary(kvp => kvp.Key, _ => -1L);

            int multiple = busOffsets.First().Key;
            long timestamp = busOffsets.First().Key;

            while (BusTimestampNotFound(busTimestampFound))
            {
                busTimestampFound = ResetBusTimestamps(busTimestampFound);
                foreach (KeyValuePair<int,int> busOffset in busOffsets)
                {
                    if ((timestamp + busOffset.Value) % busOffset.Key == 0)
                    {
                        busTimestampFound[busOffset.Key] = timestamp;
                    }
                }

                timestamp += multiple;
            }

            Console.WriteLine($"Earliest Timestamp: {busTimestampFound[multiple]}");
        }

        private static Dictionary<int, long> ResetBusTimestamps(Dictionary<int, long> busTimestamps)
        {
            return busTimestamps.ToDictionary(kvp => kvp.Key, _ => -1L);
        }

        private static bool BusTimestampNotFound(Dictionary<int, long> busTimestamps)
        {
            return busTimestamps.Any(busTimestamp => busTimestamp.Value == -1);
        }

        private static Dictionary<int, int> ParseBusOffsets()
        {
            var file = new StreamReader(@"/Users/rbakken/RiderProjects/AdventOfCode/AdventOfCode/Day13/day_13.txt");
            string line;
            var result = new Dictionary<int, int>();

            while ((line = file.ReadLine()) != null)
            {
                var idx = 0;
                line.Split(',').ToList().ForEach(busId =>
                {
                    if (busId != "x")
                    {
                        result.Add(Int32.Parse(busId), idx);
                    }
                    idx++;
                });

            }

            file.Close();

            return result;
        }
    }
}
