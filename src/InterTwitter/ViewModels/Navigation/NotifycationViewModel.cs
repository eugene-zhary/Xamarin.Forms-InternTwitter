using InterTwitter.Services.Notification;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using InterTwitter.Enums;
using InterTwitter.Extensions;
using InterTwitter.Helpers;
using InterTwitter.Services.UserService;
using InterTwitter.Services;
using InterTwitter.Services.Settings;
using InterTwitter.ViewModels.Notification;
using InterTwitter.ViewModels.Posts;

namespace InterTwitter.ViewModels.Navigation
{
    public class NotifycationViewModel : BaseTabViewModel
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly ISettingsManager _settingsManager;

        public NotifycationViewModel(
            INavigationService navigation,
            INotificationService notificationService,
            IUserService userService,
            IPostService postService,
            ISettingsManager settingsManager)
            : base(navigation)
        {
            _notificationService = notificationService;
            _userService = userService;
            _postService = postService;
            _settingsManager = settingsManager;

            IconPath = "ic_notifications_gray";

            NotificationCollection = new ObservableCollection<NotificationViewModel>();
        }

        #region -- Public properties --

        private ObservableCollection<NotificationViewModel> _notificationCollection;
        public ObservableCollection<NotificationViewModel> NotificationCollection
        {
            get => _notificationCollection;
            set => SetProperty(ref _notificationCollection, value);
        }

        private EPageState _pageState;
        public EPageState PageState
        {
            get => _pageState;
            set => SetProperty(ref _pageState, value);
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand => _refreshCommand ??= SingleExecutionCommand.FromFunc(OnRefreshAsync, delayMillisec: 0);

        #endregion

        #region -- Overrides --

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            await UpdateCollectionAsync();
        }

        public override void OnAppearing()
        {
            IconPath = "ic_notifications_blue";
        }

        public override void OnDisappearing()
        {
            IconPath = "ic_notifications_gray";
        }

        #endregion

        #region -- Private helpers --

        private async Task UpdateCollectionAsync()
        {
            // getting all my posts
            var postResult = await _postService.GetPostsAsync(p => p.UserId == _settingsManager.RememberedUserId);

            if (postResult.IsSuccess)
            {
                var posts = new List<BasePostViewModel>(postResult.Result);
                var postIds = posts.Select(p => p.PostModel.Id).ToList();

                // getting all notifications for my posts
                var notificationResult =
                    await _notificationService.GetNotificationsAsync(n => postIds.Contains(n.PostId));

                if (notificationResult.IsSuccess)
                {
                    var notificationModels = notificationResult.Result.ToList();
                    var actorIds = notificationModels.Select(nm => nm.ActorId);

                    // getting all actors (users, who liked/bookmarked)
                    var actorResult = await _userService.GetUsersAsync(u => actorIds.Contains(u.Id));

                    if (actorResult.IsSuccess)
                    {
                        var actors = actorResult.Result.ToList();
                        var notificationViewModels = new List<NotificationViewModel>();

                        foreach (var notification in notificationModels)
                        {
                            notificationViewModels.Add(notification.ToViewModel(
                                actors.First(a => a.Id == notification.ActorId),
                                posts.First(p => p.PostModel.Id == notification.PostId)));
                        }

                        // sort: newest first
                        notificationViewModels.Sort((n1, n2) =>
                            n2.Notification.Id - n1.Notification.Id);

                        NotificationCollection =
                            new ObservableCollection<NotificationViewModel>(notificationViewModels);

                        PageState = EPageState.Normal;
                    }
                    else
                    {
                        PageState = EPageState.Empty;
                    }
                }
                else
                {
                    PageState = EPageState.Empty;
                }
            }
            else
            {
                PageState = EPageState.Empty;
            }
        }

        private async Task OnRefreshAsync()
        {
            await UpdateCollectionAsync();

            IsRefreshing = false;
        }

        #endregion
    }
}
