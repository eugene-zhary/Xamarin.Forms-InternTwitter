using InterTwitter.Helpers;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.ViewModels.Navigation
{
    public class HomeViewModel : BaseTabViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public HomeViewModel(INavigationService navigation, IEventAggregator eventAggregator) : base(navigation)
        {
            _eventAggregator = eventAggregator;
            IconPath = "ic_home_gray.png";
        }

        #region -- Overrides --

        public override void OnAppearing()
        {
            IconPath = "ic_home_blue.png";
        }

        public override void OnDisappearing()
        {
            IconPath = "ic_home_gray.png";
        }

        #endregion

        private Thickness _Margin;
        public Thickness Margin
        {
            get => _Margin;
            set => SetProperty(ref _Margin, value);
        }

        public ICommand PicProfileTapGestureRecognizer => new Command<object>(OnPicProfileTapGestureRecognizer);
        private void OnPicProfileTapGestureRecognizer(object obj)
        {
            _eventAggregator.GetEvent<MenuVisibilityChangedEvent>().Publish(true);
        }
    }
}
