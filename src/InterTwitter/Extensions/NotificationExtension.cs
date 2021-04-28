using InterTwitter.Models;
using InterTwitter.ViewModels.Notification;

namespace InterTwitter.Extensions
{
    public static class NotificationExtension
    {
        #region -- Public methods --

        public static NotificationViewModel ToViewModel(
            this Models.Notification notification,
            User actor,
            Post post)
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
