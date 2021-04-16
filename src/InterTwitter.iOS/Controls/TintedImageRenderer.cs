using InterTwitter.Controls;
using InterTwitter.iOS.Controls;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TintedImage), typeof(TintedImageRenderer))]
namespace InterTwitter.iOS.Controls
{
    public class TintedImageRenderer : ImageRenderer
    {

        #region -- Overrides --

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            SetTint();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            switch(e.PropertyName)
            {
                case nameof(TintedImage.TintColorProperty):
                case nameof(TintedImage.SourceProperty):
                    SetTint();
                    break;
            }
        }

        #endregion

        #region -- Private helpers --

        private void SetTint()
        {
            if(Control?.Image!= null && Element != null)
            {
                var tintImg = Element as TintedImage;

                Control.Image = Control.Image.ImageWithRenderingMode(UIKit.UIImageRenderingMode.AlwaysTemplate);
                Control.TintColor = tintImg.TintColor.ToUIColor();
            }
        }

        #endregion
    }
}