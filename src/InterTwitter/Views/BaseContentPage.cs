using InterTwitter.Helpers;
using Prism.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Page = Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page;

namespace InterTwitter.Views
{
    public class BaseContentPage : ContentPage
    {
        public BaseContentPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            ViewModelLocator.SetAutowireViewModel(this, true);

            Page.SetUseSafeArea(On<iOS>(), true);
        }

        #region -- Overrides --

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if(BindingContext is IViewActionsHandler actionsHandler)
            {
                actionsHandler.OnAppearing();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if(BindingContext is IViewActionsHandler actionsHandler)
            {
                actionsHandler.OnDisappearing();
            }
        }
        
        #endregion
    }
}
