using System.Threading.Tasks;
using System.Windows.Input;
using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Resources;
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

        public SignUpStartPageViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService)
            : base(navigationService)
        {
            _pageDialogService = pageDialogService;
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

        private ICommand _signUpCommand;
        public ICommand SignUpCommand => _signUpCommand ??= SingleExecutionCommand.FromFunc(OnSignUp);

        private ICommand _logInCommand;
        public ICommand LogInCommand => _logInCommand ??= SingleExecutionCommand.FromFunc(OnLogIn);

        #endregion

        #region -- Private Helpers --

        private async Task OnSignUp()
        {
            Name = Name?.Trim();
            Email = Email?.Trim();

            if (StringValidator.Validate(Name, StringValidator.Name) &&
                StringValidator.Validate(Email, StringValidator.Email))
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
                await _pageDialogService.DisplayAlertAsync(Strings.SignUpErrorTitle,
                    Strings.SignUpErrorMessage, Strings.Ok);
            }
        }

        private async Task OnLogIn()
        {
            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(SignInPage)}");
        }

        #endregion
    }
}
