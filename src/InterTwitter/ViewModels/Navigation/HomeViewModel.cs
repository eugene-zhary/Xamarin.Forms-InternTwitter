using InterTwitter.Helpers;
using InterTwitter.Services;
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

        public HomeViewModel(INavigationService navigation, IEventAggregator eventAggregator, IPostService postManager) : base(navigation)
        {
            _eventAggregator = eventAggregator;
            _postManager = postManager;

            IconPath = "ic_home_gray.png";
            PostCollection = new ObservableCollection<BasePostViewModel>();
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

        
        #endregion

        #region -- Overrides --

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

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
