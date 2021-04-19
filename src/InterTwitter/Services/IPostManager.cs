using InterTwitter.Helpers;
using InterTwitter.ViewModels.Posts;
using System.Collections.Generic;

namespace InterTwitter.Services
{
    public interface IPostManager
    {
        IEnumerable<BasePostViewModel> GetPosts();
    }
}
