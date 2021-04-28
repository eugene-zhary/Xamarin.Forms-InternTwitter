using InterTwitter.Services.Notification;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using InterTwitter.Extensions;
using InterTwitter.Services.UserService;
using InterTwitter.Services;

namespace InterTwitter.ViewModels.Navigation
{
    public class NotifycationViewModel : BaseTabViewModel
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IPostService _postService;

        public NotifycationViewModel(
            INavigationService navigation,
            INotificationService notificationService,
            IUserService userService,
            IPostService postService)
            : base(navigation)
        {
            _notificationService = notificationService;
            _userService = userService;
            _postService = postService;

            IconPath = "ic_notifications_gray";
        }

        #region -- Overrides --

        public override void OnAppearing()
        {
            IconPath = "ic_notifications_blue";
        }

        public override void OnDisappearing()
        {
            IconPath = "ic_notifications_gray";
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var result = await _notificationService.GetNotificationsAsync(u => true);

            if (result.IsSuccess)
            {
                // TODO :
                // получить айдишники всех постов, у которых я - автор
                // получить все уведомленая по этим постам
                // конвертировать их во вбюмодели и добавить обзервабл коллекшн

                var notifics = result.Result.ToList();

                IList<Notification.NotificationViewModel> notificationVms = new List<Notification.NotificationViewModel>();

                foreach (var notification in notifics)
                {
                    var userResult = await _userService.GetUserAsync(notification.ActorId);
                    var postResult = await _postService.GetPostsAsync(p => p.Id == notification.PostId);

                    if (userResult.IsSuccess && postResult.IsSuccess)
                    {
                        var user = userResult.Result;
                        var post = postResult.Result.FirstOrDefault().PostModel;

                        notificationVms.Add(notification.ToViewModel(user, post));
                    }
                }
            }
        }

        #endregion
    }
}
