using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterTwitter.Services
{
    public interface IPostService
    {
        Task<AOResult> AddPostAsync(Post post);
        Task<AOResult<IEnumerable<BasePostViewModel>>> GetPostsAsync(Func<Post, bool> predecate = null);
        AOResult<User> GetPostAuthorAsync(int userId);
        Task<AOResult> LikePostAsync(int postId);
        Task<AOResult> UnlikePostAsync(int postId);
        Task<AOResult> BookmarkPostAsync(int postId);
        Task<AOResult> UnbookmarkPostAsync(int postId);
    }
}
