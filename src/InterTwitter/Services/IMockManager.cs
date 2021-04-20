using InterTwitter.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InterTwitter.Services
{
    public interface IMockManager
    {
        IEnumerable<Post> GetMockedPosts(Func<Post, bool> predicate = null);
        IEnumerable<User> GetMockedUsers(Func<User, bool> predicate = null);
    }
}
