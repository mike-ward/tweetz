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
        public ITwitterService TwitterService { get; }

        public FavoritesTimelineControlViewModel(ITwitterService twitterService, ISettings settings, ISystemState systemState)
            : base(settings, systemState, 20)
        {
            TwitterService = twitterService;

            AddUpdateTask(async tl =>
            {
                var statuses = await GetStatuses();
                UpdateStatusesTask.Execute(statuses, tl);
            });
            AddUpdateTask(TruncateStatusCollectionTask.Execute);
            AddUpdateTask(UpdateTimeStampsTask.Execute);
        }

        private async Task<IEnumerable<TwitterStatus>> GetStatuses()
        {
            return await TwitterService.GetFavoritesTimeline().ConfigureAwait(true);
        }
    }
}