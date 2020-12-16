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

            long multiplier = 0;
            int step = 1;
            Tuple<long, long> matchingTimestamps = new Tuple<long, long>(0, 0);
            for (var i = 0; i < busOffsets.Count - 1; i++)
            {
                KeyValuePair<int, int> bus = busOffsets.ElementAt(i);
                KeyValuePair<int, int> otherBus = busOffsets.ElementAt(i + 1);

                matchingTimestamps = FindFirstMatchingTimestampFor(bus, otherBus, multiplier, step);

                multiplier = matchingTimestamps.Item2 / otherBus.Key;
                step = bus.Key;
            }

            Console.WriteLine($"Earliest Timestamp: {matchingTimestamps.Item2 - busOffsets.Last().Value}");
        }

        private static Tuple<long, long> FindFirstMatchingTimestampFor(KeyValuePair<int, int> bus, KeyValuePair<int, int> otherBus, long multiplier = 0, int step = 1)
        {
            int interval = step;
            long timestamp = bus.Key * (multiplier + interval);
            Tuple<long, long> timestamps = null;
            while (timestamps == null)
            {
                Console.WriteLine($"Trying Timestamp: {timestamp}");
                if (timestamp % bus.Key == 0 && (timestamp + otherBus.Value - bus.Value) % otherBus.Key == 0)
                {
                    timestamps = new Tuple<long, long>(timestamp, timestamp + otherBus.Value - bus.Value);
                    Console.WriteLine($"Found timestamps: {bus.Key} {timestamp} | {otherBus.Key} {timestamp + otherBus.Value - bus.Value}");
                }
                else
                {
                    interval += step;
                    timestamp = bus.Key * (multiplier + interval);
                }
            }

            return timestamps;
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
