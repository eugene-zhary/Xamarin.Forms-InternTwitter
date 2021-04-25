using System.Threading.Tasks;

namespace InterTwitter.Services.Permission
{
    public interface IPermissionManager
    {
        public Task<bool> RequestStoragePermissionAsync();
    }
}
