using InterTwitter.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InterTwitter.Views.Flyout
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutTabbedView : CustomTabbedPage
    {
        public FlyoutTabbedView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}