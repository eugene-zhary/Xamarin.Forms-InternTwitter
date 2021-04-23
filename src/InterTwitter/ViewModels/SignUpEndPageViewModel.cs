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

        private bool _isNextButtonVisible;
        public bool IsNextButtonVisible
        {
            get => _isNextButtonVisible;
            set => SetProperty(ref _isNextButtonVisible, value);
        }
        private bool _isConfirmMovableButtonVisible;
        public bool IsConfirmMovableButtonVisible
        {
            get => _isConfirmMovableButtonVisible;
            set => SetProperty(ref _isConfirmMovableButtonVisible, value);
        }

        private bool _shouldPasswordEntryBeFocused;
        public bool ShouldPasswordEntryBeFocused
        {
            get => _shouldPasswordEntryBeFocused;
            set => SetProperty(ref _shouldPasswordEntryBeFocused, value);
        }

        private bool _shouldConfirmPasswordEntryBeFocused;
        public bool ShouldConfirmPasswordEntryBeFocused
        {
            get => _shouldConfirmPasswordEntryBeFocused;
            set => SetProperty(ref _shouldConfirmPasswordEntryBeFocused, value);
        }

        private bool _isDefaultControlsVisible = true;
        public bool IsDefaultControlsVisible
        {
            get => _isDefaultControlsVisible;
            set => SetProperty(ref _isDefaultControlsVisible, value);
        }

        private ICommand _goBackCommand;
        public ICommand GoBackCommand => _goBackCommand ??= SingleExecutionCommand.FromFunc(OnGoBack);

        private ICommand _confirmCommand;
        public ICommand ConfirmCommand => _confirmCommand ??= SingleExecutionCommand.FromFunc(OnConfirm);

        private ICommand _passwordEntryFocusedCommand;
        public ICommand PasswordEntryFocusedCommand =>
            _passwordEntryFocusedCommand ??= SingleExecutionCommand.FromFunc(OnPasswordEntryFocused);

        private ICommand _passwordEntryUnFocusedCommand;
        public ICommand PasswordEntryUnFocusedCommand =>
            _passwordEntryUnFocusedCommand ??= SingleExecutionCommand.FromFunc(OnPasswordEntryUnFocused);

        private ICommand _confirmPasswordEntryFocusedCommand;
        public ICommand ConfirmPasswordEntryFocusedCommand =>
            _confirmPasswordEntryFocusedCommand ??= SingleExecutionCommand.FromFunc(OnConfirmPasswordEntryFocused);

        private ICommand _confirmPasswordEntryUnFocusedCommand;
        public ICommand ConfirmPasswordEntryUnFocusedCommand =>
            _confirmPasswordEntryUnFocusedCommand ??= SingleExecutionCommand.FromFunc(OnConfirmPasswordEntryUnFocused);

        private ICommand _nextButtonClickedCommand;
        public ICommand NextButtonClickedCommand =>
            _nextButtonClickedCommand ??= SingleExecutionCommand.FromFunc(OnNextButtonClicked);

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
                                         !ConfirmPassword.Equals(Password);

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

        private Task OnPasswordEntryFocused()
        {
            IsNextButtonVisible = true;
            IsDefaultControlsVisible = false;

            ShouldPasswordEntryBeFocused = true;
            ShouldConfirmPasswordEntryBeFocused = false;

            return Task.CompletedTask;
        }

        private Task OnPasswordEntryUnFocused()
        {
            IsNextButtonVisible = false;
            IsDefaultControlsVisible = true;

            ShouldPasswordEntryBeFocused = false;

            return Task.CompletedTask;
        }

        private Task OnNextButtonClicked()
        {
            ShouldPasswordEntryBeFocused = false;
            ShouldConfirmPasswordEntryBeFocused = true;

            return Task.CompletedTask;
        }

        private Task OnConfirmPasswordEntryFocused()
        {
            IsConfirmMovableButtonVisible = true;
            IsDefaultControlsVisible = false;

            ShouldPasswordEntryBeFocused = false;
            ShouldConfirmPasswordEntryBeFocused = true;

            return Task.CompletedTask;
        }

        private Task OnConfirmPasswordEntryUnFocused()
        {
            IsConfirmMovableButtonVisible = false;
            IsDefaultControlsVisible = true;

            ShouldConfirmPasswordEntryBeFocused = false;

            return Task.CompletedTask;
        }

        #endregion
    }
}
