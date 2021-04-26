using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials;
using Xamarin.Forms;
using AndroidX.AppCompat.App;
using FFImageLoading.Forms.Platform;
using Octane.Xamarin.Forms.VideoPlayer.Android;
using PanCardView.Droid;
using Android.Content;

namespace InterTwitter.Droid
{
    [Activity(Label = "@string/ApplicationName", Icon = "@mipmap/launcher_foreground", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        #region -- Overrides --

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);


            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);

            CardsViewRenderer.Preserve();
            CachedImageRenderer.Init(true);
            FormsVideoPlayer.Init();
            Xamarin.MediaGallery.Platform.Init(this, savedInstanceState);

            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;

            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            if(Xamarin.MediaGallery.Platform.CheckCanProcessResult(requestCode, resultCode, intent))
            {
                Xamarin.MediaGallery.Platform.OnActivityResult(requestCode, resultCode, intent);
            }

            base.OnActivityResult(requestCode, resultCode, intent);
        }

        #endregion

        public class AndroidInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
            }
        }

    }
}