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

        private double OldScrollParameter = 0;
        public ICommand ScrolledCommand => SingleExecutionCommand.FromFunc<double>(OnScrolledCommand, delayMillisec: 200);
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
    }
}
