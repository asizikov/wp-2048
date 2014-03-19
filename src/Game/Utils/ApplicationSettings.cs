using System.Collections.Generic;
using System.IO.IsolatedStorage;
using GameEngine;
using Newtonsoft.Json;

namespace Game.Utils
{
    public class GameSettings
    {
        public bool UseSwipe { get; set; }
        public List<int> BestScores { get; set; }
        public bool UseVibro { get; set; }
    }

    public class ApplicationSettings
    {
        private const string KEY = "GameState";
        private const string SettingsKey = "settings";

        public ApplicationSettings()
        {
            Settings = LoadSettings();
        }

        private GameSettings LoadSettings()
        {
            GameSettings settings = null;
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(SettingsKey))
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingsKey, SerializeToStrng(new GameSettings
                {
                    UseSwipe = false,
                    BestScores = new List<int>(),
                    UseVibro = false
                }));
            }
            var favsJsonString = (string) IsolatedStorageSettings.ApplicationSettings[SettingsKey];
            settings = DeserializeFromString<GameSettings>(favsJsonString);
            if (settings.BestScores == null)
            {
                settings.BestScores = new List<int>();
            }
            return settings;
        }

        public bool HasStoredGame
        {
            get { return IsolatedStorageSettings.ApplicationSettings.Contains(KEY); }
        }

        public GameSettings Settings { get; set; }

        private static GameState Load()
        {
            GameState state = null;
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(KEY))
            {
                IsolatedStorageSettings.ApplicationSettings.Add(KEY, SerializeToStrng(state));
            }
            else
            {
                var favsJsonString = (string) IsolatedStorageSettings.ApplicationSettings[KEY];
                state = DeserializeFromString<GameState>(favsJsonString);
            }
            return state;
        }

        private static T DeserializeFromString<T>(string favsJsonString)
        {
            var deserializedFavs = JsonConvert.DeserializeObject<T>(favsJsonString);
            return deserializedFavs;
        }

        private static string SerializeToStrng<T>(T favs)
        {
            return JsonConvert.SerializeObject(favs);
        }

        public void SaveSettings()
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains(SettingsKey))
            {
                IsolatedStorageSettings.ApplicationSettings.Add(SettingsKey, SerializeToStrng(Settings));
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings[SettingsKey] = SerializeToStrng(Settings);
            }
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