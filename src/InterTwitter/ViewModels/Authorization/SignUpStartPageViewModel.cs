using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
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
    public class SignUpStartPageViewModel : BaseViewModel
    {
        private readonly IPageDialogService _pageDialogService;
        private readonly IUserService _userService;

        public SignUpStartPageViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IUserService userService)
            : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _userService = userService;
        }

        #region -- Public properties --

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private bool _isNextButtonVisible;
        public bool IsNextButtonVisible
        {
            get => _isNextButtonVisible;
            set => SetProperty(ref _isNextButtonVisible, value);
        }
        private bool _isSignUpMovableButtonVisible;
        public bool IsSignUpMovableButtonVisible
        {
            get => _isSignUpMovableButtonVisible;
            set => SetProperty(ref _isSignUpMovableButtonVisible, value);
        }

        private bool _shouldNameEntryBeFocused;
        public bool ShouldNameEntryBeFocused
        {
            get => _shouldNameEntryBeFocused;
            set => SetProperty(ref _shouldNameEntryBeFocused, value);
        }

        private bool _shouldEmailEntryBeFocused;
        public bool ShouldEmailEntryBeFocused
        {
            get => _shouldEmailEntryBeFocused;
            set => SetProperty(ref _shouldEmailEntryBeFocused, value);
        }

        private bool _isDefaultControlsVisible = true;
        public bool IsDefaultControlsVisible
        {
            get => _isDefaultControlsVisible;
            set => SetProperty(ref _isDefaultControlsVisible, value);
        }

        private ICommand _signUpCommand;
        public ICommand SignUpCommand => _signUpCommand ??= SingleExecutionCommand.FromFunc(OnSignUp);

        private ICommand _logInCommand;
        public ICommand LogInCommand => _logInCommand ??= SingleExecutionCommand.FromFunc(OnLogIn);

        private ICommand _nameEntryFocusedCommand;
        public ICommand NameEntryFocusedCommand =>
            _nameEntryFocusedCommand ??= SingleExecutionCommand.FromFunc(OnNameEntryFocused);

        private ICommand _nameEntryUnFocusedCommand;
        public ICommand NameEntryUnFocusedCommand =>
            _nameEntryUnFocusedCommand ??= SingleExecutionCommand.FromFunc(OnNameEntryUnFocused);

        private ICommand _emailEntryFocusedCommand;
        public ICommand EmailEntryFocusedCommand =>
            _emailEntryFocusedCommand ??= SingleExecutionCommand.FromFunc(OnEmailEntryFocused);

        private ICommand _emailEntryUnFocusedCommand;
        public ICommand EmailEntryUnFocusedCommand =>
            _emailEntryUnFocusedCommand ??= SingleExecutionCommand.FromFunc(OnEmailEntryUnFocused);

        private ICommand _nextButtonClickedCommand;
        public ICommand NextButtonClickedCommand =>
            _nextButtonClickedCommand ??= SingleExecutionCommand.FromFunc(OnNextButtonClicked);

        #endregion

        #region -- Private Helpers --

        private async Task OnSignUp()
        {
            Name = Name?.Trim();
            Email = Email?.Trim();

            if (StringValidator.Validate(Name, StringValidator.Name) &&
                StringValidator.Validate(Email, StringValidator.Email))
            {
                AOResult<User> result;

                using (UserDialogs.Instance.Loading())
                {
                    result = await _userService.GetUserAsync(u => u.Email == Email);
                }

                var doesSuchUserExist = result.IsSuccess;

                if (!doesSuchUserExist)
                {
                    var userModel = new User
                    {
                        Name = Name,
                        Email = Email
                    };
                    var parameters = new NavigationParameters
                    {
                        {nameof(User), userModel}
                    };

                    await NavigationService.NavigateAsync(nameof(SignUpEndPage), parameters);
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync(Strings.SignUpErrorTitle, Strings.SuchUserAlreadyExists,
                        Strings.Ok);
                }
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync(Strings.SignUpErrorTitle,
                    Strings.SignUpErrorMessage, Strings.Ok);
            }
        }

        private async Task OnLogIn()
        {
            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInPage)}");
        }

        private Task OnNameEntryFocused()
        {
            IsNextButtonVisible = true;
            IsDefaultControlsVisible = false;

            ShouldNameEntryBeFocused = true;
            ShouldEmailEntryBeFocused = false;

            return Task.CompletedTask;
        }

        private Task OnNameEntryUnFocused()
        {
            IsNextButtonVisible = false;
            IsDefaultControlsVisible = true;

            ShouldNameEntryBeFocused = false;

            return Task.CompletedTask;
        }

        private Task OnNextButtonClicked()
        {
            ShouldNameEntryBeFocused = false;
            ShouldEmailEntryBeFocused = true;

            return Task.CompletedTask;
        }

        private Task OnEmailEntryFocused()
        {
            IsSignUpMovableButtonVisible = true;
            IsDefaultControlsVisible = false;

            ShouldNameEntryBeFocused = false;
            ShouldEmailEntryBeFocused = true;

            return Task.CompletedTask;
        }

        private Task OnEmailEntryUnFocused()
        {
            IsSignUpMovableButtonVisible = false;
            IsDefaultControlsVisible = true;

            ShouldEmailEntryBeFocused = false;

            return Task.CompletedTask;
        }

        #endregion
    }
}
