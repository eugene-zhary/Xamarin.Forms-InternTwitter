using InterTwitter.Services.Permission;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace InterTwitter.Droid.Services
{
    public class PermissionService : IPermissionService
    {
        #region -- IPermissionService implementation --

        public async Task<PermissionStatus> RequestPermissionAsync<T>() where T : Permissions.BasePermission, new()
        {
            var status = await CheckPermissionAsync<T>();

            if(status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<T>();
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