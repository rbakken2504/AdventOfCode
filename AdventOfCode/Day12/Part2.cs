using System;
using System.IO;

namespace AdventOfCode.Day12
{
    public static class Part2
    {
        public static void Solve()
        {
            var file = new StreamReader(@"/Users/rbakken/RiderProjects/AdventOfCode/AdventOfCode/Day12/day_12.txt");
            string line;

            // Tuple representing a position as (x, y) coordinates, where negative x represents west, and negative y represents south
            var waypoint = new Tuple<int, int>(10, 1);
            var shipPosition = new Tuple<int, int>(0, 0);
            while ((line = file.ReadLine()) != null)
            {
                Action action = ParseAction(line.Substring(0, 1));
                int value = Int32.Parse(line.Substring(1));

                switch (action)
                {
                    case Action.West:
                    case Action.East:
                    case Action.South:
                    case Action.North:
                        waypoint = UpdateWaypointPosition(waypoint, action, value);
                        break;
                    case Action.Left:
                    case Action.Right:
                        waypoint = RotateWaypoint(waypoint, action, value);
                        break;
                    case Action.Forward:
                        shipPosition = UpdateShipPosition(shipPosition, waypoint, value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Console.WriteLine($"Ship Position (x, y): ({shipPosition.Item1}, {shipPosition.Item2})");
            }

            Console.WriteLine($"Manhattan Distance: {Math.Abs(shipPosition.Item1) + Math.Abs(shipPosition.Item2)}");
            file.Close();
        }

        /**
         * For every ninety degree turn, we flip x and y coordinates, and invert the sign on the Y coordinate if rotating clockwise, else invert the sign on the X coordinate
         */
        private static Tuple<int, int> RotateWaypoint(Tuple<int, int> waypoint, Action action, int value)
        {
            var updatedWaypoint = new Tuple<int, int>(waypoint.Item1, waypoint.Item2);
            for (int ninetyDegreeTurns = value / 90; ninetyDegreeTurns > 0; ninetyDegreeTurns--)
            {
                updatedWaypoint = action.Equals(Action.Right)
                    ? new Tuple<int, int>(updatedWaypoint.Item2, updatedWaypoint.Item1 * -1)
                    : new Tuple<int, int>(updatedWaypoint.Item2 * -1, updatedWaypoint.Item1);
            }

            return updatedWaypoint;
        }

        private static Tuple<int, int> UpdateShipPosition(Tuple<int, int> shipPosition, Tuple<int, int> waypointPosition, int value)
        {
            (int shipX, int shipY) = shipPosition;
            (int waypointX, int waypointY) = waypointPosition;
            return new Tuple<int, int>(shipX + waypointX * value, shipY + waypointY * value);
        }

        private static Tuple<int, int> UpdateWaypointPosition(Tuple<int, int> currentPosition, Action action, int value)
        {
            return action switch
            {
                Action.North   => new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 + value),
                Action.South   => new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 - value),
                Action.East    => new Tuple<int, int>(currentPosition.Item1 + value, currentPosition.Item2),
                Action.West    => new Tuple<int, int>(currentPosition.Item1 - value, currentPosition.Item2)
            };
        }

        private static Action ParseAction(string action)
        {
            return action switch
            {
                "N" => Action.North,
                "S" => Action.South,
                "E" => Action.East,
                "W" => Action.West,
                "L" => Action.Left,
                "R" => Action.Right,
                "F" => Action.Forward,
                _   => throw new ArgumentOutOfRangeException(nameof(action), action, null)
            };
        }

        private enum Action
        {
            North,
            South,
            East,
            West,
            Left,
            Right,
            Forward
        }
    }
}
