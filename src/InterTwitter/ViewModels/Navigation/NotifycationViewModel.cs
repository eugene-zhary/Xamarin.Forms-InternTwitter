using InterTwitter.Services.Notification;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using InterTwitter.Extensions;
using InterTwitter.Services.UserService;
using InterTwitter.Services;
using InterTwitter.Services.Settings;
using InterTwitter.ViewModels.Notification;

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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            await UpdateCollectionAsync();
        }

        #endregion

        #region -- Private helpers --

        private async Task UpdateCollectionAsync()
        {
            // getting all my posts
            var postResult = await _postService.GetPostsAsync(p => p.UserId == _settingsManager.RememberedUserId);

            if (postResult.IsSuccess)
            {
                var posts = postResult.Result.ToList();

                List<NotificationViewModel> notificationViewModels = new List<NotificationViewModel>();

                foreach (var post in posts)
                {
                    // getting all notification for each my post
                    var notificationResult =
                        await _notificationService.GetNotificationsAsync(n => n.PostId == post.PostModel.Id);

                    if (notificationResult.IsSuccess)
                    {
                        var notifications = notificationResult.Result.ToList();

                        foreach (var notification in notifications)
                        {
                            // getting actor of each notification (the user who liked/bookmarked)
                            var actorResult = await _userService.GetUserAsync(notification.ActorId);

                            if (actorResult.IsSuccess)
                            {
                                notificationViewModels.Add(notification.ToViewModel(actorResult.Result, post));
                            }
                        }
                    }
                }

                // sort: newest first
                notificationViewModels.Sort((n1, n2) =>
                {
                    int result = 0;

                    if (n1.Notification.Id > n2.Notification.Id)
                    {
                        result = -1;
                    }
                    else if (n1.Notification.Id < n2.Notification.Id)
                    {
                        result = 1;
                    }

                    return result;
                });

                NotificationCollection = new ObservableCollection<NotificationViewModel>(notificationViewModels);
            }
        }

        #endregion
    }
}
