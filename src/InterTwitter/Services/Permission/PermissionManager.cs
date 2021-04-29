using InterTwitter.Helpers;
using System.Threading.Tasks;

//namespace InterTwitter.Services.Permission
//{
//    public class PermissionManager : IPermissionManager
//    {
//        #region -- IPermissionManager implementation --

//        public async Task<bool> RequestStoragePermissionAsync()
//        {
//            var request = await RequestPermissionAsync<StoragePermission>();

//            return request.Result;
//        }

//        public AOResult GoToAppSettings()
//        {
//            var result = new AOResult();

//            try
//            {
//                CrossPermissions.Current.OpenAppSettings();
//                result.SetSuccess();
//            }
//            catch(System.Exception ex)
//            {
//                result.SetError($"{nameof(GoToAppSettings): exception}", "Something went wrong", ex);
//            }

//            return result;
//        }

//        #endregion

//        #region -- Private helpers --

//        private async Task<AOResult<bool>> RequestPermissionAsync<T>() where T : BasePermission, new()
//        {
//            var result = new AOResult<bool>();


//            try
//            {
//                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();

//                if(status != PermissionStatus.Granted)
//                {
//                    status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
//                }

//                if(status == PermissionStatus.Granted)
//                {
//                    result.SetSuccess(true);
//                }
//                else if(status == PermissionStatus.Denied)
//                {
//                    result.SetFailure(false);
//                }
//            }
//            catch(System.Exception ex)
//            {
//                result.SetError($"{nameof(RequestPermissionAsync)} : exeption", "Something went wrong", ex);
//            }

//            return result;
//        }

//        #endregion
//    }
//}