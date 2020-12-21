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
            List<int> sortedBusIds = busOffsets.Skip(1).Select(bus => bus.Key).OrderByDescending(id => id).ToList();

            KeyValuePair<int, int> firstBus = busOffsets.First();
            busOffsets.Remove(firstBus.Key);

            Dictionary<int, long> factors = busOffsets.ToDictionary(
                bus => bus.Key,
                bus => FindFirstMatchingTimestampFor(firstBus, bus).Item1 / firstBus.Key
            );

            var timestampFound = false;
            var i = 3270237242L;
            var timestamp = 0L;
            while (!timestampFound)
            {
                int highestBusId = sortedBusIds.First();
                timestamp = firstBus.Key * (factors[highestBusId] + highestBusId * i);
                Console.WriteLine($"Trying Timestamp: ({i}) {timestamp}");
                if ((timestamp + busOffsets[highestBusId]) % highestBusId == 0)
                {
                    if (CheckAgainstRemainingBuses(timestamp, busOffsets, sortedBusIds))
                    {
                        timestampFound = true;
                    }
                }

                i++;
            }
            Console.WriteLine($"Answer: {timestamp}");

            /*List<Tuple<long, int>> relationships = busOffsets
                                                   .Select(bus =>
                                                   {
                                                       Tuple<long, long> matchingTimestamps = FindFirstMatchingTimestampFor(firstBus, bus);
                                                       return new Tuple<long, int>(matchingTimestamps.Item1, bus.Key);
                                                   })
                                                   .ToList();



            var listOfFactors = new List<List<long>>();
            for (var i = 0; i < relationships.Count; i++)
            {
                (long firstTimestamp, int step) = relationships[i];
                long multiplier = firstTimestamp / firstBus.Key;

                var list = new List<long>();
                for (long j = multiplier; j < 100000000000; j += step)
                {
                    if ((j + step) * firstBus.Key > 370000000592)
                    {
                        list.Add(j + step);
                    }
                }
                listOfFactors.Add(list);
            }

            stopwatch.Stop();
            Console.WriteLine($"Answer: {result.FirstOrDefault() * firstBus.Key}");
            Console.WriteLine($"Elapsed: {stopwatch.ElapsedMilliseconds}");*/
        }

        private static bool CheckAgainstRemainingBuses(long timestamp, IReadOnlyDictionary<int, int> busOffsets, IEnumerable<int> sortedBusIds)
        {
            return sortedBusIds.Skip(1).All(busId => (timestamp + busOffsets[busId]) % busId == 0);
        }

        private static Tuple<long, long> FindFirstMatchingTimestampFor(KeyValuePair<int, int> bus, KeyValuePair<int, int> otherBus, long multiplier = 0, int step = 1)
        {
            int interval = step;
            long timestamp = bus.Key * (multiplier + interval);
            Tuple<long, long> timestamps = null;
            while (timestamps == null)
            {
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
