using InterTwitter.ViewModels.Posts;
using System.Collections.Generic;
using InterTwitter.Extensions;
using System;
using InterTwitter.Models;
using System.Linq;

namespace InterTwitter.Services
{
    public class PostManager : IPostManager
    {
        private readonly IMockManager _mock;
        private readonly ISettingsManager _settings;

        public PostManager(IMockManager mock, ISettingsManager settings)
        {
            _mock = mock;
            _settings = settings;
        }


        #region -- IPostManager implementation --

        public IEnumerable<BasePostViewModel> GetPosts(Func<Post, bool> predecate = null)
        {
            IEnumerable<BasePostViewModel> output = null;

            output = (predecate == null) ?
                _mock.MockedPosts.ToViewModelCollection(this) :
                _mock.MockedPosts.Where(predecate)?.ToViewModelCollection(this);

            return output;
        }

        public User GetPostAuthor(int userId)
        {
            return _mock.MockedUsers.Where(user => user.Id == userId).FirstOrDefault();
        }

        public void LikePost(int postId, int userId)
        {
            _mock.MockedPosts.Where(p => p.Id == postId).FirstOrDefault()?.LikedUserIds.Add(userId);
        }

        public void UnlikePost(int postId, int userId)
        {
            _mock.MockedPosts.Where(p => p.Id == postId).FirstOrDefault()?.LikedUserIds.Remove(userId);
        }

        public void BookmarkPost(int postId, int userId)
        {
            _mock.MockedPosts.Where(p => p.Id == postId).FirstOrDefault()?.BookmarkedUserIds.Add(userId);
        }

        public void UnbookmarkPost(int postId, int userId)
        {
            _mock.MockedPosts.Where(p => p.Id == postId).FirstOrDefault()?.BookmarkedUserIds.Remove(userId);
        }

        #endregion
    }
}
