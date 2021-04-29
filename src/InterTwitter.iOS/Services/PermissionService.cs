using Foundation;
using InterTwitter.Services.Permission;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Essentials;

namespace InterTwitter.iOS.Services
{
    public class PermissionService : IPermissionService
    {
        #region -- IPermissionService implementation --

        public async Task<PermissionStatus> RequestPermissionAsync<T>() where T : Permissions.BasePermission, new()
        {
            var status = await CheckPermissionAsync<T>();

            //HACK for iOS 14
            if(status == PermissionStatus.Unknown)
            {
                Permissions.RequestAsync<T>();

                while((status = await Permissions.CheckStatusAsync<T>()) == PermissionStatus.Unknown)
                {
                    await Task.Delay(50);
                }
            }

            if(status == PermissionStatus.Denied)
            {
                var okCancelAlertController = UIAlertController.Create("Permission Denied", "Please change permission in settings", UIAlertControllerStyle.Alert);

                okCancelAlertController.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, alert => UIApplication.SharedApplication.OpenUrl(new NSUrl("app-settings:"))));
                okCancelAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

                var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;

                while(vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }

                vc.PresentViewController(okCancelAlertController, true, null);
            }

            return status;
        }

        #endregion

        #region -- Private helpers --

        private Task<PermissionStatus> CheckPermissionAsync<T>() where T : Permissions.BasePermission, new()
        {
            return Permissions.CheckStatusAsync<T>();
        }

        #endregion
    }
}