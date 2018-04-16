using System;
using System.Collections.Generic;
using System.Linq;
using MessageSimulator.Core.Domain;

namespace MessageSimulator.Core.Data.Extensions
{
    public static class DataExtensions
    {
        public static string ExtractUsernameFromLine(this string line, string separator)
        {
            return line.Substring(0, line.IndexOf(separator)).Split(null)[0];
        }

        public static IEnumerable<string> ExtractUsersFollowedByUser(this string line, string username, 
            string separator, char stringSplitChar)
        {
            return line.Split(stringSplitChar, ' ')
                .Where(x => x != separator && x != username && !string.IsNullOrWhiteSpace(x));
        }

        public static string ExtractMessageFromLine(this string line, string username)
        {
            return line.Substring(username.Length + 1).Trim();
        }

        public static TUser CreateUser<TUser>(this TUser user, SortedSet<TUser> sortedUserSet, string username)
            where TUser:class, IMessageFeedSubscriber 
        {
            return sortedUserSet.Any(x => x.Name == username) ?
                sortedUserSet.First(x => x.Name == username) : Activator.CreateInstance(typeof(TUser), username) as TUser;
        }
    }
}