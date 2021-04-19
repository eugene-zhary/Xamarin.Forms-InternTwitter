using InterTwitter.Models;
using System.Collections.Generic;

namespace InterTwitter.Services
{
    public interface IMockManager
    {
        IEnumerable<Post> GetMockedPosts();
    }
}
