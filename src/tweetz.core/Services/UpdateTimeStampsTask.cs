using System.Threading.Tasks;
using tweetz.core.Models;

namespace tweetz.core.Services
{
    public static class UpdateTimeStampsTask
    {
        public static async Task Execute(TwitterTimeline timeline)
        {
            foreach (var status in timeline.StatusCollection)
            {
                status.InvokeUpdateTimeStamp();
            }

            await Task.CompletedTask;
        }
    }
}