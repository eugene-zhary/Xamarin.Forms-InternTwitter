using System.Threading.Tasks;
using System.Windows.Input;
using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Resources;
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

        public SignInPageViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            IUserService userService)
            : base(navigationService)
        {
            _pageDialogService = pageDialogService;
            _userService = userService;
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

        private ICommand _signInCommand;
        public ICommand SignInCommand => _signInCommand ??= SingleExecutionCommand.FromFunc(OnSignIn);

        private ICommand _signUpCommand;
        public ICommand SignUpCommand => _signUpCommand ??= SingleExecutionCommand.FromFunc(OnSignUp);

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

        #endregion
    }
}
