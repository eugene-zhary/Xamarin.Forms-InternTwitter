using InterTwitter.Enums;
using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services;
using InterTwitter.Views.Navigation;
using InterTwitter.Views.PostPage;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterTwitter.ViewModels.Posts
{
    public class BasePostViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        public BasePostViewModel(User userModel, Post postModel)
        {
            _eventAggregator = App.Resolve<IEventAggregator>();

            NavigationService = App.Resolve<INavigationService>();
            PostService = App.Resolve<IPostService>();

            _userModel = userModel;
            _postModel = postModel;
        }

        #region -- Public properties --

        protected INavigationService NavigationService { get; private set; }
        protected IPostService PostService { get; private set; }

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

        private async Task OnLikesAsync()
        {
            IsLiked = !IsLiked;

            if (IsLiked)
            {
                await PostService.LikePostAsync(PostModel.Id);
            }
            else
            {
                await PostService.UnlikePostAsync(PostModel.Id);
            }

            RaisePropertyChanged(nameof(LikesCount));
        }

        private async Task OnBookmarksAsync()
        {
            IsBookmarked = !IsBookmarked;

            if (IsBookmarked)
            {
                await PostService.BookmarkPostAsync(PostModel.Id);
            }
            else
            {
                await PostService.UnbookmarkPostAsync(PostModel.Id);
            }
        }

        private async Task OnOpenPostAsync()
        {
            _eventAggregator.GetEvent<NavigationEvent>().Publish(this);
            await Task.CompletedTask;
            //var paramenters = new NavigationParameters
            //{
            //    { nameof(BasePostViewModel), this }
            //};

            //switch (PostModel.MediaType)
            //{
            //    case EMediaType.Gallery:
            //        await NavigationService.NavigateAsync($"{nameof(GalleryPostPage)}", paramenters, null, animated: false);
            //        break;

            //    case EMediaType.Photo:
            //        await NavigationService.NavigateAsync($"{nameof(PhotoPostPage)}", paramenters, null, animated: false);
            //        break;

            //    case EMediaType.Gif:
            //        await NavigationService.NavigateAsync($"{nameof(GifPostPage)}", paramenters, null, animated: false);
            //        break;

            //    case EMediaType.Video:
            //        await NavigationService.NavigateAsync($"{nameof(VideoPostPage)}", paramenters, null, animated: false);
            //        break;
            //}
        }

        #endregion
    }
}
