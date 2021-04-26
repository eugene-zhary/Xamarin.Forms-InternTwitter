using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InterTwitter.Services.Permission
{
    public interface IPermissionManager
    {
        public Task<bool> RequestStoragePermissionAsync();
    }
}
