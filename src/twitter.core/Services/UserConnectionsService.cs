using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using twitter.core.Models;

namespace twitter.core.Services
{
    // Twitter has deprecated information about following and followed by
    // statuses in the Tweet object. It can still be had by looking up the user
    // info using their API but of course its rate limited. This class creates a
    // dictionary and keeps it updated as much as it can given the rate limits.

    public static class UserConnectionsService
    {
        private const int maxIds = 100; // Twitter cap
        private static int lastIndex;
        private static readonly ConcurrentDictionary<string, UserConnection> UserConnectionsDirectory = new ConcurrentDictionary<string, UserConnection>(StringComparer.Ordinal);

        public static async Task UpdateUserConnectionsAsync(TwitterApi twitterApi)
        {
            try
            {
                var ids = UserConnectionsDirectory.Keys.Skip(lastIndex).Take(maxIds);
                var connections = await twitterApi.GetFriendships(ids).ConfigureAwait(false);
                lastIndex = (lastIndex + maxIds) < UserConnectionsDirectory.Count ? lastIndex + maxIds : 0;

                foreach (var connection in connections)
                {
                    UserConnectionsDirectory.AddOrUpdate(connection.Id, connection, (_, __) => connection);
                }
            }
            catch (Exception ex)
            {
                TraceService.Message(ex.Message);
            }
        }

        public static Task AddUserIdsAsync(IEnumerable<string> userIds, TwitterApi twitterApi)
        {
            foreach (var userId in userIds)
            {
                TryAddUserId(userId);
            }

            return UpdateUserConnectionsAsync(twitterApi);
        }

        private static void TryAddUserId(string userId)
        {
            UserConnectionsDirectory.TryAdd(userId, new UserConnection { Id = userId });
        }

        public static UserConnection? LookupUserConnections(string id)
        {
            if (!UserConnectionsDirectory.TryGetValue(id, out var user))
            {
                TryAddUserId(id);
            }

            return user;
        }
    }
}