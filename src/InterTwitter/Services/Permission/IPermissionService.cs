using System.Threading.Tasks;
using Xamarin.Essentials;

namespace InterTwitter.Services.Permission
{
    public interface IPermissionService
    {
        Task<PermissionStatus> RequestPermissionAsync<T>() where T : Permissions.BasePermission, new();
    }
}
