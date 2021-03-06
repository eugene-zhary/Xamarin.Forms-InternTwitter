using InterTwitter.Helpers;
using InterTwitter.Services;
using InterTwitter.Services.Authorization;
using InterTwitter.Services.UserService;
using InterTwitter.ViewModels.Posts;
using InterTwitter.Views.PostPage;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterTwitter.ViewModels.Navigation
{
    public class HomeViewModel : BaseTabViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IPostService _postManager;
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;

        public HomeViewModel(INavigationService navigation, IEventAggregator eventAggregator, IPostService postManager, IUserService userService, IAuthorizationService authorizationService) : base(navigation)
        {
            _eventAggregator = eventAggregator;
            _postManager = postManager;
            _userService = userService;
            _authorizationService = authorizationService;

            IconPath = "ic_home_gray.png";
            PostCollection = new ObservableCollection<BasePostViewModel>();
        }

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set => SetProperty(ref _imagePath, value);
        }

        #region -- Public region --

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value, nameof(IsRefreshing));
        }

        public ObservableCollection<BasePostViewModel> PostCollection { get; set; }

        private ICommand _picProfileTapGestureRecognizer;
        public ICommand PicProfileTapGestureRecognizer => _picProfileTapGestureRecognizer ??= SingleExecutionCommand.FromFunc(OnPicProfileTapGestureRecognizerAsync);

        private ICommand _refreshCommand;
        public ICommand RefreshCommand => _refreshCommand ??= SingleExecutionCommand.FromFunc(OnRefreshAsync, delayMillisec: 0);

        private ICommand _addPostCommand;
        public ICommand AddPostCommand => _addPostCommand ??= SingleExecutionCommand.FromFunc(OnAddPostAsync);

        #endregion

        #region -- Overrides --

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            ImagePath = (await _userService.GetUserAsync(_authorizationService.GetCurrentUserId)).Result.ProfileImagePath;

            await UpdateCollecitonAsync();
        }

        public override void OnAppearing()
        {
            IconPath = "ic_home_blue.png";
        }

        public override void OnDisappearing()
        {
            IconPath = "ic_home_gray.png";
        }

        #endregion

        #region -- Private helpers --

        private Task OnAddPostAsync()
        {
            return NavigationService.NavigateAsync(nameof(AddPostPage), null, true, true);
        }

        private Task OnRefreshAsync()
        {
            return UpdateCollecitonAsync();
        }

        private async Task<AOResult> UpdateCollecitonAsync()
        {
            var result = new AOResult();

            IsRefreshing = true;

            try
            {
                PostCollection.Clear();

                var posts = await _postManager.GetPostsAsync();

                foreach(var post in posts.Result)
                {
                    PostCollection.Add(post);
                }

                result.SetSuccess();
            }
            catch(Exception ex)
            {
                result.SetError($"{nameof(UpdateCollecitonAsync)}", "Something went wrong", ex);
            }

            IsRefreshing = false;

            return result;
        }

        private Task OnPicProfileTapGestureRecognizerAsync()
        {
            _eventAggregator.GetEvent<MenuVisibilityChangedEvent>().Publish(true);

            return Task.CompletedTask;
        }

        #endregion
    }
}
