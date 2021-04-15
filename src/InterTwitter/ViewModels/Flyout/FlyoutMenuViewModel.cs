using InterTwitter.Helpers;
using InterTwitter.Resources;
using InterTwitter.Views.Navigation;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.ViewModels.Flyout
{
    public class FlyoutMenuViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IPageDialogService _pageDialog;

        public FlyoutMenuViewModel(IEventAggregator aggregator, IPageDialogService pageDialog)
        {
            _eventAggregator = aggregator;
            _pageDialog = pageDialog;
            aggregator.GetEvent<MenuItemChangedEvent>().Subscribe(OnMenuItemChanged);

            MenuItems = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel
                {
                    Title = "Home",
                    IconSource = "ic_home_gray.png",
                    TargetType = typeof(HomeView)
                },
                new MenuItemViewModel
                {
                    Title = "Search",
                    IconSource = "ic_search_gray.png",
                    TargetType = typeof(SearchView)
                },
                new MenuItemViewModel
                {
                    Title = "Notifycation",
                    IconSource = "ic_notifications_gray.png",
                    TargetType = typeof(NotifycationView)
                },
                new MenuItemViewModel
                {
                    Title = "Bookmarks",
                    IconSource = "ic_bookmarks_gray.png",
                    TargetType = typeof(BookmarksView)
                }
            };

            SelectedItem = MenuItems.FirstOrDefault();
        }


        #region -- Public properties --

        private MenuItemViewModel _selectedItem;
        public MenuItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value, nameof(SelectedItem));
        }

        public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }

        private ICommand _logoutCommand;
        public ICommand LogoutCommand => _logoutCommand ??= SingleExecutionCommand.FromFunc(OnLogout);


        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(SelectedItem):
                    SendSelectedItem();
                    break;
            }
        }

        #endregion

        #region -- Private helpers --

        private async Task OnLogout()
        {
            bool result = await _pageDialog.DisplayAlertAsync(AppResource.LogoutAlertTitle, AppResource.LogoutAlertBody, AppResource.LogoutAlertOk, AppResource.LogoutAlertCancel);

            if(result)
            {
                // navigate to sign up
            }
        }

        private void SendSelectedItem()
        {
            if(SelectedItem != null)
            {
                _eventAggregator.GetEvent<MenuItemChangedEvent>().Publish(SelectedItem.TargetType);
                _eventAggregator.GetEvent<MenuVisibilityChangedEvent>().Publish(false);
                ChangeVisualState(SelectedItem.TargetType);
            }
        }

        private void OnMenuItemChanged(Type obj)
        {
            if(obj != null && SelectedItem?.TargetType != obj)
            {
                ChangeVisualState(obj);
            }
        }

        private void ChangeVisualState(Type selectedType)
        {
            foreach(var item in MenuItems)
            {
                item.IsSelected = (selectedType == item.TargetType);
                item.TextColor = item.IsSelected ? Color.FromHex("#2356C5") : Color.FromHex("#02060E");

                switch(item.TargetType.Name)
                {
                    case nameof(HomeView):
                        item.IconSource = item.IsSelected ? "ic_home_blue.png" : "ic_home_gray.png";
                        break;

                    case nameof(SearchView):
                        item.IconSource = item.IsSelected ? "ic_search_blue.png" : "ic_search_gray.png";
                        break;

                    case nameof(NotifycationView):
                        item.IconSource = item.IsSelected ? "ic_notifications_blue.png" : "ic_notifications_gray.png";
                        break;

                    case nameof(BookmarksView):
                        item.IconSource = item.IsSelected ? "ic_bookmarks_blue.png" : "ic_bookmarks_gray.png";
                        break;
                }
            }
        }

        #endregion
    }
}