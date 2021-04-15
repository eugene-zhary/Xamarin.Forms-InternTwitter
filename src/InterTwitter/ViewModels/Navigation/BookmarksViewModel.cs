using Prism.Navigation;

namespace InterTwitter.ViewModels.Navigation
{
    public class BookmarksViewModel : BaseTabViewModel
    {
        public BookmarksViewModel(INavigationService navigation) : base(navigation)
        {
            IconPath = "ic_bookmarks_gray.png";
        }

        #region -- Overrides --

        public override void OnAppearing()
        {
            IconPath = "ic_bookmarks_blue.png";
        }

        public override void OnDisappearing()
        {
            IconPath = "ic_bookmarks_gray.png";
        }

        #endregion
    }
}
