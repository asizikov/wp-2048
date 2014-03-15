using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Lifecicle;
using Yandex.Metrica;

namespace Game
{
    internal static class Bootstrapper
    {
        public static void InitApplication()
        {
            if (Configuration.EnableStatistics)
            {
                Counter.CustomAppVersion = Configuration.Version;
                Counter.TrackLocationEnabled = true;
                Counter.Start(Configuration.YandexMetricaKey);
                Counter.SendEventsBuffer();
            }
        }

        public static void SaveState()
        {
            //ServiceLocator.ApplicationSettings.Save();
            //ServiceLocator.WebCache.PushToStorage();
        }
    }
}
