using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Game.View
{
    public partial class Leaderboard : PhoneApplicationPage
    {
        private LeaderboardPageController _leaderboardPageController;

        public Leaderboard()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _leaderboardPageController = new LeaderboardPageController(this);
        }
    }
}