using System;
using System.Windows;
using System.Windows.Navigation;
using Game.Utils;
using Microsoft.Phone.Controls;

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

    public partial class Settings : PhoneApplicationPage
    {
        private SettingsController _settingsController;

        public Settings()
        {
            InitializeComponent();
            _settingsController = new SettingsController(this);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            _settingsController.OnLeave();

        }
    }
}