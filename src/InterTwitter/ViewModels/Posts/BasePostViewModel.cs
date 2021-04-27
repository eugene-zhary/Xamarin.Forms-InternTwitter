using InterTwitter.Enums;
using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services;
using InterTwitter.Views.Navigation;
using InterTwitter.Views.PostPage;
using Prism.Mvvm;
using Prism.Navigation;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterTwitter.ViewModels.Posts
{
    public class BasePostViewModel : BindableBase
    {
        public BasePostViewModel(User userModel, Post postModel)
        {
            NavigationService = App.Resolve<INavigationService>();
            PostManager = App.Resolve<IPostService>();

            _userModel = userModel;
            _postModel = postModel;
        }

        #region -- Public properties --

        protected INavigationService NavigationService { get; private set; }
        protected IPostService PostManager { get; private set; }

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
        public ICommand LikesCommand => _likesCommand ??= SingleExecutionCommand.FromFunc(OnLikesAsync);

        private ICommand _bookmarksCommand;
        public ICommand BookmarksCommand => _bookmarksCommand ??= SingleExecutionCommand.FromFunc(OnBookmarksAsync);

        private ICommand _openPostCommand;
        public ICommand OpenPostCommand => _openPostCommand ??= SingleExecutionCommand.FromFunc(OnOpenPostAsync);

        #endregion

        #region -- Private helpers --

        private async Task OnNavigationToProfile()
        {
            var pairs = new NavigationParameters
            {
                { nameof(UserModel), UserModel }
            };

            await NavigationService.NavigateAsync($"/{nameof(ProfileView)}", pairs);
        }

        private async Task OnLikesAsync()
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

        private async Task OnBookmarksAsync()
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
        private async Task OnOpenPostAsync()
        {
            var paramenters = new NavigationParameters
            {
                { nameof(BasePostViewModel), this }
            };

            switch (PostModel.MediaType)
            {
                case EMediaType.Gallery:
                    await NavigationService.NavigateAsync($"/{nameof(GalleryPostPage)}", paramenters);
                    break;

                case EMediaType.Photo:
                    await NavigationService.NavigateAsync($"/{nameof(PhotoPostPage)}", paramenters);
                    break;

                case EMediaType.Gif:
                    await NavigationService.NavigateAsync($"/{nameof(GifPostPage)}", paramenters);
                    break;

                case EMediaType.Video:
                    await NavigationService.NavigateAsync($"/{nameof(VideoPostPage)}", paramenters);
                    break;

            }
        }

        #endregion
    }
}
