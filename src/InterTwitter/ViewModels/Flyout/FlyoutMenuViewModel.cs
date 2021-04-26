using InterTwitter.Helpers;
using InterTwitter.Resources;
using InterTwitter.Views;
using InterTwitter.Views.Navigation;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using InterTwitter.Services.Authorization;
using Xamarin.Forms;
using InterTwitter.Services.UserService;

namespace InterTwitter.ViewModels.Flyout
{
    public class FlyoutMenuViewModel : BaseViewModel, IViewActionsHandler
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IPageDialogService _pageDialog;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;

        public FlyoutMenuViewModel(INavigationService navigationService, IEventAggregator aggregator, IPageDialogService pageDialog, IAuthorizationService authorizationService, IUserService userService) : base(navigationService)
        {
            _eventAggregator = aggregator;
            _pageDialog = pageDialog;
            _authorizationService = authorizationService;
            _userService = userService;
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
        
        private string _profileImage;
        public string ProfileImage
        {
            get => _profileImage;
            set => SetProperty(ref _profileImage, value);
        }
        private string _profileName;
        public string ProfileName
        {
            get => _profileName;
            set => SetProperty(ref _profileName, value);
        }
        private string _profileMail;
        public string ProfileMail
        {
            get => _profileMail;
            set => SetProperty(ref _profileMail, value);
        }

        public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }

        private ICommand _logoutCommand;
        public ICommand LogoutCommand => _logoutCommand ??= SingleExecutionCommand.FromFunc(OnLogout);

        private ICommand _navigationToProfileCommand;
        public ICommand NavigationToProfileCommand => _navigationToProfileCommand ??= SingleExecutionCommand.FromFunc(OnNavigationToProfileCommand);
      
        private ICommand _navigationToChangeProfileCommand;
        public ICommand NavigationToChangeProfileCommand => _navigationToChangeProfileCommand ??= SingleExecutionCommand.FromFunc(OnNavigationToChangeProfileCommand);

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

        private async Task OnNavigationToProfileCommand()
        {
            await NavigationService.NavigateAsync($"/{nameof(ProfileView)}");
        }

        private async Task OnNavigationToChangeProfileCommand()
        {
            await NavigationService.NavigateAsync($"/{nameof(ChangeProfileView)}");
        }

        private async Task OnLogout()
        {
            bool shouldLogOut = await _pageDialog.DisplayAlertAsync(Strings.LogoutAlertTitle, Strings.LogoutAlertBody,
                Strings.LogoutAlertOk, Strings.LogoutAlertCancel);

            if(shouldLogOut)
            {
                _authorizationService.UnAuthorize();
                await NavigationService.NavigateAsync($"/{nameof(SignUpStartPage)}");
            }
        }

        private void SendSelectedItem()
        {
            if(SelectedItem != null)
            {
                _eventAggregator.GetEvent<MenuItemChangedEvent>().Publish(SelectedItem.TargetType);
                _eventAggregator.GetEvent<MenuVisibilityChangedEvent>().Publish(false);
                ChangeVisualState(SelectedItem.TargetType);
                SelectedItem = null;
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

        public async void OnAppearing()
        {
            var user = await _userService.GetUserAsync(_authorizationService.GetCurrentUserId);

            ProfileImage = user.Result.ProfileImagePath;
            ProfileName = user.Result.Name;
            ProfileMail = user.Result.Email;
        }

        public void OnDisappearing()
        {

        }

        #endregion
    }
}