using System;
using System.Drawing;
using System.Linq;
using Foundation;
using InterTwitter.Controls;
using InterTwitter.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MovableFrame), typeof(MovableFrameRenderer))]
namespace InterTwitter.iOS.Renderers
{
    public class MovableFrameRenderer : FrameRenderer
    {
        private NSObject _keyboardShowObserver;
        private NSObject _keyboardHideObserver;

        #region -- Overrides --

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                RegisterForKeyboardNotifications();
            }

            if (e.OldElement != null)
            {
                UnregisterForKeyboardNotifications();
            }
        }

        #endregion

        #region -- Private Helpers --

        private void RegisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver == null)
            {
                _keyboardShowObserver = UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShow);
            }

            if (_keyboardHideObserver == null)
            {
                _keyboardHideObserver = UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHide);
            }
        }

        private void UnregisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver != null)
            {
                _keyboardShowObserver.Dispose();
                _keyboardShowObserver = null;
            }

            if (_keyboardHideObserver != null)
            {
                _keyboardHideObserver.Dispose();
                _keyboardHideObserver = null;
            }
        }

        private async void OnKeyboardShow(object sender, UIKeyboardEventArgs args)
        {
            NSValue result = (NSValue)args.Notification.UserInfo.ObjectForKey(new NSString(UIKeyboard.FrameEndUserInfoKey));

            SizeF keyboardSize = result.RectangleFValue.Size;
            
            UIWindow window = UIApplication.SharedApplication.Windows.FirstOrDefault();
            nfloat bottomPadding = window.SafeAreaInsets.Bottom;

            nfloat finalOffset = keyboardSize.Height - bottomPadding; 

            if (Element != null)
            {
                await Element.TranslateTo(0, -finalOffset, easing: Easing.Linear);
            }
        }

        private async void OnKeyboardHide(object sender, UIKeyboardEventArgs args)
        {
            if (Element != null)
            {
                await Element.TranslateTo(0, 0);
            }
        }

        #endregion
    }
}