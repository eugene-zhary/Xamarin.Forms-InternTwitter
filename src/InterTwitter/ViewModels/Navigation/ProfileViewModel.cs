using InterTwitter.Helpers;
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
    public class ProfileViewModel : BaseViewModel
    {
        public ProfileViewModel(INavigationService navigationService) : base(navigationService)
        {
			Items = new ObservableCollection<object>
			{
				new { Source = CreateSource(), Ind = _imageCount++, Color = Color.Red, Title = "Posts" },
				new { Source = CreateSource(), Ind = _imageCount++, Color = Color.Green, Title = "Likes" },
			};

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

		private ICommand _navigationToChangeProfileCommand;
		public ICommand NavigationToChangeProfileCommand => _navigationToChangeProfileCommand ??= SingleExecutionCommand.FromFunc(OnNavigationToChangeProfileCommand);

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
	}
}

