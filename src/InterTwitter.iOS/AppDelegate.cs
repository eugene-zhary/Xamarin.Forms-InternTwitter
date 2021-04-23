using FFImageLoading.Forms.Platform;
using Foundation;
using Plugin.Media;
using Prism;
using Prism.Ioc;
using UIKit;
using Xamarin.Forms;

namespace InterTwitter.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        #region -- Overrides --

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            CachedImageRenderer.Init();

            LoadApplication(new App(new IosInitializer()));

            return base.FinishedLaunching(app, options);
        }

        #endregion

        public class IosInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
            }
        }
    }
}
