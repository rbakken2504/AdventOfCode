using System;

namespace AdventOfCode.Day12
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }

        public static T ToEnum<T>(this int value)
        {
            return Enum.GetName(typeof(T), value).ToEnum<T>();
        }
    }
}
