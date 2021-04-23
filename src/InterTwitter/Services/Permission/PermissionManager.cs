using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InterTwitter.Services.Permission
{

    public class PermissionManager : IPermissionManager
    {
        public async Task<bool> RequestStoragePermissionAsync()
        {
            var status = await RequestPermissionAsync<StoragePermission>();
            return (status == PermissionStatus.Granted);
        }
        private async Task<PermissionStatus> RequestPermissionAsync<T>() where T : BasePermission, new()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<T>();
            if (status == PermissionStatus.Denied) 
            { 
                CrossPermissions.Current.OpenAppSettings(); 
                status = await CrossPermissions.Current.CheckPermissionStatusAsync<T>(); 
            }
            if (status != PermissionStatus.Granted) 
            {
                status = await CrossPermissions.Current.RequestPermissionAsync<T>(); 
            }
            return status;
        }

    }
}
