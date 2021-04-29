using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services;
using InterTwitter.Services.Authorization;
using InterTwitter.Services.UserService;
using InterTwitter.ViewModels.Posts;
using InterTwitter.Views.Flyout;
using InterTwitter.Views.Navigation;
using PanCardView.Extensions;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.ViewModels.Navigation
{
	public class ProfileViewModel : BaseViewModel, INavigatedAware
	{

		#region Private Variables/Properties

		private readonly IAuthorizationService _authorizationService;
		private readonly IUserService _userService;
		private readonly IPostService _postManager;
		private User CurrentProfile;
		private User CurrentUser;

		#endregion

		#region Constructors

		public ProfileViewModel(INavigationService navigationService, IAuthorizationService AuthorizationService, IUserService userService, IPostService postManager) : base(navigationService)
		{
			_authorizationService = AuthorizationService;
			_userService = userService;
			_postManager = postManager;


			RemoveCurrentItemCommand = new Command(() =>
			{
				if (!Items.Any())
				{
					return;
				}
				Items.RemoveAt(CurrentIndex.ToCyclicalIndex(Items.Count));
			});

			GoToLastCommand = new Command(() =>
			{
				CurrentIndex = Items.Count - 1;
			});

			Items = new ObservableCollection<object>
			{
				new { PostCollection = UserPostCollection, Title = "Posts" },
				new { PostCollection = UserLikePostCollection, Title = "Likes" },
			};
		}

		#endregion

		#region -- Public properties --

		#region Bindble Properties

		private string _userBackGround;
		public string UserBackGround
		{
			get => _userBackGround;
			set => SetProperty(ref _userBackGround, value);
		}

		private string _userImagePath;
		public string UserImagePath
		{
			get => _userImagePath;
			set => SetProperty(ref _userImagePath, value);
		}

		private string _userMail;
		public string UserMail
		{
			get => _userMail;

			set => SetProperty(ref _userMail, value);
		}

		private string _userName;
		public string UserName
		{
			get => _userName;
			set => SetProperty(ref _userName, value);
		}

		private bool _IsAlianShowMenu;
		public bool IsShowAlianMenu
		{
			get => _IsAlianShowMenu;
			set => SetProperty(ref _IsAlianShowMenu, value);
		}

		private bool _IsShowMenu;
		public bool IsShowMenu
		{
			get => _IsShowMenu;
			set => SetProperty(ref _IsShowMenu, value);
		}

		private bool _isRefreshing;
		public bool IsRefreshing
		{
			get => _isRefreshing;
			set => SetProperty(ref _isRefreshing, value, nameof(IsRefreshing));
		}

		private bool _IsAlianProfile;
		public bool IsAlianProfile
		{
			get => _IsAlianProfile;
			set => SetProperty(ref _IsAlianProfile, value);
		}

		private bool _IsYourProfile = true;
		public bool IsYourProfile
		{
			get => _IsYourProfile;
			set => SetProperty(ref _IsYourProfile, value);
		}

		private string _profileInfo;
		public string ProfileInfo
		{
			get => _profileInfo;
			set => SetProperty(ref _profileInfo, value);
		}

		private string _blockedMessage;
		public string BlockedMessage
		{
			get => _blockedMessage;
			set => SetProperty(ref _blockedMessage, value);
		}

		private string _mutedMessage;
		public string MutedMessage
		{
			get => _mutedMessage;
			set => SetProperty(ref _mutedMessage, value);
		}

		private int _currentIndex;
		public int CurrentIndex
		{
			get => _currentIndex;
			set => SetProperty(ref _currentIndex, value);
		}
		

		private ObservableCollection<BasePostViewModel> _UserPostCollection = new ObservableCollection<BasePostViewModel>();
		public ObservableCollection<BasePostViewModel> UserPostCollection
		{
			get => _UserPostCollection;
			set => SetProperty(ref _UserPostCollection, value);
		}
		private ObservableCollection<BasePostViewModel> _UserLikePostCollection = new ObservableCollection<BasePostViewModel>();
		public ObservableCollection<BasePostViewModel> UserLikePostCollection
		{
			get => _UserLikePostCollection;
			set => SetProperty(ref _UserLikePostCollection, value);
		}

		#endregion
		public ObservableCollection<object> Items { get; set; }
		public bool IsAutoAnimationRunning { get; set; }
		public bool IsUserInteractionRunning { get; set; }

		private ICommand _navigationToChangeProfileCommand;
		public ICommand NavigationToChangeProfileCommand => _navigationToChangeProfileCommand ??= SingleExecutionCommand.FromFunc(OnNavigationToChangeProfileAsync);

		private ICommand _userBlockCommand;
		public ICommand UserBlockCommand => _userBlockCommand ??= SingleExecutionCommand.FromFunc(OnUserBlockAsync);

		private ICommand _userMuteCommand;
		public ICommand UserMuteCommand => _userMuteCommand ??= SingleExecutionCommand.FromFunc(OnUserMuteAsync);

		private ICommand _showMenuCommand;
		public ICommand ShowMenuCommand => _showMenuCommand ??= SingleExecutionCommand.FromFunc(OnShowMenuAsync);

		private ICommand _backNavigationCommand;
		public ICommand BackNavigationCommand => _backNavigationCommand ??= SingleExecutionCommand.FromFunc(OnBackNavigationAsync);
	
		private ICommand _refreshCommand;
		public ICommand RefreshCommand => _refreshCommand ??= SingleExecutionCommand.FromFunc(OnRefreshAsync, delayMillisec: 0);

        public ICommand PanPositionChangedCommand { get; }
		public ICommand RemoveCurrentItemCommand { get; }
		public ICommand GoToLastCommand { get; }


		#endregion

		#region -- Overrides --

		public override async void OnNavigatedTo(INavigationParameters parameters)
		{
			var user = await _userService.GetUserAsync(_authorizationService.GetCurrentUserId);

			User UserModel;
			if (parameters.TryGetValue(nameof(UserModel), out UserModel))
			{
				CurrentProfile = UserModel;
				CurrentUser = user.Result;
				UserName = CurrentProfile.Name;
				UserMail = CurrentProfile.Email;
				UserBackGround = CurrentProfile.ProfileBackgroundImagePath;
				UserImagePath = CurrentProfile.ProfileImagePath;
				IsYourProfile = (CurrentProfile.Id == CurrentUser?.Id || CurrentUser == null);
				IsAlianProfile = CurrentUser.Id != CurrentProfile.Id;

				UpdatePage();

			}
			else
			{
				if (user.TrackingResult)
				{
					CurrentProfile = user.Result;
					UserName = CurrentProfile.Name;
					UserMail = CurrentProfile.Email;
					UserBackGround = CurrentProfile.ProfileBackgroundImagePath;
					UserImagePath = CurrentProfile.ProfileImagePath;
				}
			}

			await UpdateCollecitonAsync();
		}

		#endregion

		#region -- Private helpers -- 
		private async Task OnRefreshAsync()
		{
			await UpdateCollecitonAsync();
		}

		private async Task OnBackNavigationAsync()
		{
			await NavigationService.GoBackAsync();
		}

		private async Task OnUserBlockAsync()
		{
			var newlist = CurrentUser.BlockedUserIds.ToList();

			if (BlockedMessage == Resources.Strings.Block)
			{
				newlist.Add(CurrentProfile.Id);
			}
			else
			{
				newlist.Remove(CurrentProfile.Id);
			}

			CurrentUser.BlockedUserIds = newlist;

			await _userService.UpdateUserAsync(CurrentUser);

			UpdatePage();
		}

		private async Task OnUserMuteAsync()
		{
			var newlist = CurrentUser.MutedUserIds.ToList();

			if (MutedMessage == Resources.Strings.Mute)
			{
				newlist.Add(CurrentProfile.Id);
			}
			else
			{
				newlist.Remove(CurrentProfile.Id);
			}

			CurrentUser.MutedUserIds = newlist;

			await _userService.UpdateUserAsync(CurrentUser);

			UpdatePage();
		}

		private void UpdatePage()
		{
			MutedMessage = CurrentUser.MutedUserIds.Contains(CurrentProfile.Id) ? Resources.Strings.Unmute : Resources.Strings.Mute;
			BlockedMessage = CurrentUser.BlockedUserIds.Contains(CurrentProfile.Id) ? Resources.Strings.RemoveFromBlacklist : Resources.Strings.Block;

			ProfileInfo = BlockedMessage != Resources.Strings.Block ? Resources.Strings.ProfileInBlacklist : MutedMessage != Resources.Strings.Mute ? Resources.Strings.Muted : string.Empty;
			if (string.IsNullOrEmpty(ProfileInfo))
			{
				IsAlianProfile = false;
			}
			else
			{
				IsAlianProfile = true;
			}
		}

		private async Task OnShowMenuAsync()
		{
			if (IsYourProfile)
			{
				IsShowMenu = true;
			}
			else
			{
				IsShowAlianMenu = true;
			}

			await Task.Delay(3000);

			IsShowMenu = false;
			IsShowAlianMenu = false;
		}

		private async Task OnNavigationToChangeProfileAsync()
		{
			await NavigationService.NavigateAsync(nameof(ChangeProfileView));
		}

		private async Task<AOResult> UpdateCollecitonAsync()
		{
			var result = new AOResult();

			try
			{
				IsRefreshing = true;

				UserPostCollection.Clear();
				UserLikePostCollection.Clear();

				var posts = await _postManager.GetPostsAsync();

				posts.Result.Where(u => CurrentProfile.Id == u.UserModel.Id).ToList().ForEach(UserPostCollection.Add);
				posts.Result.Where(u => u.PostModel.LikedUserIds.Contains(CurrentProfile.Id)).ToList().ForEach(UserLikePostCollection.Add);

				IsRefreshing = false;

				result.SetSuccess();
			}
			catch (Exception ex)
			{
				result.SetError($"{nameof(UpdateCollecitonAsync)}", "Something went wrong", ex);
			}

			return result;
		}

		#endregion
	}
}

