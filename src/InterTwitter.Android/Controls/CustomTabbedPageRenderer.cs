using Android.Content;
using Google.Android.Material.BottomNavigation;
using InterTwitter.Controls;
using InterTwitter.Droid.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(CustomTabbedPage), typeof(CustomTabbedPageRenderer))]
namespace InterTwitter.Droid.Controls
{
    public class CustomTabbedPageRenderer : TabbedPageRenderer
    {
        private TabbedPage _tabbedPage;
        private BottomNavigationView _bottomNavigationView;

        public CustomTabbedPageRenderer(Context context) : base(context)
        {

        }

        #region -- Overrides --

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                _tabbedPage = e.NewElement as CustomTabbedPage;
                _bottomNavigationView = (GetChildAt(0) as Android.Widget.RelativeLayout).GetChildAt(1) as BottomNavigationView;
                _bottomNavigationView.LabelVisibilityMode = LabelVisibilityMode.LabelVisibilityUnlabeled;
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (_bottomNavigationView != null)
            {
                _bottomNavigationView.ItemIconTintList = null;
            }
        }

        #endregion
    }
}