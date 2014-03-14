using System.IO.IsolatedStorage;
using GameEngine;
using Newtonsoft.Json;

namespace Game.Utils
{
    public class ApplicationSettings
    {
        private const string KEY = "GameState";

        public ApplicationSettings()
        {
        }

        public bool HasStoredGame
        {
            get { return IsolatedStorageSettings.ApplicationSettings.Contains(KEY); }
        }


        private static GameState Load()
        {
            GameState state = null;
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(KEY))
            {
                IsolatedStorageSettings.ApplicationSettings.Add(KEY, SerializeToStrng(state));
            }
            else
            {
                var favsJsonString = (string)IsolatedStorageSettings.ApplicationSettings[KEY];
                state = DeserializeFromString(favsJsonString);
            }
            return state;
        }

        private static GameState DeserializeFromString(string favsJsonString)
        {
            var deserializedFavs = JsonConvert.DeserializeObject<GameState>(favsJsonString);
            return deserializedFavs;
        }

        private static string SerializeToStrng(GameState favs)
        {
            return JsonConvert.SerializeObject(favs);
        }


        public void Save(GameProcess gameProcess)
        {
            var serializable = gameProcess.GameStatusSnapshot;
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(KEY))
            {
                IsolatedStorageSettings.ApplicationSettings.Add(KEY, SerializeToStrng(serializable));
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[KEY] = SerializeToStrng(serializable);
            }
        }

        public GameState LoadGameState()
        {
            return Load();
        }
    }
}