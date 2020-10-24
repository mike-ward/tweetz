using System;
using System.Collections.Generic;
using twitter.core.Models;

namespace tweetz.core.Services
{
    public static class TwitterNamesService
    {
        private static readonly ISet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);

        public static IEnumerable<string> Names => hashSet;

        public static void Add(TwitterStatus status)
        {
            try
            {
                hashSet.Add(status.OriginatingStatus.User.ScreenName!);

                if (status.IsRetweet)
                {
                    hashSet.Add(status.RetweetedStatus!.User.ScreenName!);
                }

                if (status.Entities is not null && status.Entities.Mentions is not null)
                {
                    foreach (var mention in status.Entities.Mentions)
                    {
                        hashSet.Add(mention.ScreenName);
                    }
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }
    }
}