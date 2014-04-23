using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Game.Lifecicle;
using Game.Resources;
using Microsoft.Phone.Tasks;

namespace Game.View
{
    public class AppItem
    {
        public string Name { get; set; }
        public string ImageUri { get; set; }
        public string Description { get; set; }
        public string AppUri { get; set; }
    }

    internal class AboutPageController
    {
        private readonly About _view;
        private ObservableCollection<AppItem> _otherAppsItems = new ObservableCollection<AppItem>();

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
            _otherAppsItems.Add(new AppItem
            {
                Name = "PinHolder",
                Description = AppResources.AppDescPinholder,
                ImageUri = "/Assets/OtherApps/Pinholder.png",
                AppUri = "9c716f8e-bed1-4a22-9000-49f67189f56c"
            });
            _view.OtherAppsList.ItemsSource = _otherAppsItems;
            _view.OtherAppsList.SelectionChanged += OtherAppsListOnSelectionChanged;
        }

        private void OtherAppsListOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            if (e == null || e.AddedItems == null || e.AddedItems.Count == 0) return;
            var selectedItem = e.AddedItems[0] as AppItem;
            if (selectedItem != null)
            {
                StatisticsService.PublishTryAppClicked(selectedItem.Name);
                
                var task = new MarketplaceDetailTask
                {
                    ContentIdentifier = selectedItem.AppUri
                };
                task.Show();
            }
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