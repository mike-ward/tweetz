using System.Collections.Generic;
using System.Threading.Tasks;
using tweetz.core.Infrastructure;
using twitter.core.Models;

namespace tweetz.core.ViewModels
{
    public class FavoritesTimelineControlViewModel : BaseTimelineControlViewModel
    {
        public ITwitterService TwitterService { get; }

        public FavoritesTimelineControlViewModel(ITwitterService twitterService, ISettings settings, ISystemState systemState)
            : base(settings, systemState)
        {
            TwitterService = twitterService;
        }

        protected override double IntervalInMinutes => 20;

        protected override async Task<IEnumerable<TwitterStatus>> GetTimeline()
        {
            return await TwitterService.GetFavoritesTimeline().ConfigureAwait(true);
        }
    }
}