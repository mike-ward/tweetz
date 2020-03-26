using System.Threading.Tasks;
using tweetz.core.Infrastructure;
using tweetz.core.Models;
using tweetz.core.Services;

namespace tweetz.core.ViewModels
{
    public class FavoritesTimelineControlViewModel : TwitterTimeline
    {
        private const int twentyMinutes = 20;

        public ITwitterService TwitterService { get; }

        public FavoritesTimelineControlViewModel(ITwitterService twitterService, ISettings settings, ISystemState systemState)
            : base(settings, systemState, twentyMinutes)
        {
            TwitterService = twitterService;
            AddUpdateTask(GetAndUpdateFavorites);
            AddUpdateTask(TruncateStatusCollectionTask.Execute);
            AddUpdateTask(UpdateTimeStampsTask.Execute);
        }

        private async ValueTask GetAndUpdateFavorites(TwitterTimeline timeline)
        {
            var statuses = await TwitterService.GetFavoritesTimeline().ConfigureAwait(true);
            await UpdateStatuses.Execute(statuses, timeline);
        }
    }
}