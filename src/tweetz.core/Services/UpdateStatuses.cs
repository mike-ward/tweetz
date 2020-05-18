using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tweetz.core.Models;
using twitter.core.Models;

namespace tweetz.core.Services
{
    public static class UpdateStatuses
    {
        private static readonly IEqualityComparer<TwitterStatus> twitterStatusComparer = new TwitterStatusEqualityComparer();

        public static ValueTask Execute(IEnumerable<TwitterStatus> statuses, TwitterTimeline timeline)
        {
            // Build a hashset for faster lookups.
            var statusesNoNags = timeline.StatusCollection.Where(status => string.CompareOrdinal(status.Id, DonateNagStatus.DonateNagStatusId) != 0);
            var hashSet = new HashSet<TwitterStatus>(statusesNoNags, twitterStatusComparer);

            foreach (var status in statuses.OrderBy(status => status.OriginatingStatus.CreatedDate))
            {
                if (hashSet.TryGetValue(status, out var statusToUpdate))
                {
                    statusToUpdate.OriginatingStatus.UpdateFromStatus(status.OriginatingStatus);
                }
                else if (!timeline.AlreadyAdded.Contains(status.Id))
                {
                    timeline.AlreadyAdded.Add(status.Id);
                    status.UpdateAboutMeProperties(timeline.Settings.ScreenName);
                    timeline.StatusCollection.Insert(0, status);
                }
            }

            return default;
        }
    }
}