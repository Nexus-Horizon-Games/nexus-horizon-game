using System;

namespace Nexus_Horizon_Game
{
    /// <summary>
    /// A static wrapper around the Random class.
    /// </summary>
    internal static class RandomGenerator
    {
        private static Random random = new Random();

        public static int GetInteger()
        {
            return random.Next();
        }

        public static int GetInteger(int maxValue)
        {
            return random.Next(maxValue);
        }

        public static int GetInteger(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }
    }
}
