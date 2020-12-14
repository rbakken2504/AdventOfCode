using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day13
{
    public static class Part2
    {
        public static void Solve()
        {
            SortedDictionary<int, int> busOffsets = ParseBusOffsets();
            Dictionary<int, long> busTimestampFound = busOffsets.ToDictionary(kvp => kvp.Key, _ => -1L);

            int multiple = busOffsets.First().Key;
            int offset = busOffsets.First().Value;
            long timestamp = multiple;

            foreach (KeyValuePair<int,int> keyValuePair in busOffsets)
            {
                Console.WriteLine($"{keyValuePair.Key} => {keyValuePair.Value}");
            }

            while (BusTimestampNotFound(busTimestampFound))
            {
                busTimestampFound = ResetBusTimestamps(busTimestampFound);
                foreach (KeyValuePair<int,int> busOffset in busOffsets)
                {
                    if ((timestamp - offset + busOffset.Value) % busOffset.Key == 0)
                    {
                        busTimestampFound[busOffset.Key] = timestamp - offset + busOffset.Value;
                    }
                    else
                    {
                        break;
                    }
                }

                timestamp += multiple;
                Console.WriteLine($"Timestamp to Try: {timestamp}");
            }

            Console.WriteLine($"Earliest Timestamp: {busTimestampFound[busOffsets.First(kvp => kvp.Value == 0).Key]}");
        }

        private static Dictionary<int, long> ResetBusTimestamps(Dictionary<int, long> busTimestamps)
        {
            return busTimestamps.ToDictionary(kvp => kvp.Key, _ => -1L);
        }

        private static bool BusTimestampNotFound(Dictionary<int, long> busTimestamps)
        {
            return busTimestamps.Any(busTimestamp => busTimestamp.Value == -1);
        }

        private static SortedDictionary<int, int> ParseBusOffsets()
        {
            var file = new StreamReader(@"/Users/rbakken/RiderProjects/AdventOfCode/AdventOfCode/Day13/day_13.txt");
            string line;
            var result = new SortedDictionary<int, int>(Comparer<int>.Create((x, y) => y.CompareTo(x)));

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
