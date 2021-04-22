using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services;
using Prism.Mvvm;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterTwitter.ViewModels.Posts
{
    public class BasePostViewModel : BindableBase
    {

        public BasePostViewModel(User userModel, Post postModel, IPostService postManager)
        {
            _userModel = userModel;
            _postModel = postModel;
            PostManager = postManager;
        }

        #region -- Public properties --
        protected readonly IPostService PostManager;

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

            if (IsLiked)
            {
                await PostManager.LikePostAsync(PostModel.Id);
            }
            else
            {
                await PostManager.UnlikePostAsync(PostModel.Id);
            }

            RaisePropertyChanged(nameof(LikesCount));
        }

        private async Task OnBookmarks()
        {
            IsBookmarked = !IsBookmarked;

            if (IsBookmarked)
            {
                await PostManager.BookmarkPostAsync(PostModel.Id);
            }
            else
            {
                await PostManager.UnbookmarkPostAsync(PostModel.Id);
            }
        }

        #endregion
    }
}
