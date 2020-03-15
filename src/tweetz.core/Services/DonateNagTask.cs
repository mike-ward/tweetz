using System.Threading.Tasks;
using tweetz.core.Models;

namespace tweetz.core.Services
{
    public static class DonateNagTask
    {
        private const int donateNagCounterInterval = 120;
        private static int donateNagCounter = donateNagCounterInterval - 10;

        public static Task Execute(TwitterTimeline timeline)
        {
            if (timeline is null) throw new System.ArgumentNullException(nameof(timeline));

            if (timeline.Settings.Donated)
            {
                return Task.CompletedTask;
            }

            if (donateNagCounter >= donateNagCounterInterval)
            {
                donateNagCounter = 0;
                timeline.StatusCollection.Insert(0, new DonateNagStatus());
            }
            else
            {
                donateNagCounter += 1;
            }

            return Task.CompletedTask;
        }
    }
}