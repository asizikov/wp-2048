using System;
using System.Windows;
using Game.Lifecicle;
using Microsoft.Phone.Tasks;

namespace Game.View
{
    internal class AboutPageController
    {
        private readonly About _view;

        public AboutPageController(About view)
        {
            if (view == null) throw new ArgumentNullException("view");
            _view = view;
            Initialize();
        }

        private void Initialize()
        {
            _view.Version.Text = Configuration.Version.ToString();
            _view.OriginalGameButton.Click += OriginalGameButtonOnClick;
            _view.ReviewButton.Click += ReviewButtonOnClick;
        }

        private void ReviewButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var task = new MarketplaceReviewTask();
            task.Show();
        }

        private void OriginalGameButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var wtb = new WebBrowserTask
            {
                Uri = new Uri("http://gabrielecirulli.github.io/2048/", UriKind.Absolute)
            };
            wtb.Show();
        }
    }
}