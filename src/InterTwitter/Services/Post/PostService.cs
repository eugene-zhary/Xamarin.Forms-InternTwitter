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
                var posts = (predecate == null)
                    ? _mock.MockedPosts
                    : _mock.MockedPosts.Where(predecate);

                var postsResult = new List<BasePostViewModel>();

                foreach(var post in posts)
                {
                    var postAuthor = await GetPostAuthorAsync(post.UserId);
                    if(postAuthor.IsSuccess)
                    {
                        postsResult.Add(post.ToViewModel(postAuthor.Result, _settings.RememberedUserId));
                    }
                }

                if(postsResult!= null && postsResult.Any())
                {
                    result.SetSuccess(postsResult.OrderByDescending(p => p.PostModel.CreationDateTime));
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

        public Task<AOResult<User>> GetPostAuthorAsync(int userId)
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

            return Task.FromResult(result);
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

        public async Task<AOResult> AddPostAsync(Post post)
        {
            var result = new AOResult();
            await Task.Delay(100);

            try
            {
                var lastPostId = _mock.MockedPosts.Last().Id;
                post.Id = ++lastPostId;

                _mock.MockedPosts.Add(post);
                result.SetSuccess();
            }
            catch(Exception ex)
            {
                result.SetError($"{nameof(AddPostAsync)}: exception", "Something went wrong", ex);
            }

            return result;
        }

        #endregion
    }
}
