using InterTwitter.Helpers;
using InterTwitter.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.ViewModels.Posts
{
    public abstract class BasePostViewModel : BindableBase
    {
        public BasePostViewModel(User userModel, Post postModel)
        {
            _userModel = userModel;
            _postModel = postModel;

            _likeIconSource = "ic_like_gray";
            _bookmarksIconSource = "ic_bookmarks_gray";

            _likesCount = _postModel.LikedUserIds.Count();
            _likesCountColor = Color.FromHex("#66696E");
        }


        #region -- Public properties --

        private User _userModel;
        public User UserModel
        {
            get => _userModel;
            set => SetProperty(ref _userModel, value, nameof(UserModel));
        }

        private Post _postModel;
        public Post PostModel
        {
            get => _postModel;
            set => SetProperty(ref _postModel, value, nameof(PostModel));
        }

        private string _likeIconSource;
        public string LikeIconSource
        {
            get => _likeIconSource;
            set => SetProperty(ref _likeIconSource, value, nameof(LikeIconSource));
        }

        private string _bookmarksIconSource;
        public string BookmarksIconSource
        {
            get => _bookmarksIconSource;
            set => SetProperty(ref _bookmarksIconSource, value, nameof(BookmarksIconSource));
        }

        private int _likesCount;
        public int LikesCount
        {
            get => _likesCount;
            set => SetProperty(ref _likesCount, value, nameof(LikesCount));
        }

        private Color _likesCountColor;
        public Color LikesCountColor
        {
            get => _likesCountColor;
            set => SetProperty(ref _likesCountColor, value, nameof(LikesCountColor));
        }


        private ICommand _likesCommand;
        public ICommand LikesCommand => _likesCommand ??= SingleExecutionCommand.FromFunc(OnLikes);

        private ICommand _bookmarksCommand;
        public ICommand BookmarksCommand => _bookmarksCommand ??= SingleExecutionCommand.FromFunc(OnBookmarks);

        #endregion

        #region -- Private helpers --

        private async Task OnLikes()
        {
            ChangeLikeState(LikeIconSource.Equals("ic_like_gray"));
            
            await Task.CompletedTask;
        }

        private async Task OnBookmarks()
        {
            if (BookmarksIconSource.Equals("ic_bookmarks_gray"))
            {
                BookmarksIconSource = "ic_bookmarks_blue";
            }
            else if (BookmarksIconSource.Equals("ic_bookmarks_blue"))
            {
                BookmarksIconSource = "ic_bookmarks_gray";
            }

            await Task.CompletedTask;
        }


        private void ChangeLikeState(bool isLiked)
        {
            // todo: add UserId to UserLikeIds

            if (isLiked)
            {
                LikeIconSource = "ic_like_blue";
                LikesCountColor = Color.FromHex("#2356C5");
                LikesCount++;
            }
            else
            {
                LikeIconSource = "ic_like_gray";
                LikesCountColor = Color.FromHex("#66696E");
                LikesCount--;
            }
        }

        #endregion
    }
}
