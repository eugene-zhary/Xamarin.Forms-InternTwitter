using InterTwitter.Resources;
using InterTwitter.Services.ContextMenu;
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
                                           IContextMenuService contextMenuService) : 
            base(navigationService, pageDialogService, contextMenuService)
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

        protected override async Task OnShareAsync()
        {
            IsContextMenuVisible = false;

            await ContextMenuService.ShareImg(SelectedMediaPath);
        }

        protected override async Task OnSaveAsync()
        {
            IsContextMenuVisible = false;

            await ContextMenuService.SaveImgFromWeb(SelectedMediaPath);
            await PageDialogService.DisplayAlertAsync(Strings.SaveTitle, Strings.SaveSucces, Strings.Ok);
        }

        #endregion
    }
}
