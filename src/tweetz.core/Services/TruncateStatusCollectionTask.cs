using System.Threading.Tasks;
using tweetz.core.Models;

namespace tweetz.core.Services
{
    public static class TruncateStatusCollectionTask
    {
        public static ValueTask Execute(TwitterTimeline timeline)
        {
            const int maxNumberOfStatuses = 500;
            var truncated = timeline.StatusCollection.Count > maxNumberOfStatuses;

            while (timeline.StatusCollection.Count > maxNumberOfStatuses)
            {
                timeline.StatusCollection.RemoveAtNoNotify(timeline.StatusCollection.Count - 1);
            }

            if (truncated)
            {
                timeline.StatusCollection.NotifyCollectionChanged();
            }

            return default;
        }
    }
}