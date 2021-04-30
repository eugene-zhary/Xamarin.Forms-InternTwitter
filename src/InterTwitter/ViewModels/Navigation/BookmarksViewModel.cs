using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using InterTwitter.Helpers;
using InterTwitter.Resources;
using InterTwitter.Services;
using InterTwitter.Services.Authorization;
using InterTwitter.ViewModels.Posts;
using Prism.Navigation;
using Prism.Services;

namespace InterTwitter.ViewModels.Navigation
{
    public class BookmarksViewModel : BaseTabViewModel
    {
        private readonly IPostService _postService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IPageDialogService _pageDialogService;

        public BookmarksViewModel(INavigationService navigation,
            IPostService postService,
            IAuthorizationService authorizationService,
            IPageDialogService pageDialogService)
            : base(navigation)
        {
            _postService = postService;
            _authorizationService = authorizationService;
            _pageDialogService = pageDialogService;

            IconPath = "ic_bookmarks_gray.png";

            PostCollection = new ObservableCollection<BasePostViewModel>();
        }

        #region -- Public properties --

        private ObservableCollection<BasePostViewModel> _postCollection;
        public ObservableCollection<BasePostViewModel> PostCollection
        {
            get => _postCollection;
            set => SetProperty(ref _postCollection, value);
        }

        private bool _isThereAnyBookmarks;
        public bool IsThereAnyBookmarks
        {
            get => _isThereAnyBookmarks;
            set => SetProperty(ref _isThereAnyBookmarks, value);
        }

        private bool _isEmptyState;
        public bool IsEmptyState
        {
            get => _isEmptyState;
            set => SetProperty(ref _isEmptyState, value);
        }

        private bool _isMenuButtonVisible;
        public bool IsMenuButtonVisible
        {
            get => _isMenuButtonVisible;
            set => SetProperty(ref _isMenuButtonVisible, value);
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand => _refreshCommand ??= SingleExecutionCommand.FromFunc(OnRefresh, delayMillisec: 0);

        private ICommand _hiddenMenuTapCommand;

        public ICommand HiddenMenuTapCommand =>
            _hiddenMenuTapCommand ??= SingleExecutionCommand.FromFunc(OnHiddenMenuTap);

        private ICommand _hideMenuButtonCommand;

        public ICommand HideMenuButtonCommand =>
            _hideMenuButtonCommand ??= SingleExecutionCommand.FromFunc(OnHideMenuButton);

        private ICommand _deleteAllBookmarksCommand;
        public ICommand DeleteAllBookmarksCommand =>
            _deleteAllBookmarksCommand ??= SingleExecutionCommand.FromFunc(OnDeleteAllBookmarksAsync);

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(PostCollection))
            {
                if (PostCollection == null)
                {
                    IsThereAnyBookmarks = false;
                    IsEmptyState = true;
                }
                else
                {
                    IsThereAnyBookmarks = PostCollection.Any();
                    IsEmptyState = !PostCollection.Any();
                }
            }
            else if (args.PropertyName == nameof(IsThereAnyBookmarks))
            {
                if (IsThereAnyBookmarks == false)
                {
                    IsMenuButtonVisible = false;
                }
            }
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            await UpdateCollectionAsync();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            IsMenuButtonVisible = false;
        }

        public override void OnAppearing()
        {
            IconPath = "ic_bookmarks_blue.png";
        }

        public override void OnDisappearing()
        {
            IconPath = "ic_bookmarks_gray.png";
        }

        #endregion

        #region -- Private helpers --

        private async Task UpdateCollectionAsync()
        {
            PostCollection.Clear();

            var result = await _postService.GetPostsAsync(p =>
                p.BookmarkedUserIds.Contains(_authorizationService.GetCurrentUserId));

            if (result.IsSuccess)
            {
                IList<BasePostViewModel> posts = result.Result.ToList();

                PostCollection = new ObservableCollection<BasePostViewModel>(posts);
            }
            else
            {
                PostCollection = new ObservableCollection<BasePostViewModel>();
            }

        }

        private async Task OnRefresh()
        {
            IsMenuButtonVisible = false;

            await UpdateCollectionAsync();

            IsRefreshing = false;
        }

        private Task OnHiddenMenuTap()
        {
            IsMenuButtonVisible = !IsMenuButtonVisible;

            return Task.CompletedTask;
        }

        private Task OnHideMenuButton()
        {
            IsMenuButtonVisible = false;

            return Task.CompletedTask;
        }

        private async Task OnDeleteAllBookmarksAsync()
        {
            IsMenuButtonVisible = false;

            var wasConfirmed =
                await _pageDialogService.DisplayAlertAsync(Strings.DeleteAllBookmarksConfirmTitle,
                    Strings.DeleteAllBookmarksConfirmMessage, Strings.Yes, Strings.Cancel);

            if (wasConfirmed)
            {
                foreach (var post in PostCollection)
                {
                    await _postService.UnbookmarkPostAsync(post.PostModel.Id);
                }

                await UpdateCollectionAsync();
            }
        }

        #endregion
    }
}
