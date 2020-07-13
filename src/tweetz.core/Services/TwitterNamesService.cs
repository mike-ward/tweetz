using System;
using System.Collections.Generic;
using System.Diagnostics;
using twitter.core.Models;

namespace tweetz.core.Services
{
    public static class TwitterNamesService
    {
        private static readonly HashSet<string> hashSet = new HashSet<string>();

        public static IEnumerable<string> Names => hashSet;

        public static void Add(TwitterStatus status)
        {
            try
            {
                hashSet.Add(status.OriginatingStatus.User.ScreenName!);
                if (status.IsRetweet) { hashSet.Add(status.RetweetedStatus!.User.ScreenName!); }

                if (!(status.Entities is null) && !(status.Entities.Mentions is null))
                {
                    foreach (var mention in status.Entities.Mentions)
                    {
                        hashSet.Add(mention.ScreenName);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
        }
    }
}