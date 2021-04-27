using InterTwitter.ViewModels.Posts;
using InterTwitter.Views.PostPage;
using Prism.Navigation;
using System.Threading.Tasks;

namespace InterTwitter.ViewModels.PostPage
{
    public class PhotoPostPageViewModel : BasePostPageViewModel
    {
        public PhotoPostPageViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        #region -- Overrides --

        protected override async Task OnNavigateToPreviewAsync()
        {
            var parameters = new NavigationParameters
            {
                { nameof(BasePostViewModel), PostViewModel }
            };

            await NavigationService.NavigateAsync(nameof(PhotoPreviewPage), parameters, true, true);
        }

        #endregion
    }
}
