using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yandex.Metrica;

namespace Game.Lifecicle
{
    public static class StatisticsService

    {
        private static void PublishEvent(string eventName)
        {
            Counter.ReportEvent(eventName);
        }

        private const string GamePageLoaded = "Game page loaded";
        private const string AboutPageLoaded = "About page loaded";
        private const string Won = "We have a winner";


        public static void PublishAboutPageLoaded()
        {
            PublishEvent(AboutPageLoaded);
        }

        public static void ReportGamePageLoaded()
        {
            PublishEvent(GamePageLoaded);
        }

        public static void PublishWon()
        {
            PublishEvent(Won);
        }
    }
}
