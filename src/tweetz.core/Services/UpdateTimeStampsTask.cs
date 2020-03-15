using System;
using System.Threading.Tasks;
using tweetz.core.Models;

namespace tweetz.core.Services
{
    public static class UpdateTimeStampsTask
    {
        public static Task Execute(TwitterTimeline timeline)
        {
            if (timeline is null) throw new ArgumentNullException(nameof(timeline));

            foreach (var status in timeline.StatusCollection)
            {
                status.InvokeUpdateTimeStamp();
            }

            return Task.CompletedTask;
        }
    }
}