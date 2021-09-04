using System.Threading.Tasks;
using System.Windows;
using tweetz.core.Interfaces;
using tweetz.core.Models;
using tweetz.core.Services;

namespace tweetz.core.ViewModels
{
    public class FavoritesTimelineControlViewModel : TwitterTimeline
    {
        private const int             twentyMinutes = 20;
        public        ITwitterService TwitterService { get; }

        public FavoritesTimelineControlViewModel(ITwitterService twitterService, ISettings settings, ISystemState systemState)
            : base(settings, systemState, twentyMinutes)
        {
            timelineName   = App.GetString("favorites-timeline")!;
            TwitterService = twitterService;
            AddUpdateTask(tl => GetAndUpdateFavoritesAsync(tl));
            AddUpdateTask(tl => TruncateStatusCollectionTask.Execute(tl));
            AddUpdateTask(tl => UpdateTimeStampsTask.Execute(tl));
        }

        private async ValueTask GetAndUpdateFavoritesAsync(TwitterTimeline timeline)
        {
            var statuses = await TwitterService.TwitterApi.FavoritesTimeline().ConfigureAwait(true);
            await UpdateStatuses.Execute(statuses, timeline).ConfigureAwait(true);
        }
    }
}