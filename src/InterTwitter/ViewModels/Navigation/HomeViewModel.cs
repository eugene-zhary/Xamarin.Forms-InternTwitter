using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services;
using Prism.Events;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.ViewModels.Navigation
{
    public class HomeViewModel : BaseTabViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IMockManager _mockManager;
        
        public HomeViewModel(INavigationService navigation, IEventAggregator eventAggregator, IMockManager mockManager) : base(navigation)
        {
            _eventAggregator = eventAggregator;
            _mockManager = mockManager;

            IconPath = "ic_home_gray.png";
            PostCollection = new ObservableCollection<Post>();
        }

        #region -- Public region --

        public ObservableCollection<Post> PostCollection { get; set; }

        private Thickness _Margin;
        public Thickness Margin
        {
            get => _Margin;
            set => SetProperty(ref _Margin, value);
        }

        public ICommand PicProfileTapGestureRecognizer => new Command(OnPicProfileTapGestureRecognizer);

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            UpdateColleciton();
        }

        public override void OnAppearing()
        {
            IconPath = "ic_home_blue.png";
        }

        public override void OnDisappearing()
        {
            IconPath = "ic_home_gray.png";
        }

        #endregion

        #region -- Private helpers --

        private void UpdateColleciton()
        {
            PostCollection.Clear();
            var mockPosts = _mockManager.GetPosts();
            mockPosts.ToList().ForEach(PostCollection.Add);
        }

        private void OnPicProfileTapGestureRecognizer()
        {
            _eventAggregator.GetEvent<MenuVisibilityChangedEvent>().Publish(true);
        }

        #endregion
    }
}
