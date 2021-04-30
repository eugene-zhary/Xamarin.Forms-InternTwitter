using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services.Authorization;
using InterTwitter.Services.Permission;
using InterTwitter.Services.UserService;
using InterTwitter.Validators;
using InterTwitter.Views.Flyout;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace InterTwitter.ViewModels.Navigation
{
    public class ChangeProfileViewModel : BaseViewModel, INavigatedAware
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;
        private readonly IPageDialogService _dialogService;
        private readonly IPermissionService _permissionManager;
        private User CurrentUser;
        public ChangeProfileViewModel(
            INavigationService navigationService, IAuthorizationService AuthorizationService,
            IUserService userService, IPageDialogService dialogService, IPermissionService permissionManager) : base(navigationService)
        {
            _authorizationService = AuthorizationService;
            _userService = userService;
            _dialogService = dialogService;
            _permissionManager = permissionManager;
        }

        #region -- Public properties --

        #region Bindble Properties

        private string _password;
        public string OldPassword
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _NewPassword;
        public string NewPassword
        {
            get => _NewPassword;
            set => SetProperty(ref _NewPassword, value);
        }

        private string _userBackGround;
        public string UserBackGround
        {
            get => _userBackGround;
            set => SetProperty(ref _userBackGround, value);
        }

        private string _userImagePath;
        public string UserImagePath
        {
            get => _userImagePath;
            set => SetProperty(ref _userImagePath, value);
        }

        private string _userMail;
        public string UserMail
        {
            get => _userMail;
            set => SetProperty(ref _userMail, value);
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        #endregion

        private ICommand _navigationToBackCommand;
        public ICommand NavigationToBackCommand => _navigationToBackCommand ??= SingleExecutionCommand.FromFunc(OnNavigationToBackAsync);

        private ICommand _confirmCommand;
        public ICommand ConfirmCommand => _confirmCommand ??= SingleExecutionCommand.FromFunc(OnConfirmAsync);

        private ICommand _profileImagePickCommand;
        public ICommand ProfileImagePickCommand => _profileImagePickCommand ??= SingleExecutionCommand.FromFunc(OnProfileImagePickAsync);

        private ICommand _profileBackgroundPickCommand;
        public ICommand ProfileBackgroundPickCommand => _profileBackgroundPickCommand ??= SingleExecutionCommand.FromFunc(OnProfileBackGroundPickAsync);

        #endregion

        #region -- Overrides --

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            var user = await _userService.GetUserAsync(_authorizationService.GetCurrentUserId);

            if (user.TrackingResult)
            {
                CurrentUser = user.Result;
                UserName = CurrentUser.Name;
                UserMail = CurrentUser.Email;
                UserBackGround = CurrentUser.ProfileBackgroundImagePath;
                UserImagePath = CurrentUser.ProfileImagePath;
            }
        }

        #endregion

        #region -- Private helpers -- 

        private async Task OnProfileImagePickAsync()
        {
            try
            {
                var status = await _permissionManager.RequestPermissionAsync<Permissions.StorageRead>();

                if (status == PermissionStatus.Granted)
                {
                    var file = await MediaPicker.PickPhotoAsync();

                    if (file == null)
                        return;

                    UserImagePath = file.FullPath;
                }
            }
            catch (Exception)
            {
            }
        }

        private async Task OnProfileBackGroundPickAsync()
        {
            try
            {
                var status = await _permissionManager.RequestPermissionAsync<Permissions.StorageRead>();

                if(status == PermissionStatus.Granted)
                {
                    var file = await MediaPicker.PickPhotoAsync();

                    if (file == null)
                        return;

                    UserBackGround = file.FullPath;
                }
            }
            catch (Exception)
            {
            }
        }

        private async Task OnConfirmAsync()
        {
            var Result = await _dialogService.DisplayAlertAsync(string.Empty, Resources.Strings.SaveChanges, Resources.Strings.Confirm, Resources.Strings.Cancel);

            if (Result)
            {
                if (CurrentUser.Email != UserMail && StringValidator.Validate(UserMail, StringValidator.Email))
                {
                    CurrentUser.Email = UserMail;
                }
                if (CurrentUser.Name != UserName && StringValidator.Validate(UserName, StringValidator.Name))
                {
                    CurrentUser.Name = UserName;
                }

                if (!string.IsNullOrWhiteSpace(NewPassword) || !string.IsNullOrWhiteSpace(OldPassword))
                {
                    if (!string.IsNullOrWhiteSpace(NewPassword) && !string.IsNullOrWhiteSpace(OldPassword))
                    {
                        if (NewPassword != OldPassword && StringValidator.Validate(NewPassword, StringValidator.Password) && OldPassword == CurrentUser.Password)
                        {
                            CurrentUser.Password = NewPassword;
                        }
                        else
                        {
                            await _dialogService.DisplayAlertAsync(string.Empty, Resources.Strings.InvalidPasswordMessage, Resources.Strings.Ok);

                            return;
                        }
                    }
                    else
                    {
                        await _dialogService.DisplayAlertAsync(string.Empty, Resources.Strings.YouLlNeedAPassword, Resources.Strings.Ok);

                        return;
                    }
                }

                if (UserImagePath != CurrentUser.ProfileImagePath)
                {
                    CurrentUser.ProfileImagePath = UserImagePath;
                }
                if (UserBackGround != CurrentUser.ProfileBackgroundImagePath)
                {
                    CurrentUser.ProfileBackgroundImagePath = UserBackGround;
                }

                await _userService.UpdateUserAsync(CurrentUser);

                NavigationToBackCommand.Execute(string.Empty);
            }
        }

        private async Task OnNavigationToBackAsync()
        {
            if (NavigationService.GetNavigationUriPath().Count(u => u == '/') >= 2)
            {
                await NavigationService.GoBackAsync();
            }
            else
            {
                await NavigationService.NavigateAsync($"{nameof(MasterDetailNavigationView)}");
            }
        }

        #endregion
    }
}