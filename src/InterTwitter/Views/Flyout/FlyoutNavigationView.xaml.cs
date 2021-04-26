using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InterTwitter.Views.Flyout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutNavigationView : MasterDetailPage
    {
        public FlyoutNavigationView()
        {
            InitializeComponent();
        }
    }
}