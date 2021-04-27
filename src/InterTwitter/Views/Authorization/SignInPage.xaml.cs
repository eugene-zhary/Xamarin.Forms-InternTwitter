using Prism;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace InterTwitter.Views
{
    public partial class SignInPage : BaseContentPage
    {
        public SignInPage()
        {
            InitializeComponent();

            PrismApplicationBase.Current.On<Xamarin.Forms.PlatformConfiguration.Android>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }
    }
}
