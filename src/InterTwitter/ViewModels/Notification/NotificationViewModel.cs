using InterTwitter.Models;
using Prism.Mvvm;

namespace InterTwitter.ViewModels.Notification
{
    public class NotificationViewModel : BindableBase
    {
        #region -- Public properties --

        private User _actor;
        public User Actor
        {
            get => _actor;
            set => SetProperty(ref _actor, value);
        }

        private Post _post;
        public Post Post
        {
            get => _post;
            set => SetProperty(ref _post, value);
        }

        private Models.Notification _notification;
        public Models.Notification Notification
        {
            get => _notification;
            set => SetProperty(ref _notification, value);
        }

        #endregion
    }
}
