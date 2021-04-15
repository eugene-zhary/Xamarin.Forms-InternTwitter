using Prism.Navigation;

namespace InterTwitter.ViewModels.Navigation
{
    public class SearchViewModel : BaseTabViewModel
    {
        public SearchViewModel(INavigationService navigation) : base(navigation)
        {
            IconPath = "ic_search_gray.png";
        }

        #region -- Overrides --

        public override void OnAppearing()
        {
            IconPath = "ic_search_blue.png";
        }

        public override void OnDisappearing()
        {
            IconPath = "ic_search_gray.png";
        }

        #endregion
    }
}
