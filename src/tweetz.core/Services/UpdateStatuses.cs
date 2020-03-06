using System.Collections.Generic;
using System.Linq;
using tweetz.core.Models;
using twitter.core.Models;

namespace tweetz.core.Services
{
    public static class UpdateStatuses
    {
        private static readonly IEqualityComparer<TwitterStatus> twitterStatusComparer = new TwitterStatusEqualityComparer();

        public static void Execute(IEnumerable<TwitterStatus> statuses, TwitterTimeline timeline)
        {
            // Build a hashset for faster lookups.
            var statusesWithoutNags = timeline.StatusCollection.Where(status => status.Id != DonateNagStatus.DonateNagStatusId);
            var hashSet = new HashSet<TwitterStatus>(statusesWithoutNags, twitterStatusComparer);

            foreach (var status in statuses.Reverse())
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
        }
    }
}