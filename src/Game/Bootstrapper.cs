using Game.Lifecicle;
using Yandex.Metrica;

namespace Game
{
    internal static class Bootstrapper
    {
        public static void InitApplication()
        {
            if (!Configuration.EnableStatistics) return;

            Counter.CustomAppVersion = Configuration.Version;
            Counter.TrackLocationEnabled = true;
            Counter.Start(Configuration.YandexMetricaKey);
            Counter.SendEventsBuffer();
        }
    }
}
