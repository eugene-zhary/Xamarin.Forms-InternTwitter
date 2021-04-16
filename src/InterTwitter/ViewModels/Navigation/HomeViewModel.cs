using InterTwitter.Helpers;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InterTwitter.ViewModels.Navigation
{
    public class HomeViewModel : BaseTabViewModel
    {
        public HomeViewModel(INavigationService navigation) : base(navigation)
        {
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

        private bool _IsVisible = true;
        public bool IsVisible
        {
            get => _IsVisible;
            set => SetProperty(ref _IsVisible, value);
        }

        public ICommand ScrollSizeChanged => new Command<object>(OnScrollSizeChanged);
        private void OnScrollSizeChanged(object obj)
        {

            Console.WriteLine();
        }

        private double OldScrollParameter = 0;
        public ICommand ScrolledCommand => SingleExecutionCommand.FromFunc<double>(OnScrolledCommand, delayMillisec: 200);
        private Task<double> OnScrolledCommand(double obj)
        {
            if (Math.Abs(obj - OldScrollParameter) > 60)
            {
                if (obj > OldScrollParameter)
                {
                    IsVisible = false;
                }
                else
                {
                    IsVisible = true;
                }
                OldScrollParameter = obj;

                Console.WriteLine(obj);
            }

            return Task.FromResult(.1d);
        }
    }
}
