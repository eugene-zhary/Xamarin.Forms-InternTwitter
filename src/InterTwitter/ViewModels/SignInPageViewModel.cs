using System.Threading.Tasks;
using System.Windows.Input;
using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Resources;
using InterTwitter.Services.Authorization;
using InterTwitter.Services.UserService;
using InterTwitter.Views;
using InterTwitter.Views.Flyout;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace InterTwitter.ViewModels
{
    public class SignInPageViewModel : BaseViewModel
    {
        private readonly IPageDialogService _pageDialogService;
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;

        public SignInPageViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IUserService userService,
            IAuthorizationService authorizationService)
            : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _userService = userService;
            _authorizationService = authorizationService;
        }

        #region -- Public Properties --

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private bool _isNextButtonVisible;
        public bool IsNextButtonVisible
        {
            get => _isNextButtonVisible;
            set => SetProperty(ref _isNextButtonVisible, value);
        }
        private bool _isSignInMovableButtonVisible;
        public bool IsSignInMovableButtonVisible
        {
            get => _isSignInMovableButtonVisible;
            set => SetProperty(ref _isSignInMovableButtonVisible, value);
        }

        private bool _shouldEmailEntryBeFocused;
        public bool ShouldEmailEntryBeFocused
        {
            get => _shouldEmailEntryBeFocused;
            set => SetProperty(ref _shouldEmailEntryBeFocused, value);
        }

        private bool _shouldPasswordEntryBeFocused;
        public bool ShouldPasswordEntryBeFocused
        {
            get => _shouldPasswordEntryBeFocused;
            set => SetProperty(ref _shouldPasswordEntryBeFocused, value);
        }

        private bool _isDefaultControlsVisible = true;
        public bool IsDefaultControlsVisible
        {
            get => _isDefaultControlsVisible;
            set => SetProperty(ref _isDefaultControlsVisible, value);
        }

        private ICommand _signInCommand;
        public ICommand SignInCommand => _signInCommand ??= SingleExecutionCommand.FromFunc(OnSignIn);

        private ICommand _signUpCommand;
        public ICommand SignUpCommand => _signUpCommand ??= SingleExecutionCommand.FromFunc(OnSignUp);

        private ICommand _emailEntryFocusedCommand;
        public ICommand EmailEntryFocusedCommand =>
            _emailEntryFocusedCommand ??= SingleExecutionCommand.FromFunc(OnEmailEntryFocused);

        private ICommand _emailEntryUnFocusedCommand;
        public ICommand EmailEntryUnFocusedCommand =>
            _emailEntryUnFocusedCommand ??= SingleExecutionCommand.FromFunc(OnEmailEntryUnFocused);

        private ICommand _passwordEntryFocusedCommand;
        public ICommand PasswordEntryFocusedCommand =>
            _passwordEntryFocusedCommand ??= SingleExecutionCommand.FromFunc(OnPasswordEntryFocused);

        private ICommand _passwordEntryUnFocusedCommand;
        public ICommand PasswordEntryUnFocusedCommand =>
            _passwordEntryUnFocusedCommand ??= SingleExecutionCommand.FromFunc(OnPasswordEntryUnFocused);

        private ICommand _nextButtonClickedCommand;
        public ICommand NextButtonClickedCommand =>
            _nextButtonClickedCommand ??= SingleExecutionCommand.FromFunc(OnNextButtonClicked);

        #endregion

        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(nameof(User), out User user))
            {
                Email = user.Email;
            }
        }

        #endregion

        #region -- Private helpers --

        private async Task OnSignIn()
        {
            if (!string.IsNullOrWhiteSpace(Email) &&
                !string.IsNullOrEmpty(Password))
            {
                Email = Email.Trim();

                var result = await _userService.GetUserAsync(u => u.Email == Email && u.Password == Password);

                if (result.IsSuccess)
                {
                    var authorizedUser = result.Result;

                    _authorizationService.Authorize(authorizedUser.Id);

                    await NavigationService.NavigateAsync($"/{nameof(FlyoutNavigationView)}");
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync(Strings.LogInErrorTitle, Strings.NoSuchUser, Strings.Ok);
                }

            }
            else
            {
                await _pageDialogService.DisplayAlertAsync(Strings.LogInErrorTitle, Strings.EmptyFieldsError,
                    Strings.Ok);
            }
        }

        private async Task OnSignUp()
        {
            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignUpStartPage)}");
        }

        private Task OnEmailEntryFocused()
        {
            IsNextButtonVisible = true;
            IsDefaultControlsVisible = false;

            ShouldEmailEntryBeFocused = true;
            ShouldPasswordEntryBeFocused = false;

            return Task.CompletedTask;
        }

        private Task OnEmailEntryUnFocused()
        {
            IsNextButtonVisible = false;
            IsDefaultControlsVisible = true;

            ShouldEmailEntryBeFocused = false;

            return Task.CompletedTask;
        }

        private Task OnNextButtonClicked()
        {
            ShouldEmailEntryBeFocused = false;
            ShouldPasswordEntryBeFocused = true;

            return Task.CompletedTask;
        }

        private Task OnPasswordEntryFocused()
        {
            IsSignInMovableButtonVisible = true;
            IsDefaultControlsVisible = false;

            ShouldEmailEntryBeFocused = false;
            ShouldPasswordEntryBeFocused = true;

            return Task.CompletedTask;
        }

        private Task OnPasswordEntryUnFocused()
        {
            IsSignInMovableButtonVisible = false;
            IsDefaultControlsVisible = true;

            ShouldPasswordEntryBeFocused = false;

            return Task.CompletedTask;
        }

        #endregion
    }
}
