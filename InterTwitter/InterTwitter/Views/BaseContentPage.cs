using InterTwitter.Helpers;
using Prism.Mvvm;
using Xamarin.Forms;

namespace InterTwitter.Views
{
    public class BaseContentPage : ContentPage
    {
        public BaseContentPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            ViewModelLocator.SetAutowireViewModel(this, true);

            BackgroundColor = Color.Red;
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
