using CoreGraphics;
using InterTwitter.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(InterTwitter.Controls.BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace InterTwitter.iOS.Renderers
{
    class BorderlessEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UIKit.UITextBorderStyle.None;

                Control.LeftView = new UIView(new CGRect(0, 0, 0, 0));
                Control.LeftViewMode = UITextFieldViewMode.Always;
                Control.RightView = new UIView(new CGRect(0, 0, 0, 0));
                Control.RightViewMode = UITextFieldViewMode.Always;
            }
        }
    }
}
