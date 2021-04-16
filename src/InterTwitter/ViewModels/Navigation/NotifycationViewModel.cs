using Prism.Navigation;

namespace InterTwitter.ViewModels.Navigation
{
    public class NotifycationViewModel : BaseTabViewModel
    {
        public NotifycationViewModel(INavigationService navigation) : base(navigation)
        {
            IconPath = "ic_notifications_gray.png";
        }

        #region -- Overrides --

        public override void OnAppearing()
        {
            IconPath = "ic_notifications_blue.png";
        }

        public override void OnDisappearing()
        {
            IconPath = "ic_notifications_gray.png";
        }

        #endregion
    }
}
