using InterTwitter.Resources;
using InterTwitter.Services.ContextMenu;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;

namespace InterTwitter.ViewModels.Posts
{
    public class PhotoPreviewPageViewModel : BasePreviewPageViewModel
    {
        private string _mediaPath;

        public PhotoPreviewPageViewModel(INavigationService navigationService,
                                         IPageDialogService pageDialogService,
                                         IContextMenuService contextMenuService) :
            base(navigationService, pageDialogService, contextMenuService)
        {
        }

        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if(PostViewModel is OneMediaPostViewModel oneMediaPostViewModel)
            {
                _mediaPath = oneMediaPostViewModel.MediaPath;
            }
        }

        protected override async Task OnShareAsync()
        {
            IsContextMenuVisible = false;

            await ContextMenuService.ShareImg(_mediaPath);
        }

        protected override async Task OnSaveAsync()
        {
            IsContextMenuVisible = false;

            await ContextMenuService.SaveImgFromWeb(_mediaPath);
            await PageDialogService.DisplayAlertAsync(Strings.SaveTitle, Strings.SaveSucces, Strings.Ok);
        }

        #endregion
    }
}
