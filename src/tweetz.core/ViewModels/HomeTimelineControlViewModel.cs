using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using tweetz.core.Infrastructure;
using tweetz.core.Models;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.ViewModels
{
    public class HomeTimelineControlViewModel : TwitterTimeline
    {
        public HomeTimelineControlViewModel(ITwitterService twitterService, ISettings settings, ISystemState systemState)
            : base(settings, systemState, 1.1)
        {
            TwitterService = twitterService;
            AddUpdateTask(GetAndUpdateHomeTweets);
            AddUpdateTask(DonateNagTask.Execute);
            AddUpdateTask(TruncateStatusCollectionTask.Execute);
            AddUpdateTask(UpdateTimeStampsTask.Execute);
        }

        private ITwitterService TwitterService { get; }

        private async Task GetAndUpdateHomeTweets(TwitterTimeline timeline)
        {
            var mentions = await GetMentions();
            var statuses = await TwitterService.GetHomeTimeline();
            UpdateStatuses.Execute(statuses.Concat(mentions), timeline);
        }

        // Twitter limits getting mentions to 100,000 per day per Application.
        // Application in this case means all running Tweetz clients. Once
        // an hour allows everybody get mentions albiet not in a timely manner.
        private const int mentionsInterval = 60;

        private int mentionsCounter = mentionsInterval;

        private async Task<IEnumerable<TwitterStatus>> GetMentions()
        {
            IEnumerable<TwitterStatus> mentions = System.Array.Empty<TwitterStatus>();

            try
            {
                if (mentionsCounter++ >= mentionsInterval)
                {
                    mentionsCounter = 0;
                    mentions = await TwitterService.GetMentionsTimeline();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse response && response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    // Probably hit the daily limit
                    // Alerting the user does no good in this instance (IMO)
                    return mentions;
                }
                throw;
            }
            return mentions;
        }
    }
}