using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Resources;
using InterTwitter.Services.UserService;
using InterTwitter.Validators;
using InterTwitter.Views;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace InterTwitter.ViewModels
{
    public class SignUpEndPageViewModel : BaseViewModel
    {
        private readonly IPageDialogService _pageDialogService;
        private readonly IUserService _userService;

        private string _name;
        private string _email;

        public SignUpEndPageViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IUserService userService)
            : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _userService = userService;
        }

        #region -- Public properties --

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private Color _confirmPasswordUnderlineColor;
        public Color ConfirmPasswordUnderlineColor
        {
            get => _confirmPasswordUnderlineColor;
            set => SetProperty(ref _confirmPasswordUnderlineColor, value);
        }

        private bool _isConfirmPasswordErrorTextVisible;
        public bool IsConfirmPasswordErrorTextVisible
        {
            get => _isConfirmPasswordErrorTextVisible;
            set => SetProperty(ref _isConfirmPasswordErrorTextVisible, value);
        }

        private ICommand _goBackCommand;
        public ICommand GoBackCommand => _goBackCommand ??= SingleExecutionCommand.FromFunc(OnGoBack);

        private ICommand _confirmCommand;
        public ICommand ConfirmCommand => _confirmCommand ??= SingleExecutionCommand.FromFunc(OnConfirm);

        #endregion

        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(nameof(User), out User userModel))
            {
                _name = userModel.Name;
                _email = userModel.Email;
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(Password) || 
                args.PropertyName == nameof(ConfirmPassword))
            {
                var shouldShowErrorMsg = !string.IsNullOrEmpty(ConfirmPassword) &&
                                         !Password.Equals(ConfirmPassword);

                if (shouldShowErrorMsg)
                {
                    ConfirmPasswordUnderlineColor = Color.Red;
                    IsConfirmPasswordErrorTextVisible = true;
                }
                else
                {
                    ConfirmPasswordUnderlineColor = Color.Black;
                    IsConfirmPasswordErrorTextVisible = false;
                }
            }
        }

        #endregion

        #region -- Private helpers --

        private async Task OnGoBack()
        {
            await NavigationService.GoBackAsync();
        }

        private async Task OnConfirm()
        {
            if (!string.IsNullOrEmpty(Password) ||
                !string.IsNullOrEmpty(ConfirmPassword))
            {
                if (Password.Equals(ConfirmPassword))
                {
                    if (StringValidator.Validate(Password, StringValidator.Password))
                    {
                        var newUser = new User
                        {
                            Name = _name,
                            Email = _email,
                            Password = Password,
                            BlockedUserIds = new List<int>(),
                            MutedUserIds = new List<int>(),
                            ProfileImagePath = Constants.DEFAULT_PROFILE_IMAGE_PATH
                        };

                        var insertionResult = await _userService.InsertUserAsync(newUser);

                        if (insertionResult.IsSuccess)
                        {
                            var parameters = new NavigationParameters
                            {
                                {nameof(User), newUser}
                            };
                        
                            await NavigationService.NavigateAsync(nameof(SignInPage), parameters);
                        }
                        else
                        {
                            await _pageDialogService.DisplayAlertAsync(Strings.SignUpErrorTitle,
                                insertionResult.Message, Strings.Ok);
                        }
                    }
                    else
                    {
                        await _pageDialogService.DisplayAlertAsync(Strings.SignUpErrorTitle, Strings.InvalidPasswordMessage,
                            Strings.Ok);
                    }
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync(Strings.SignUpErrorTitle, Strings.DoNotMatch,
                        Strings.Ok);
                }
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync(Strings.SignUpErrorTitle, Strings.PasswordLengthError,
                    Strings.Ok);
            }
        }

        #endregion
    }
}
