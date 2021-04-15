using Android.Content;
using Android.Graphics;
using InterTwitter.Controls;
using InterTwitter.Droid.Controls;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TintedImage), typeof(TintedImageRenderer))]
namespace InterTwitter.Droid.Controls
{
    public class TintedImageRenderer : ImageRenderer
    {
        public TintedImageRenderer(Context context) : base(context)
        {

        }

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
            if(Control != null && Element != null)
            {
                var tintImg = Element as TintedImage;

                var colorFilter = new PorterDuffColorFilter(tintImg.TintColor.ToAndroid(), PorterDuff.Mode.SrcIn);
                Control.SetColorFilter(colorFilter);
            }
        }

        #endregion
    }
}