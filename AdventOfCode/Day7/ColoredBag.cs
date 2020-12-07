using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day7
{
    public class ColoredBag
    {
        public Dictionary<string, int> ContainedBags { get; set; }
        public string Color { get; set; }
        public int? Count { get; set; }

        public bool CanContainBag(string color, Dictionary<string, ColoredBag> bagDefinitions)
        {
            return ContainedBags.ContainsKey(color) || ContainedBags.Any(kvp => bagDefinitions[kvp.Key].CanContainBag(color, bagDefinitions));
        }

        public int TotalContainedBags(Dictionary<string, ColoredBag> bagDefinitions)
        {
            return ContainedBags.Sum(keyValuePair => keyValuePair.Value + keyValuePair.Value * bagDefinitions[keyValuePair.Key].TotalContainedBags(bagDefinitions));
        }
    }
}
