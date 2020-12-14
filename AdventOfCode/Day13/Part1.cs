using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Day13
{
    public static class Part1
    {
        public static void Solve()
        {
            var file = new StreamReader(@"/Users/rbakken/RiderProjects/AdventOfCode/AdventOfCode/Day13/day_13.txt");
            string line;
            int? arrivalTime = null;
            List<int> availableBuses = new List<int>();
            while ((line = file.ReadLine()) != null)
            {
                if (arrivalTime == null)
                {
                    arrivalTime = Int32.Parse(line);
                }
                else
                {
                    availableBuses = line.Split(',').Where(bus => bus != "x").Select(Int32.Parse).ToList();
                }
            }

            Tuple<int, int> bus = null;
            int currentTime = arrivalTime.GetValueOrDefault();
            while (bus == null)
            {
                availableBuses.ForEach(busId =>
                {
                    if (currentTime % busId == 0)
                    {
                        bus = new Tuple<int, int>(busId, currentTime);
                    }
                });
                currentTime++;
            }

            Console.WriteLine($"Bus ID: {bus.Item1} Arrival: {arrivalTime} Departure: {bus.Item2} Answer: {(bus.Item2 - arrivalTime) * bus.Item1}");

            file.Close();
        }
    }
}
