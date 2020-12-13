using System;
using System.IO;

namespace AdventOfCode.Day12
{
    public static class Part1
    {
        public static void Solve()
        {
            var file = new StreamReader(@"/Users/rbakken/RiderProjects/AdventOfCode/AdventOfCode/Day12/day_12.txt");
            string line;

            var shipDirection = Direction.East;
            // Tuple representing ship position as (x, y) coordinates, where negative x represents west, and negative y represents south
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
                        shipPosition = UpdateShipPosition(shipPosition, action, value);
                        break;
                    case Action.Left:
                        shipDirection = UpdateShipDirection(shipDirection, action, value);
                        break;
                    case Action.Right:
                        shipDirection = UpdateShipDirection(shipDirection, action, value);
                        break;
                    case Action.Forward:
                        shipPosition = UpdateShipPosition(shipPosition, ToAction(shipDirection), value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                Console.WriteLine($"Ship Position (x, y): ({shipPosition.Item1}, {shipPosition.Item2})");
            }

            Console.WriteLine($"Manhattan Distance: {Math.Abs(shipPosition.Item1) + Math.Abs(shipPosition.Item2)}");
            file.Close();
        }

        private static Action ToAction(Direction direction)
        {
            return direction switch
            {
                Direction.North => Action.North,
                Direction.East  => Action.East,
                Direction.South => Action.South,
                Direction.West  => Action.West,
                _               => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        private static Tuple<int, int> UpdateShipPosition(Tuple<int, int> currentPosition, Action action, int value)
        {
            return action switch
            {
                Action.North   => new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 + value),
                Action.South   => new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 - value),
                Action.East    => new Tuple<int, int>(currentPosition.Item1 + value, currentPosition.Item2),
                Action.West    => new Tuple<int, int>(currentPosition.Item1 - value, currentPosition.Item2)
            };
        }

        private static Direction UpdateShipDirection(Direction shipDirection, Action action, int value)
        {
            int updatedDir;
            if (action.Equals(Action.Left))
            {
                updatedDir = (int) shipDirection - value;
                return updatedDir < 0 ? (updatedDir + 360).ToEnum<Direction>() : updatedDir.ToEnum<Direction>();
            }

            updatedDir = (int) shipDirection + value;
            return updatedDir >= 360 ? (updatedDir % 360).ToEnum<Direction>() : updatedDir.ToEnum<Direction>();
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

        private enum Direction
        {
            North = 0,
            East = 90,
            South = 180,
            West = 270
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
