using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using tweetz.core.Interfaces;
using tweetz.core.Models;
using tweetz.core.Services;
using twitter.core.Models;

namespace tweetz.core.ViewModels
{
    public class HomeTimelineControlViewModel : TwitterTimeline
    {
        private const int mentionsInterval = 60;
        private       int mentionsCounter  = mentionsInterval;

        private const double          justOverMinute = 1.1;
        private       ITwitterService TwitterService { get; }

        public HomeTimelineControlViewModel(ITwitterService twitterService, ISettings settings, ISystemState systemState)
            : base(settings, systemState, justOverMinute)
        {
            timelineName   = App.GetString("home-timeline");
            TwitterService = twitterService;
            AddUpdateTask(tl => GetAndUpdateStatusesAsync(tl));
            AddUpdateTask(tl => DonateNagTask.Execute(tl));
            AddUpdateTask(tl => TruncateStatusCollectionTask.Execute(tl));
            AddUpdateTask(tl => UpdateTimeStampsTask.Execute(tl));
            AddUpdateTask(t1 => UpdateNamesTask.Execute(t1));
            AddUpdateTask(_ => CollectAllTask.Execute());
        }

        private async ValueTask GetAndUpdateStatusesAsync(TwitterTimeline timeline)
        {
            var mentions = await GetMentionsAsync().ConfigureAwait(true);
            var statuses = await TwitterService.TwitterApi.HomeTimeline().ConfigureAwait(true);
            await UpdateStatuses.Execute(statuses.Concat(mentions), timeline).ConfigureAwait(true);
        }

        private async ValueTask<IEnumerable<TwitterStatus>> GetMentionsAsync()
        {
            try
            {
                // Twitter limits getting mentions to 100,000 per day per Application.
                // Application in this case means all running Tweetz clients. Once
                // an hour allows everybody get mentions albeit not in a timely manner.
                if (mentionsCounter++ >= mentionsInterval)
                {
                    mentionsCounter = 0;
                    return await TwitterService.TwitterApi.MentionsTimeline(20).ConfigureAwait(true);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse { StatusCode: HttpStatusCode.TooManyRequests })
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