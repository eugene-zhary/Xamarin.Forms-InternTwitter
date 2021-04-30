using InterTwitter.Models;
using InterTwitter.ViewModels.Notification;
using InterTwitter.ViewModels.Posts;

namespace InterTwitter.Extensions
{
    public static class NotificationExtension
    {
        #region -- Public methods --

        public static NotificationViewModel ToViewModel(this Notification notification, User actor, BasePostViewModel post)
        {
            var notificationViewModel = new NotificationViewModel
            {
                Notification = notification,
                Actor = actor,
                Post = post
            };

            return notificationViewModel;
        }

        #endregion
    }
}
