using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using tweetz.core.Infrastructure;
using twitter.core.Models;

namespace tweetz.core.ViewModels
{
    public class HomeTimelineControlViewModel : BaseTimelineControlViewModel
    {
        public ITwitterService TwitterService { get; }

        public HomeTimelineControlViewModel(ITwitterService twitterService, ISettings settings, ISystemState systemState)
            : base(settings, systemState)
        {
            TwitterService = twitterService;
        }

        protected override double IntervalInMinutes => 1.1;

        protected override async Task<IEnumerable<TwitterStatus>> GetTimeline()
        {
            var mentions = await GetMentions();
            var statuses = await TwitterService.GetHomeTimeline();
            return statuses.Concat(mentions);
        }

        // Twitter limits getting mentions to 100,000 per day per Application.
        // Application in this case means all running Tweetz clients. Once
        // an hour allows everybody get mentions albiet not in a timely manner.
        private const int mentionsInterval = 60;

        private int mentionsCounter = mentionsInterval;

        private async Task<IEnumerable<TwitterStatus>> GetMentions()
        {
            IEnumerable<TwitterStatus> mentions = new TwitterStatus[0];

            try
            {
                if (mentionsCounter >= mentionsInterval)
                {
                    mentionsCounter = 0;
                    mentions = await TwitterService.GetMentionsTimeline();
                }
                else
                {
                    mentionsCounter += 1;
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