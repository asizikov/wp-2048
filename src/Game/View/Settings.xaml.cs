using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace Game.View
{
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