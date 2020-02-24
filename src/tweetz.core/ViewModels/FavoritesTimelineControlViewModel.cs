using System.Collections.Generic;
using System.Threading.Tasks;
using tweetz.core.Infrastructure;
using tweetz.core.Models;
using tweetz.core.Services;
using twitter.core.Models;

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

            AddUpdateTask(async timeline =>
            {
                var statuses = await GetStatuses();
                UpdateStatuses.Execute(statuses, timeline);
            });
            AddUpdateTask(TruncateStatusCollectionTask.Execute);
            AddUpdateTask(UpdateTimeStampsTask.Execute);
        }

        private async Task<IEnumerable<TwitterStatus>> GetStatuses()
        {
            return await TwitterService.GetFavoritesTimeline();
        }
    }
}