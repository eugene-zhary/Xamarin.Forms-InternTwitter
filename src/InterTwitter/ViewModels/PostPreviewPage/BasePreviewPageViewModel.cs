using InterTwitter.Helpers;
using InterTwitter.Resources;
using InterTwitter.Services.ContextMenu;
using InterTwitter.Services.Permission;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterTwitter.ViewModels.Posts
{
    public class BasePreviewPageViewModel : BaseViewModel
    {
        public BasePreviewPageViewModel(
            INavigationService navigationService,
            IPageDialogService pageDialogService,
            IContextMenuService contextMenuService,
            IPermissionService permissionService) : base(navigationService)
        {
            PageDialogService = pageDialogService;
            ContextMenuService = contextMenuService;
            PermissionService = permissionService;
        }

        #region -- Public properties --

        protected IPageDialogService PageDialogService { get; private set; }
        protected IContextMenuService ContextMenuService { get; private set; }
        protected IPermissionService PermissionService { get; private set; }

        private BasePostViewModel _postViewModel;
        public BasePostViewModel PostViewModel
        {
            get => _postViewModel;
            set => SetProperty(ref _postViewModel, value, nameof(PostViewModel));
        }

        private bool _isContextMenuVisible;
        public bool IsContextMenuVisible
        {
            get => _isContextMenuVisible;
            set => SetProperty(ref _isContextMenuVisible, value, nameof(IsContextMenuVisible));
        }

        private ICommand _goBackCommand;
        public ICommand GoBackCommand => _goBackCommand ??= SingleExecutionCommand.FromFunc(OnGoBackAsync);

        private ICommand _contextMenuCommand;
        public ICommand ContextMenuCommand => _contextMenuCommand ??= SingleExecutionCommand.FromFunc(OnContextMenuAsync);

        private ICommand _pageFocusCommand;
        public ICommand PageFocusCommand => _pageFocusCommand ??= SingleExecutionCommand.FromFunc(OnPageFocusAsync);

        private ICommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= SingleExecutionCommand.FromFunc(OnSaveAsync);

        private ICommand _shareCommand;
        public ICommand ShareCommand => _shareCommand ??= SingleExecutionCommand.FromFunc(OnShareAsync);

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

        #region -- Protected helpers --

        protected virtual Task OnShareAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnSaveAsync()
        {
            return Task.CompletedTask;
        }

        protected async Task SavePhotoAsync(string MediaPath)
        {
            IsContextMenuVisible = false;

            var result = await ContextMenuService.SaveImg(MediaPath);

            if(result.IsSuccess)
            {
                await PageDialogService.DisplayAlertAsync(Strings.SaveTitle, Strings.SaveSucces, Strings.Ok);
            }
            else
            {
                await PageDialogService.DisplayAlertAsync(Strings.SaveTitle, Strings.SaveFailed, Strings.Ok);
            }
        }

        protected async Task SharePhotoAsync(string MediaPath)
        {
            IsContextMenuVisible = false;

            await ContextMenuService.ShareImg(MediaPath);
        }

        #endregion

        #region -- Private helpers --

        private Task OnGoBackAsync()
        {
            return NavigationService.GoBackAsync();
        }

        private Task OnContextMenuAsync()
        {
            IsContextMenuVisible = true;

            return Task.CompletedTask;
        }

        private Task OnPageFocusAsync()
        {
            IsContextMenuVisible = false;

            return Task.CompletedTask;
        }

        #endregion
    }
}
