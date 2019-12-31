using twitter.core.Models;

namespace tweetz.core.Models
{
    public class DonateNagStatus : TwitterStatus
    {
        public const string DonateNagStatusId = "1";
        private const string donateUrl = "https://mike-ward.net/donate";

        public DonateNagStatus()
        {
            Id = DonateNagStatusId;
            FullText = "Please consider donating to Tweetz.\nhttps://mike-ward.net/donate";
            CreatedAt = "Fri Nov 22 15:12:15 +0000 2019";
            OverrideLink = donateUrl;
            Entities = new Entities
            {
                Urls = new[] {
                   new UrlEntity
                    {
                       DisplayUrl = donateUrl,
                       ExpandedUrl = donateUrl,
                       Indices = new[] {36, 64}
                    }
                },
            };
            ExtendedEntities = new Entities
            {
                Media = new[]
                {
                    new Media
                    {
                        MediaUrl = "https://mike-ward.net/cdn/images/donate.png",
                    }
                }
            };
            User = new User
            {
                ScreenName = "mikeward_aa",
                Name = "Mike Ward",
                Id = "14410002",
                Location = "Dubuque, IA",
                ProfileImageUrl = "https://pbs.twimg.com/profile_images/495209879749935104/AV0xDcXP_bigger.jpeg",
                Description = ".NET, Technology, Life, Whatever..."
            };
        }
    }
}