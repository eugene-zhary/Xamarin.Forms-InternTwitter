using InterTwitter.Models;
using System.Collections.Generic;

namespace InterTwitter.Services
{
    public interface IMockService
    {
        IList<User> MockedUsers { get; set; }
        IList<Post> MockedPosts { get; set; }
        IList<Models.Notification> MockedNotifications { get; set; }
    }
}
