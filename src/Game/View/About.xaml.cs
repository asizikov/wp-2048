using System.Windows.Navigation;
using Game.Lifecicle;

namespace Game.View
{
    public partial class About
    {
        private AboutPageController _aboutPageController;

        public About()
        {
            InitializeComponent();
            _aboutPageController = new AboutPageController(this);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            StatisticsService.PublishAboutPageLoaded();
        }
    }
}