using InterTwitter.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InterTwitter.Services
{
    public interface IMockService
    {
        IList<User> MockedUsers { get; set; }
        IList<Post> MockedPosts { get; set; }
    }
}
