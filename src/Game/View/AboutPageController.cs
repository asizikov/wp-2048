using System;
using System.Windows;
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
            _view.OriginalGameButton.Click += OriginalGameButtonOnClick;
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