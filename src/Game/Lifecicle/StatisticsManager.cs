using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Game.View;
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
        private const string ShareClick = "Share result button clicked";
        private const string SettingsClick = "settings button clicked";
        private const string LeaderboardClick = "leaderboard button clicked";


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

        public static void PublishShareResultClick()
        {
            PublishEvent(ShareClick);
        }

        public static void PublishSettingsClicked()
        {
            PublishEvent(SettingsClick);
        }

        public static void PublishLeaderboardClicked()
        {
            PublishEvent(LeaderboardClick);
        }

        public static void ReportBgColor(BackgroundItem bgColor)
        {
            PublishEvent("Background Color is " + ( bgColor.Value ==  "#328FDB" ? "default" : "dark") );
        }

        public static void PublishTryAppClicked(string name)
        {
            PublishEvent("Try other app clicked: " + name);
        }
    }
}
