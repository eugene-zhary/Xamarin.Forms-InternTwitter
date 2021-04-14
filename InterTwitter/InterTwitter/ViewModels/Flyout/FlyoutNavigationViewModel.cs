using InterTwitter.Models;
using InterTwitter.Views.Navigation;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace InterTwitter.ViewModels.Flyout
{
    public class FlyoutNavigationViewModel : BaseViewModel
    {
        public FlyoutNavigationViewModel(INavigationService navigation) : base(navigation)
        {
            MenuItems = new List<FlyoutMenuItem>
            {
                new FlyoutMenuItem
                {
                    Title = "Home",
                    IconSource = "ic_home_gray.png",
                    TargetType = typeof(HomeView)
                },
                new FlyoutMenuItem
                {
                    Title = "Search",
                    IconSource = "ic_search_gray.png",
                    TargetType = typeof(SearchView)
                },
                new FlyoutMenuItem
                {
                    Title = "Notifycation",
                    IconSource = "ic_notifications_gray.png",
                    TargetType = typeof(NotifycationView)
                },
                new FlyoutMenuItem
                {
                    Title = "Bookmarks",
                    IconSource = "ic_bookmarks_gray.png",
                    TargetType = typeof(BookmarksView)
                }
            };

            SelectedItem = MenuItems.FirstOrDefault();
        }

        #region -- Public properties --

        public List<FlyoutMenuItem> MenuItems { get; set; }

        private FlyoutMenuItem _selectedItem;
        public FlyoutMenuItem SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value, nameof(SelectedItem));
        }

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch(args.PropertyName)
            {
                case nameof(SelectedItem):

                    break;
            }
        }

        #endregion

        #region -- Private helpers --



        #endregion
    }
}