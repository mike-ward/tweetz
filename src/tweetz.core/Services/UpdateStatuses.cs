using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using tweetz.core.Models;
using twitter.core.Models;

namespace tweetz.core.Services
{
    public static class UpdateStatuses
    {
        private static readonly ObservableCollection<TwitterStatus> EmptyStatusCollection = new ObservableCollection<TwitterStatus>();

        public static void Execute(IEnumerable<TwitterStatus> statuses, TwitterTimeline timeline)
        {
            // ObservableCollection only supports linear searching.
            // Build a dictionary for faster lookups.
            var statusDictionary = timeline.StatusCollection
                .Where(status => status.Id != DonateNagStatus.DonateNagStatusId)
                .ToDictionary(status => status.Id, status => status);

            // disconnet the binding from the status collection while updating.
            // This reduces jank in the timeline
            var current = timeline.StatusCollection;
            timeline.StatusCollection = EmptyStatusCollection;

            try
            {
                foreach (var status in statuses.Reverse())
                {
                    if (statusDictionary.TryGetValue(status.Id, out var statusToUpdate))
                    {
                        statusToUpdate.OriginatingStatus.UpdateFromStatus(status.OriginatingStatus);
                    }
                    else if (!timeline.AlreadyAdded.Contains(status.Id))
                    {
                        timeline.AlreadyAdded.Add(status.Id);
                        status.AboutMe(timeline.Settings.ScreenName);
                        current.Insert(0, status);
                    }
                }
            }
            finally
            {
                timeline.StatusCollection = current;
            }
        }
    }
}