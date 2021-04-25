using Prism;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace InterTwitter.Views
{
    public partial class SignUpEndPage : BaseContentPage
    {
        public SignUpEndPage()
        {
            InitializeComponent();

            PrismApplicationBase.Current.On<Xamarin.Forms.PlatformConfiguration.Android>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }
    }
}
