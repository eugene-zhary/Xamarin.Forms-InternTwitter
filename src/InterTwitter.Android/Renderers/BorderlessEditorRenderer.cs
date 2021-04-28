using Android.Content;
using InterTwitter.Controls;
using InterTwitter.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderlessEditor), typeof(BorderlessEditorRenderer))]
namespace InterTwitter.Droid.Renderers
{
    public class BorderlessEditorRenderer :EditorRenderer
    {
        public BorderlessEditorRenderer(Context context): base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if(e.OldElement == null)
            {
                Control.Background = null;

                Control.SetPadding(0, 0, 0, 0);
            }
        }

    }
}