using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using tweetz.core.Services;
using tweetz.core.ViewModels;
using twitter.core.Models;

namespace tweetz.core.Views.UserProfileControls
{
    public partial class UserProfileTimeline : UserControl
    {
        public UserProfileTimeline()
        {
            InitializeComponent();
            TimelineView.DataContext = App.ServiceProvider.GetService<UserProfileTimelineViewModel>();
        }

        [SuppressMessage("Usage", "VSTHRD100", MessageId = "Avoid async void methods")]
        private async void UserProfileTimeline_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (TimelineView.DataContext is UserProfileTimelineViewModel vm)
                {
                    vm.StatusCollection.Clear();

                    if (DataContext is User user)
                    {
                        await Task.Delay(100).ConfigureAwait(true);

                        try
                        {
                            var statuses = await vm.GetUserTimeline(user.ScreenName!).ConfigureAwait(true);

                            foreach (var status in statuses.OrderByDescending(status => status.OriginatingStatus.CreatedDate))
                            {
                                vm.StatusCollection.Add(status);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}