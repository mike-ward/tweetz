using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using tweetz.core.Interfaces;
using twitter.core.Models;

namespace tweetz.core.ViewModels
{
    public class UserProfileTimelineViewModel
    {
        private readonly ITwitterService twitterService;

        public ISettings                           Settings         { get; }
        public ObservableCollection<TwitterStatus> StatusCollection { get; } = new();

        public UserProfileTimelineViewModel(ITwitterService twitterService, ISettings settings)
        {
            Settings            = settings;
            this.twitterService = twitterService;
        }

        public ValueTask<IEnumerable<TwitterStatus>> GetUserTimeline(string screenName)
        {
            return twitterService.TwitterApi.GetUserTimeline(screenName);
        }
    }
}