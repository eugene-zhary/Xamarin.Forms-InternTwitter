using InterTwitter.Helpers;
using InterTwitter.ViewModels.Posts;
using InterTwitter.Views.Flyout;
using InterTwitter.Views.PostPage;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterTwitter.ViewModels.Navigation
{
    public class BasePostPageViewModel : BaseViewModel
    {
        public BasePostPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        #region -- Public properties --

        private BasePostViewModel _postViewModel;
        public BasePostViewModel PostViewModel
        {
            get => _postViewModel;
            set => SetProperty(ref _postViewModel, value, nameof(PostViewModel));
        }

        private ICommand _goBackCommand;
        public ICommand GoBackCommand => _goBackCommand ??= SingleExecutionCommand.FromFunc(OnGoBackAsync);

        private ICommand _navigateToPreviewCommand;
        public ICommand NavigateToPreviewCommand => _navigateToPreviewCommand ??= SingleExecutionCommand.FromFunc(OnNavigateToPreviewAsync);

        #endregion

        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if(parameters.ContainsKey(nameof(BasePostViewModel)))
            {
                PostViewModel = parameters.GetValue<BasePostViewModel>(nameof(BasePostViewModel));
            }
        }

        #endregion

        #region -- Private helpers --

        private async Task OnGoBackAsync()
        {
            await NavigationService.NavigateAsync($"/{nameof(FlyoutNavigationView)}");
        }

        private async Task OnNavigateToPreviewAsync()
        {
            var parameters = new NavigationParameters
            {
                { nameof(BasePostViewModel), PostViewModel }
            };

            await NavigationService.NavigateAsync($"{nameof(PhotoPreviewPage)}", parameters, null, animated: false);
        }


        #endregion
    }
}
