using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using tweetz.core.ViewModels;
using twitter.core.Models;

namespace tweetz.core.Views.UserProfileControls
{
    public partial class UserProfileTimeline : UserControl
    {
        public UserProfileTimeline()
        {
            InitializeComponent();
        }

        private async void UserProfileTimeline_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (TimelineView.DataContext is UserProfileTimelineViewModel vm)
            {
                vm.StatusCollection.Clear();

                if (DataContext is User user)
                {
                    await Task.Delay(100).ConfigureAwait(true);
                    var statuses = await vm.GetUserTimeline(user.ScreenName!).ConfigureAwait(true);

                    foreach (var status in statuses.OrderByDescending(status => status.OriginatingStatus.CreatedDate))
                    {
                        vm.StatusCollection.Add(status);
                    }
                }
            }
        }
    }
}