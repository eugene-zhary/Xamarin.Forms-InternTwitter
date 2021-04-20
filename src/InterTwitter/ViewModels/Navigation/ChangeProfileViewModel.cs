using InterTwitter.Helpers;
using InterTwitter.Views.Flyout;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterTwitter.ViewModels.Navigation
{
    public class ChangeProfileViewModel : BaseViewModel
    {
        public ChangeProfileViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region -- Public properties --

        private string _password;
        public string Password
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

        private ICommand _navigationToBackCommand;
        public ICommand NavigationToBackCommand => _navigationToBackCommand ??= SingleExecutionCommand.FromFunc(OnNavigationToBackCommand);

        #endregion

        private async Task OnNavigationToBackCommand()
        {
            if (NavigationService.GetNavigationUriPath().Count(u => u == '/') >= 2)
            {
                await NavigationService.GoBackAsync();
            }
            else
            {
                await NavigationService.NavigateAsync($"{nameof(FlyoutNavigationView)}");
            }
        }
    }
}
