using InterTwitter.Helpers;
using Prism.Navigation;

namespace InterTwitter.ViewModels.Navigation
{
    public class BaseTabViewModel : BaseViewModel, IViewActionsHandler
    {
        public BaseTabViewModel(INavigationService navigation) : base(navigation)
        {
        }

        #region -- Public properties --

        private string _iconPath;
        public string IconPath
        {
            get => _iconPath;
            set => SetProperty(ref _iconPath, value, nameof(IconPath));
        }

        #endregion

        #region -- IActiveAware implementation --

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }

        #endregion
    }
}
