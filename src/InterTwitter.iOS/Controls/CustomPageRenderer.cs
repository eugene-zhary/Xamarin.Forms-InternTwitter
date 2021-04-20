using System;
using InterTwitter.iOS.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(CustomPageRenderer))]
namespace InterTwitter.iOS.Controls
{
 
        public class CustomPageRenderer : PageRenderer
        {
            protected override void OnElementChanged(VisualElementChangedEventArgs e)
            {
                base.OnElementChanged(e);

                OverrideUserInterfaceStyle = UIUserInterfaceStyle.Light;
            }
        }
    }
