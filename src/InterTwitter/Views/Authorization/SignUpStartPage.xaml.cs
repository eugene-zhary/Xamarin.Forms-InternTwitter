using Prism;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace InterTwitter.Views
{
    public partial class SignUpStartPage : BaseContentPage
    {
        public SignUpStartPage()
        {
            InitializeComponent();

            PrismApplicationBase.Current.On<Xamarin.Forms.PlatformConfiguration.Android>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }
    }
}
