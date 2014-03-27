using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using Game.Process;
using Game.Resources;
using Game.Utils;
using Newtonsoft.Json;

namespace Game.View
{
    public class BackgroundItem
    {
        public string Name { get; set; }
        public string Value { get; set; }

        [JsonIgnore]
        public SolidColorBrush Color
        {
            get { return new SolidColorBrush(CellFactory.ConvertStringToColor(Value)); }
        }

        public static BackgroundItem Default()
        {
            return new BackgroundItem
            {
                Name = AppResources.Default,
                Value = "#328FDB"
            };
        }
    }

    internal class SettingsController
    {
        private readonly Settings _view;
        private ApplicationSettings _applicationSettings;

        private readonly ObservableCollection<BackgroundItem> _backgroundItems;

        public SettingsController(Settings view)
        {
            _backgroundItems = new ObservableCollection<BackgroundItem>(new List<BackgroundItem>
            {
                BackgroundItem.Default(),
                new BackgroundItem
                {
                    Name = AppResources.Dark,
                    Value = "#47403A"
                }
            });
            if (view == null) throw new ArgumentNullException("view");
            _view = view;
            _applicationSettings = new ApplicationSettings();
            _view.GestureSwitch.IsChecked = _applicationSettings.Settings.UseSwipe;
            _view.Backgrounds.ItemsSource = _backgroundItems;
            _view.Backgrounds.SelectedItem = _backgroundItems.FirstOrDefault(item => item.Value ==
                                                                                     _applicationSettings.Settings
                                                                                         .BgColor.Value);
        }

        public void OnLeave()
        {
            _applicationSettings.Settings.UseSwipe = _view.GestureSwitch.IsChecked ?? false;
            _applicationSettings.Settings.BgColor = (BackgroundItem) _view.Backgrounds.SelectedItem;
            _applicationSettings.SaveSettings();
        }
    }
}