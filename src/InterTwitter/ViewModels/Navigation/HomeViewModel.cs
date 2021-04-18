using InterTwitter.Enums;
using InterTwitter.Helpers;
using InterTwitter.Models;
using InterTwitter.Services;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.ViewModels.Navigation
{
    public class HomeViewModel : BaseTabViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IMockManager _mockManager;

        private double OldScrollParameter = 0;
        
        public HomeViewModel(INavigationService navigation, IEventAggregator eventAggregator, IMockManager mockManager) : base(navigation)
        {
            _eventAggregator = eventAggregator;
            _mockManager = mockManager;

            IconPath = "ic_home_gray.png";
            PostCollection = new ObservableCollection<Post>();
        }

        #region -- Public region --

        public ObservableCollection<Post> PostCollection { get; set; }

        private EScrollState _scrollState;
        public EScrollState ScrollState
        {
            get => _scrollState;
            set => SetProperty(ref _scrollState, value, nameof(ScrollState));
        }

        private Thickness _Margin;
        public Thickness Margin
        {
            get => _Margin;
            set => SetProperty(ref _Margin, value);
        }

        public ICommand PicProfileTapGestureRecognizer => new Command<object>(OnPicProfileTapGestureRecognizer);
        public ICommand ScrolledCommand => SingleExecutionCommand.FromFunc<double>(OnScrolledCommand, delayMillisec: 200);

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            // TODO: ask why it doesn't work
            //UpdateColleciton();
        }

        public override void OnAppearing()
        {
            IconPath = "ic_home_blue.png";
            UpdateColleciton();
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


        private void OnPicProfileTapGestureRecognizer(object obj)
        {
            _eventAggregator.GetEvent<MenuVisibilityChangedEvent>().Publish(true);
        }

        private async Task<double> OnScrolledCommand(double obj)
        {
            if (Math.Abs(obj - OldScrollParameter) > 48)
            {
                if (obj > OldScrollParameter)
                {
                    if (Margin.Top != -48)
                    {
                        for (int i = 0; i <= 48; i++)
                        {
                            Margin = new Thickness(0, -i, 0, 0);
                            await Task.Delay(1);
                        }
                    }
                }
                else
                {
                    if (Margin.Top != 0)
                    {
                        for (int i = 48; i >= 0; i--)
                        {
                            Margin = new Thickness(0, -i, 0, 0);
                            await Task.Delay(1);
                        }
                    }
                }
                OldScrollParameter = obj;

                Console.WriteLine(obj);
            }

            return await Task.FromResult(.1d);
        }

        #endregion
    }
}
