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
using Xamarin.Forms;

namespace InterTwitter.ViewModels.Navigation
{
	public class ProfileViewModel : BaseViewModel
	{
        public ProfileViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
        public ICommand BackNavigationCommand => SingleExecutionCommand.FromFunc(OnBackNavigationCommand);

        private async Task OnBackNavigationCommand()
        {
            await NavigationService.NavigateAsync($"/{nameof(FlyoutNavigationView)}");
        }
    }
}
