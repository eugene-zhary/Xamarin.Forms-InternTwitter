using InterTwitter.Services.ContextMenu;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using InterTwitter.Services.Permission;

namespace InterTwitter.ViewModels.Posts
{
    public class PhotoPreviewPageViewModel : BasePreviewPageViewModel
    {
        private string _mediaPath;

        public PhotoPreviewPageViewModel(INavigationService navigationService,
                                         IPageDialogService pageDialogService,
                                         IContextMenuService contextMenuService,
                                         IPermissionService permissionService)
            : base(navigationService, pageDialogService, contextMenuService, permissionService)
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

        protected override Task OnShareAsync()
        {
            return SharePhotoAsync(_mediaPath);
        }

        protected override Task OnSaveAsync()
        {
            return SavePhotoAsync(_mediaPath);
        }

        #endregion
    }
}
