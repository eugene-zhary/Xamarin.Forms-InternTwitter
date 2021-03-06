using InterTwitter.Helpers;
using InterTwitter.ViewModels.Posts;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterTwitter.ViewModels.PostPage
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

        private Task OnGoBackAsync()
        {
            return NavigationService.GoBackAsync();
        }

        protected virtual Task OnNavigateToPreviewAsync()
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
