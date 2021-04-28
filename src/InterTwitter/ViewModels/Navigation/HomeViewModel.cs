using InterTwitter.Helpers;
using InterTwitter.Services;
using InterTwitter.Services.Authorization;
using InterTwitter.Services.UserService;
using InterTwitter.ViewModels.Posts;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

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

        public ObservableCollection<BasePostViewModel> PostCollection { get; set; }

        public ICommand PicProfileTapGestureRecognizer => new Command(OnPicProfileTapGestureRecognizer);

        #endregion

        #region -- Overrides --

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            ImagePath = (await _userService.GetUserAsync(_authorizationService.GetCurrentUserId)).Result.ProfileImagePath;

            await UpdateCollecitonAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

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

        private async Task<AOResult> UpdateCollecitonAsync()
        {
            var result = new AOResult();

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

            return result;
        }

        private void OnPicProfileTapGestureRecognizer()
        {
            _eventAggregator.GetEvent<MenuVisibilityChangedEvent>().Publish(true);
        }

        #endregion
    }
}
