using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services;
using InterTwitter.Views.Navigation;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterTwitter.ViewModels.Posts
{
    public abstract class BasePostViewModel : BindableBase
    {
        private readonly IPostService _postManager;
        private readonly INavigationService _navigationService;

        public BasePostViewModel(User userModel, Post postModel, IPostService postManager, INavigationService navigation)
        {
            _userModel = userModel;
            _postModel = postModel;
            _postManager = postManager;
            _navigationService = navigation;
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
        
        private ICommand _navigationToProfileCommand;
        public ICommand NavigationToProfileCommand => _navigationToProfileCommand ??= SingleExecutionCommand.FromFunc(OnNavigationToProfile);

        #endregion

        #region -- Private helpers --

        private async Task OnNavigationToProfile()
        {
            var pairs = new NavigationParameters
            {
                { nameof(UserModel), UserModel }
            };

            await _navigationService.NavigateAsync($"/{nameof(ProfileView)}", pairs);
        }

        private async Task OnLikes()
        {
            IsLiked = !IsLiked;

            if (IsLiked)
            {
                await _postManager.LikePostAsync(PostModel.Id);
            }
            else
            {
                await _postManager.UnlikePostAsync(PostModel.Id);
            }

            RaisePropertyChanged(nameof(LikesCount));
        }

        private async Task OnBookmarks()
        {
            IsBookmarked = !IsBookmarked;

            if (IsBookmarked)
            {
                await _postManager.BookmarkPostAsync(PostModel.Id);
            }
            else
            {
                await _postManager.UnbookmarkPostAsync(PostModel.Id);
            }
        }

        #endregion
    }
}
