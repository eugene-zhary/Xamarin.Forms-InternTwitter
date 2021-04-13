using Prism.Mvvm;
using Prism.Navigation;

namespace InterTwitter.ViewModels
{
    class BaseViewModel : BindableBase, INavigationAware, IInitialize
    {
        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        #region -- Public properies --

        protected INavigationService NavigationService { get; }

        #endregion

        #region -- INavigationAware implementation --

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        #endregion

        #region -- IInitialize implementation --

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        #endregion
    }
}
