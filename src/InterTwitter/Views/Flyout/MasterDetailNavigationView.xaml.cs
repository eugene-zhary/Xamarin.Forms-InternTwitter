using Prism;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace InterTwitter.Views.Flyout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailNavigationView : MasterDetailPage
    {
        public MasterDetailNavigationView()
        {
            InitializeComponent();

            PrismApplicationBase.Current.On<Xamarin.Forms.PlatformConfiguration.Android>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);

        }
    }
}