using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.ViewModels.Posts;
using System;
using System.Collections.Generic;

namespace InterTwitter.Services
{
    public interface IPostManager
    {
        IEnumerable<BasePostViewModel> GetPosts(Func<Post, bool> predecate = null);
        User GetPostAuthor(int userId);
        void LikePost(int postId,int userId);
        void UnlikePost(int postId, int userId);
        void BookmarkPost(int postId, int userId);
        void UnbookmarkPost(int postId, int userId);
    }
}
