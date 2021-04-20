using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services;
using Prism.Mvvm;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterTwitter.ViewModels.Posts
{
    public abstract class BasePostViewModel : BindableBase
    {
        private readonly IPostManager _postManager;

        public BasePostViewModel(User userModel, Post postModel, IPostManager postManager)
        {
            _userModel = userModel;
            _postModel = postModel;
            _postManager = postManager;

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

        private bool _isLiked;
        public bool IsLiked
        {
            get => _isLiked;
            set => SetProperty(ref _isLiked, value, nameof(IsLiked));
        }

        private bool _isBookmarked;
        public bool IsBookmarked
        {
            get => _isBookmarked;
            set => SetProperty(ref _isBookmarked, value, nameof(IsBookmarked));
        }
        
        public int LikesCount
        {
            get => PostModel.LikedUserIds.Count();
        }

        private ICommand _likesCommand;
        public ICommand LikesCommand => _likesCommand ??= SingleExecutionCommand.FromFunc(OnLikes);

        private ICommand _bookmarksCommand;
        public ICommand BookmarksCommand => _bookmarksCommand ??= SingleExecutionCommand.FromFunc(OnBookmarks);

        #endregion

        #region -- Private helpers --
        
        private async Task OnLikes()
        {
            IsLiked = !IsLiked;

            int mockCurrentUserId = 0;

            if(IsLiked)
            {
                _postManager.LikePost(PostModel.Id, mockCurrentUserId);
            }
            else
            {
                _postManager.UnlikePost(PostModel.Id, mockCurrentUserId);
            }

            RaisePropertyChanged(nameof(LikesCount));

            await Task.CompletedTask;
        }

        private async Task OnBookmarks()
        {
            IsBookmarked = !IsBookmarked;

            int mockCurrentUserId = 0;

            if(IsBookmarked)
            {
                _postManager.BookmarkPost(PostModel.Id, mockCurrentUserId);
            }
            else
            {
                _postManager.UnbookmarkPost(PostModel.Id, mockCurrentUserId);
            }

            await Task.CompletedTask;
        }

        #endregion
    }
}
