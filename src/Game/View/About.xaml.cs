using System.Windows.Navigation;
using Game.Lifecicle;
using Microsoft.Phone.Controls;

namespace Game.View
{
    public partial class About
    {
        public About()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            StatisticsService.PublishAboutPageLoaded();
        }
    }
}