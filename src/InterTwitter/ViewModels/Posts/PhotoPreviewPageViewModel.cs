using InterTwitter.Helpers;
using InterTwitter.Services;
using InterTwitter.Services.Permission;
using Prism.Navigation;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InterTwitter.ViewModels.Posts
{
    public class PhotoPreviewPageViewModel : BaseViewModel
    {
        private readonly IMediaService _mediaService;
        private readonly IPermissionManager _permissionManager;

        public PhotoPreviewPageViewModel(INavigationService navigationService, IPermissionManager permissionManager) : base(navigationService)
        {
            _mediaService = DependencyService.Get<IMediaService>();
            _permissionManager = permissionManager;
        }

        #region -- Public properties --

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

        private async Task OnGoBackAsync()
        {
            await NavigationService.GoBackAsync(null, null, animated: false);
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

        private async Task OnShareAsync()
        {
            IsContextMenuVisible = false;

            string mediaUri = PostViewModel.PostModel.MediaPaths?.FirstOrDefault();

            if(mediaUri != null)
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Title = "InterTwitter",
                    Uri = mediaUri
                });
            }
        }

        private async Task OnSaveAsync()
        {
            IsContextMenuVisible = false;

            //TODO: save

            if(await _permissionManager.RequestStoragePermissionAsync())
            {
                string mediaUri = PostViewModel.PostModel.MediaPaths?.FirstOrDefault();

                if(mediaUri != null)
                {
                    using var webClient = new WebClient();

                    webClient.DownloadDataAsync(new Uri(mediaUri));
                    webClient.DownloadDataCompleted += WebClient_DownloadDataCompleted;
                }
            }
        }

        private void WebClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            _mediaService.SaveImageFromByte(e.Result, DateTime.Now.ToLongTimeString());
        }

        #endregion
    }
}
