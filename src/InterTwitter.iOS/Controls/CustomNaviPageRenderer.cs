using System;
using InterTwitter.iOS.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNaviPageRenderer))]
namespace InterTwitter.iOS.Controls
{

        public class CustomNaviPageRenderer : NavigationRenderer
        {
            protected override void OnElementChanged(VisualElementChangedEventArgs e)
            {
                base.OnElementChanged(e);

                OverrideUserInterfaceStyle = UIUserInterfaceStyle.Light;
            }
        }
    }
