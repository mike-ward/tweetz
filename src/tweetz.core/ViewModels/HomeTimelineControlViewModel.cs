using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using tweetz.core.Infrastructure;
using tweetz.core.Models;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.ViewModels
{
    public class HomeTimelineControlViewModel : TwitterTimeline
    {
        private const int mentionsInterval = 60;
        private int mentionsCounter = mentionsInterval;

        private const double justOverMinute = 1.1;
        private ITwitterService TwitterService { get; }

        public HomeTimelineControlViewModel(ITwitterService twitterService, ISettings settings, ISystemState systemState)
            : base(settings, systemState, justOverMinute)
        {
            timelineName = (string)Application.Current.FindResource("home-timeline");
            TwitterService = twitterService;
            AddUpdateTask(tl => GetAndUpdateStatuses(tl));
            AddUpdateTask(tl => DonateNagTask.Execute(tl));
            AddUpdateTask(tl => TruncateStatusCollectionTask.Execute(tl));
            AddUpdateTask(tl => UpdateTimeStampsTask.Execute(tl));
        }

        private async ValueTask GetAndUpdateStatuses(TwitterTimeline timeline)
        {
            var mentions = await GetMentions().ConfigureAwait(true);
            var statuses = await TwitterService.GetHomeTimeline().ConfigureAwait(true);
            await UpdateStatuses.Execute(statuses.Concat(mentions), timeline).ConfigureAwait(true);
        }

        private async ValueTask<IEnumerable<TwitterStatus>> GetMentions()
        {
            try
            {
                // Twitter limits getting mentions to 100,000 per day per Application.
                // Application in this case means all running Tweetz clients. Once
                // an hour allows everybody get mentions albiet not in a timely manner.
                if (mentionsCounter++ >= mentionsInterval)
                {
                    mentionsCounter = 0;
                    return await TwitterService.GetMentionsTimeline().ConfigureAwait(true);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse response && response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    // Probably hit the daily limit
                    // Alerting the user does no good in this instance (IMO)
                    return Enumerable.Empty<TwitterStatus>();
                }
                throw;
            }
            return Enumerable.Empty<TwitterStatus>();
        }
    }
}