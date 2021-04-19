using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services;
using InterTwitter.ViewModels.Posts;
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
        private readonly IPostManager _postManager;
        
        public HomeViewModel(INavigationService navigation, IEventAggregator eventAggregator, IPostManager postManager) : base(navigation)
        {
            _eventAggregator = eventAggregator;
            _postManager = postManager;

            IconPath = "ic_home_gray.png";
            PostCollection = new ObservableCollection<BasePostViewModel>();
        }

        #region -- Public region --

        public ObservableCollection<BasePostViewModel> PostCollection { get; set; }

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

            var posts = _postManager.GetPosts();
            posts.ToList().ForEach(PostCollection.Add);
        }

        private void OnPicProfileTapGestureRecognizer()
        {
            _eventAggregator.GetEvent<MenuVisibilityChangedEvent>().Publish(true);
        }

        #endregion
    }
}
