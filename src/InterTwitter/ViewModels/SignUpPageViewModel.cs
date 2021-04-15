using System.Threading.Tasks;
using System.Windows.Input;
using InterTwitter.Helpers;
using Prism.Navigation;

namespace InterTwitter.ViewModels
{
    public class SignUpPageViewModel : BaseViewModel
    {
        public SignUpPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
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

        private ICommand _SignUpCommand;
        public ICommand SignUpCommand => _SignUpCommand ?? (_SignUpCommand = SingleExecutionCommand.FromFunc(OnSignUp));

        #endregion

        #region -- Private Helpers --

        private Task OnSignUp()
        {

            return Task.CompletedTask;
        }

        #endregion
    }
}
