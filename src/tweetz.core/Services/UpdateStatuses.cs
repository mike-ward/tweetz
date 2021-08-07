using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using tweetz.core.Extensions;
using tweetz.core.Models;
using twitter.core.Models;

namespace tweetz.core.Services
{
    public static class UpdateStatuses
    {
        public static ValueTask Execute(IEnumerable<TwitterStatus> statuses, TwitterTimeline timeline)
        {
            // Build a hashset for faster lookups.
            var statusesNoNags = timeline.StatusCollection.Where(status => status.Id.IsNotEqualTo(DonateNagStatus.DonateNagStatusId));
            var hashSet        = new HashSet<TwitterStatus>(statusesNoNags);

            foreach (var status in statuses.OrderBy(status => status.OriginatingStatus.CreatedDate))
            {
                if (hashSet.TryGetValue(status, out var statusToUpdate))
                {
                    statusToUpdate.UpdateFromStatus(status);
                }
                else if (timeline.AlreadyAdded.Contains(status.Id) is false)
                {
                    var clonedStatus = Clone(status);
                    timeline.AlreadyAdded.Add(clonedStatus.Id);
                    clonedStatus.UpdateAboutMeProperties(timeline.Settings.ScreenName);

                    if (timeline.IsScrolled)
                    {
                        timeline.PendingStatusCollection.Add(clonedStatus);
                        timeline.PendingStatusesAvailable = true;
                    }
                    else
                    {
                        timeline.StatusCollection.Insert(0, clonedStatus);
                    }
                }
            }

            return default;
        }

        private static TwitterStatus Clone(TwitterStatus twitterStatus)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(twitterStatus);
            var span  = new ReadOnlySpan<byte>(bytes);
            return JsonSerializer.Deserialize<TwitterStatus>(span)!;
        }
    }
}