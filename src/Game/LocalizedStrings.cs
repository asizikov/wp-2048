using Game.Resources;

namespace Game
{
    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    public class LocalizedStrings
    {
        private static AppResources _s = new AppResources();

        public AppResources S { get { return _s; } }
    }
}