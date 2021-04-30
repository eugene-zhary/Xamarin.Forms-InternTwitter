using InterTwitter.Enums;
using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Resources;
using InterTwitter.Services;
using InterTwitter.Services.Permission;
using InterTwitter.Services.Settings;
using InterTwitter.Services.UserService;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;

namespace InterTwitter.ViewModels.PostPage
{
    public class AddPostPageViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly ISettingsManager _settingsManager;
        private readonly IPermissionService _permissionService;
        private readonly IPageDialogService _pageDialogService;

        public AddPostPageViewModel(
            INavigationService navigationService,
            IUserService userService,
            IPostService postService,
            ISettingsManager settingsManager,
            IPermissionService permissionService,
            IPageDialogService pageDialogService)
            : base(navigationService)
        {
            _userService = userService;
            _postService = postService;
            _settingsManager = settingsManager;
            _permissionService = permissionService;
            _pageDialogService = pageDialogService;

            _postText = String.Empty;
            _isEmpty = true;

            MediaPaths = new ObservableCollection<string>();
            _isMediaItemsVisible = false;

            _mediaEnabled = true;
            _videoEnabled = true;

            _mediaState = EMediaState.Empty;
        }

        #region -- Public properties --

        public ObservableCollection<string> MediaPaths { get; set; }

        private string _videoPath;
        public string VideoPath
        {
            get => _videoPath;
            set => SetProperty(ref _videoPath, value, nameof(VideoPath));
        }

        private EMediaState _mediaState;
        public EMediaState MediaState
        {
            get => _mediaState;
            set => SetProperty(ref _mediaState, value, nameof(MediaState));
        }

        private bool _isMediaItemsVisible;
        public bool IsMediaItemsVisible
        {
            get => _isMediaItemsVisible;
            set => SetProperty(ref _isMediaItemsVisible, value, nameof(IsMediaItemsVisible));
        }

        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set => SetProperty(ref _currentUser, value, nameof(CurrentUser));
        }

        private string _postText;
        public string PostText
        {
            get => _postText;
            set => SetProperty(ref _postText, value, nameof(PostText));
        }

        private bool _isEmpty;
        public bool IsEmpty
        {
            get => _isEmpty;
            set => SetProperty(ref _isEmpty, value, nameof(IsEmpty));
        }

        private bool _mediaEnabled;
        public bool MediaEnabled
        {
            get => _mediaEnabled;
            set => SetProperty(ref _mediaEnabled, value, nameof(MediaEnabled));
        }

        private bool _videoEnabled;
        public bool VideoEnabled
        {
            get => _videoEnabled;
            set => SetProperty(ref _videoEnabled, value, nameof(VideoEnabled));
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ??= SingleExecutionCommand.FromFunc(OnCancelAsync);

        private ICommand _postCommand;
        public ICommand PostCommand => _postCommand ??= SingleExecutionCommand.FromFunc(OnPostAsync);

        private ICommand _mediaCommand;
        public ICommand MediaCommand => _mediaCommand ??= SingleExecutionCommand.FromFunc(OnMediaAsync);

        private ICommand _videoCommand;
        public ICommand VideoCommand => _videoCommand ??= SingleExecutionCommand.FromFunc(OnVideoAsync);

        private ICommand _removeMediaItemCommand;
        public ICommand RemoveMediaItemCommand => _removeMediaItemCommand ??= SingleExecutionCommand.FromFunc<string>(OnRemoveMediaItemAsync);

        private ICommand _removeVidoCommand;
        public ICommand RemoveVidoCommand => _removeVidoCommand ??= SingleExecutionCommand.FromFunc(OnRemoveVideoAsync);

        #endregion


        #region -- Overrides --

        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            await InitUserAsync();
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if(args.PropertyName.Equals(nameof(PostText)))
            {
                IsEmpty = String.IsNullOrEmpty(PostText);
            }
        }

        #endregion


        #region -- Private helpers --


        private Task OnCancelAsync()
        {
            return NavigationService.GoBackAsync();
        }

        private async Task OnPostAsync()
        {
            if(!IsEmpty)
            {
                var post = CreatePost();
                await _postService.AddPostAsync(post.Result);
                await NavigationService.GoBackAsync();
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync(Strings.PostTitle, Strings.PostEmpty, Strings.Cancel);
            }
        }

        private Task OnRemoveMediaItemAsync(string arg)
        {
            if(MediaPaths.Contains(arg))
            {
                MediaPaths.Remove(arg);
                IsMediaItemsVisible = MediaPaths.Any();
            }

            if(MediaPaths.Count == 0)
            {
                VideoEnabled = MediaEnabled = true;

                MediaState = EMediaState.Empty;
            }

            else if(MediaPaths.Count > 0)
            {
                MediaEnabled = true;
            }

            return Task.CompletedTask;
        }

        private Task OnRemoveVideoAsync()
        {
            VideoPath = String.Empty;

            VideoEnabled = MediaEnabled = true;

            IsMediaItemsVisible = false;

            MediaState = EMediaState.Empty;

            return Task.CompletedTask;
        }

        private async Task OnMediaAsync()
        {
            if(MediaPaths.Count < 6 && MediaEnabled)
            {
                var status = await _permissionService.RequestPermissionAsync<Permissions.StorageRead>();

                if(status == PermissionStatus.Granted)
                {
                    var file = await MediaPicker.PickPhotoAsync();

                    if(file != null)
                    {
                        MediaPaths.Add(file.FullPath);
                    }

                    IsMediaItemsVisible = true;
                    VideoEnabled = false;
                }

                MediaState = EMediaState.Media;
            }

            if(MediaPaths.Count == 6)
            {
                MediaEnabled = false;
            }
        }

        private async Task OnVideoAsync()
        {
            if(VideoEnabled)
            {
                var status = await _permissionService.RequestPermissionAsync<Permissions.StorageRead>();

                if(status == PermissionStatus.Granted)
                {
                    var file = await MediaPicker.PickVideoAsync();

                    if(file != null)
                    {
                        VideoPath = file.FullPath;
                    }

                    IsMediaItemsVisible = true;
                    MediaEnabled = VideoEnabled = false;
                }

                MediaState = EMediaState.Video;
            }
        }

        private async Task InitUserAsync()
        {
            var userModel = await _userService.GetUserAsync(_settingsManager.RememberedUserId);

            if(userModel.IsSuccess)
            {
                CurrentUser = userModel.Result;
            }
        }

        private AOResult<Post> CreatePost()
        {
            var result = new AOResult<Post>();

            try
            {
                var newPost = new Post
                {
                    UserId = _settingsManager.RememberedUserId,
                    Text = PostText,
                    CreationDateTime = DateTime.Now,
                    LikedUserIds = new List<int>(),
                    BookmarkedUserIds = new List<int>(),
                    MediaPaths = new List<string>()
                };

                if(MediaState == EMediaState.Empty)
                {
                    newPost.MediaType = EMediaType.Empty;
                }
                else if(MediaState == EMediaState.Media)
                {
                    newPost.MediaType = (MediaPaths.Count == 1) ? EMediaType.Photo : EMediaType.Gallery;
                    newPost.MediaPaths = MediaPaths;
                }
                else if(MediaState == EMediaState.Video)
                {
                    newPost.MediaType = EMediaType.Video;
                    newPost.MediaPaths = new List<string> { VideoPath };
                }

                result.SetSuccess(newPost);
            }
            catch(Exception ex)
            {
                result.SetError($"{nameof(CreatePost)}: exception", "Something went wrong", ex);
            }

            return result;
        }

        #endregion
    }
}
