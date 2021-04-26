using InterTwitter.Helpers;
using InterTwitter.Services.ContextMenu;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterTwitter.ViewModels.Posts
{
    public class BasePreviewPageViewModel : BaseViewModel
    {
        public BasePreviewPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IContextMenuService contextMenuService) : base(navigationService)
        {
            PageDialogService = pageDialogService;
            ContextMenuService = contextMenuService;
        }

        #region -- Public properties --

        protected IPageDialogService PageDialogService { get; private set; }
        protected IContextMenuService ContextMenuService { get; private set; }

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

        #region -- Private helpers --

        private Task OnGoBackAsync()
        {
            return NavigationService.GoBackAsync();
        }

        private async Task OnContextMenuAsync()
        {
            IsContextMenuVisible = true;

            await Task.CompletedTask;
        }

        private async Task OnPageFocusAsync()
        {
            IsContextMenuVisible = false;

            await Task.CompletedTask;
        }

        protected virtual Task OnShareAsync()
        {
            return Task.CompletedTask;
        }
        protected virtual Task OnSaveAsync()
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
