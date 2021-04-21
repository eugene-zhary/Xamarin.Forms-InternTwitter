using InterTwitter.Helpers;
using InterTwitter.Services.Authorization;
using InterTwitter.Services.UserService;
using InterTwitter.Views.Flyout;
using InterTwitter.Views.Navigation;
using PanCardView.Extensions;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.ViewModels.Navigation
{
	public class ProfileViewModel : BaseViewModel, INavigatedAware
	{
		public ProfileViewModel(INavigationService navigationService, IAuthorizationService AuthorizationService, IUserService userService) : base(navigationService)
		{
			Items = new ObservableCollection<object>
			{
				new { Source = CreateSource(), Ind = _imageCount++, Color = Color.Red, Title = "Posts" },
				new { Source = CreateSource(), Ind = _imageCount++, Color = Color.Green, Title = "Likes" },
			};
			_authorizationService = AuthorizationService;
            _userService = userService;

			PanPositionChangedCommand = new Command(v =>
			{
				if (IsAutoAnimationRunning || IsUserInteractionRunning)
				{
					return;
				}

				var index = CurrentIndex + (bool.Parse(v.ToString()) ? 1 : -1);
				if (index < 0 || index >= Items.Count)
				{
					return;
				}
				CurrentIndex = index;
			});

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
		}


		public ICommand BackNavigationCommand => SingleExecutionCommand.FromFunc(OnBackNavigationCommand);

		private async Task OnBackNavigationCommand()
		{
			await NavigationService.NavigateAsync($"/{nameof(FlyoutNavigationView)}");
		}
		private int _currentIndex;
		private int _imageCount = 1078;


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
		private bool _IsShowMenu;
		public bool IsShowMenu
		{
			get => _IsShowMenu;
			set => SetProperty(ref _IsShowMenu, value);
		}

		private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;

		private ICommand _navigationToChangeProfileCommand;
		public ICommand NavigationToChangeProfileCommand => _navigationToChangeProfileCommand ??= SingleExecutionCommand.FromFunc(OnNavigationToChangeProfileCommand);
		
		private ICommand _hideMenuCommand;
		public ICommand HideMenuCommand => _hideMenuCommand ??= SingleExecutionCommand.FromFunc(OnHideMenuCommand);
		
		private ICommand _showMenuCommand;
		public ICommand ShowMenuCommand => _showMenuCommand ??= SingleExecutionCommand.FromFunc(OnShowMenuCommand);

        private async Task OnShowMenuCommand()
        {
			IsShowMenu = true;

			await Task.Delay(3000);

			IsShowMenu = false;

		}

		private Task OnHideMenuCommand()
        {

			return Task.CompletedTask;
        }

        private async Task OnNavigationToChangeProfileCommand()
        {
			await NavigationService.NavigateAsync(nameof(ChangeProfileView));
        }

        public ICommand PanPositionChangedCommand { get; }

		public ICommand RemoveCurrentItemCommand { get; }

		public ICommand GoToLastCommand { get; }
		public int CurrentIndex
		{
			get => _currentIndex;
			set => SetProperty(ref _currentIndex, value);
		}
		public bool IsAutoAnimationRunning { get; set; }

		public bool IsUserInteractionRunning { get; set; }

		public ObservableCollection<object> Items { get; }

        private string CreateSource()
		{
			var source = $"https://picsum.photos/500/500?image={_imageCount}";
			return source;
		}

		public override async void OnNavigatedTo(INavigationParameters parameters)
		{
			var user = await _userService.GetUserAsync(_authorizationService.GetCurrentUserId);
            
			if (user.TrackingResult)
            {
				UserName = user.Result.Name;
				UserMail = user.Result.Email;
				UserBackGround = user.Result.ProfileBackgroundImagePath;
				UserImagePath = user.Result.ProfileImagePath;
			}
		}
    }
}

