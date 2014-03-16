using System;

namespace Game.Lifecicle
{
    internal static class Configuration
    {
        public static bool EnableStatistics
        {
            get { return false; }
        }

        public static Version Version
        {
            get { return new Version(1, 0, 2); }
        }

        public static uint YandexMetricaKey
        {
            get { return 0; }
        }
    }
}