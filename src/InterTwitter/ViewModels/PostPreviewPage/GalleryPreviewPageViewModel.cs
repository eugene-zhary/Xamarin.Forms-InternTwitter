using InterTwitter.Resources;
using InterTwitter.Services.ContextMenu;
using InterTwitter.Services.Permission;
using Plugin.Permissions;
using Prism.Navigation;
using Prism.Services;
using System.Linq;
using System.Threading.Tasks;

namespace InterTwitter.ViewModels.Posts
{
    public class GalleryPreviewPageViewModel : BasePreviewPageViewModel
    {
        public GalleryPreviewPageViewModel(INavigationService navigationService,
                                           IPageDialogService pageDialogService,
                                           IContextMenuService contextMenuService,
                                           IPermissionManager permissionManager) :
            base(navigationService, pageDialogService, contextMenuService, permissionManager)
        {
        }

        #region -- Public properties --

        private int _selectedMediaIndex;
        public int SelectedMediaIndex
        {
            get => _selectedMediaIndex;
            set => SetProperty(ref _selectedMediaIndex, value, nameof(SelectedMediaIndex));
        }

        private string _selectedMediaPath;
        public string SelectedMediaPath
        {
            get => _selectedMediaPath;
            set => SetProperty(ref _selectedMediaPath, value, nameof(SelectedMediaPath));
        }

        private int _mediaPathCount;
        public int MediaPathCount
        {
            get => _mediaPathCount;
            set => SetProperty(ref _mediaPathCount, value, nameof(MediaPathCount));
        }
        #endregion

        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if(PostViewModel != null)
            {
                MediaPathCount = PostViewModel.PostModel.MediaPaths.Count();
            }
        }

        protected override Task OnShareAsync()
        {
            return SharePhotoAsync(SelectedMediaPath);
        }

        protected override Task OnSaveAsync()
        {
            return SavePhotoAsync(SelectedMediaPath);
        }

        #endregion
    }
}
