using System;
using Game.Utils;

namespace Game.View
{
    internal class SettingsController
    {
        private readonly Settings _view;
        private ApplicationSettings _applicationSettings;

        public SettingsController(Settings view)
        {
            if (view == null) throw new ArgumentNullException("view");
            _view = view;
            _applicationSettings = new ApplicationSettings();
            _view.GestureSwitch.IsChecked = _applicationSettings.Settings.UseSwipe;
        }

        public void OnLeave()
        {
            _applicationSettings.Settings.UseSwipe = _view.GestureSwitch.IsChecked ?? false;
            _applicationSettings.SaveSettings();
        }
    }
}