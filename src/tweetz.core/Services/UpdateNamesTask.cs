using System.Threading.Tasks;
using tweetz.core.Models;

namespace tweetz.core.Services
{
    internal static class UpdateNamesTask
    {
        public static ValueTask Execute(TwitterTimeline timeline)
        {
            foreach (var status in timeline.StatusCollection)
            {
                TwitterNamesService.Add(status.OriginatingStatus);
            }

            return default;
        }
    }
}