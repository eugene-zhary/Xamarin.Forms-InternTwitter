using Prism;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace InterTwitter.Views.PostPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPostPage : BaseContentPage
    {
        public AddPostPage()
        {
            InitializeComponent();

            PrismApplicationBase.Current.On<Xamarin.Forms.PlatformConfiguration.Android>()
                .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }
    }
}