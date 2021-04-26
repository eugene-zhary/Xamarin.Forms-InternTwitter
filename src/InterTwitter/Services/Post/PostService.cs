using InterTwitter.ViewModels.Posts;
using System.Collections.Generic;
using InterTwitter.Extensions;
using System;
using InterTwitter.Models;
using System.Linq;
using InterTwitter.Services.Settings;
using System.Threading.Tasks;
using InterTwitter.Helpers;
using Prism.Events;

namespace InterTwitter.Services
{
    public class PostService : IPostService
    {
        private readonly IMockService _mock;
        private readonly ISettingsManager _settings;
        private readonly IEventAggregator _eventAggregator;

        public PostService(IMockService mock, ISettingsManager settings, IEventAggregator aggregator)
        {
            _eventAggregator = aggregator;
            _mock = mock;
            _settings = settings;
        }

        #region -- IPostManager implementation --

        public async Task<AOResult<IEnumerable<BasePostViewModel>>> GetPostsAsync(Func<Post, bool> predecate)
        {
            var result = new AOResult<IEnumerable<BasePostViewModel>>();
            await Task.Delay(100);

            try
            {
                var posts = (predecate == null) ?
                    _mock.MockedPosts.ToViewModelCollection(this, _settings.RememberedUserId) :
                    _mock.MockedPosts.Where(predecate)?.ToViewModelCollection(this, _settings.RememberedUserId);

                if(posts.Any())
                {
                    result.SetSuccess(posts);
                }
                else
                {
                    result.SetFailure("Post collection is empty");
                }
            }
            catch(Exception ex)
            {
                result.SetError($"{nameof(GetPostsAsync)}: exception", "Something went wrong", ex);
            }

            return result;
        }
        public AOResult<User> GetPostAuthorAsync(int userId)
        {
            var result = new AOResult<User>();

            try
            {
                var author = _mock.MockedUsers.Where(user => user.Id == userId).FirstOrDefault();

                if(author != null)
                {
                    result.SetSuccess(author);
                }
                else
                {
                    result.SetFailure("Non-existent author of post");
                }
            }
            catch(Exception ex)
            {
                result.SetError($"{nameof(GetPostAuthorAsync)}: exception", "Something went wrong", ex);
            }

            return result;
        }
        public async Task<AOResult> LikePostAsync(int postId)
        {
            var result = new AOResult();
            await Task.Delay(100);

            try
            {
                var post = _mock.MockedPosts.Where(p => p.Id == postId).FirstOrDefault();
                post.LikedUserIds.Add(_settings.RememberedUserId);

                result.SetSuccess();
            }
            catch(Exception ex)
            {
                result.SetError($"{nameof(LikePostAsync)}: exception", "Something went wrong", ex);
            }

            return result;
        }
        public async Task<AOResult> UnlikePostAsync(int postId)
        {
            var result = new AOResult();
            await Task.Delay(100);

            try
            {
                var post = _mock.MockedPosts.Where(p => p.Id == postId).FirstOrDefault();
                post.LikedUserIds.Remove(_settings.RememberedUserId);

                result.SetSuccess();
            }
            catch(Exception ex)
            {
                result.SetError($"{nameof(UnlikePostAsync)}: exception", "Something went wrong", ex);
            }

            return result;
        }
        public async Task<AOResult> BookmarkPostAsync(int postId)
        {
            var result = new AOResult();
            await Task.Delay(100);

            try
            {
                var post = _mock.MockedPosts.Where(p => p.Id == postId).FirstOrDefault();
                post.BookmarkedUserIds.Add(_settings.RememberedUserId);

                result.SetSuccess();
            }
            catch(Exception ex)
            {
                result.SetError($"{nameof(BookmarkPostAsync)}: exception", "Something went wrong", ex);
            }

            return result;
        }
        public async Task<AOResult> UnbookmarkPostAsync(int postId)
        {
            var result = new AOResult();
            await Task.Delay(100);

            try
            {
                var post = _mock.MockedPosts.Where(p => p.Id == postId).FirstOrDefault();
                post.BookmarkedUserIds.Remove(_settings.RememberedUserId);

                result.SetSuccess();
            }
            catch(Exception ex)
            {
                result.SetError($"{nameof(UnbookmarkPostAsync)}: exception", "Something went wrong", ex);
            }

            return result;
        }

        #endregion
    }
}
