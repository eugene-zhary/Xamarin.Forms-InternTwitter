using System;
using InterTwitter.Services.Settings;

namespace InterTwitter.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ISettingsManager _settingsManager;

        public AuthorizationService(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        #region -- IAuthorizationService implementation --

        public bool IsAuthorized => _settingsManager.RememberedUserId != default(int);

        public int GetCurrentUserId => _settingsManager.RememberedUserId;

        public void Authorize(int id)
        {
            _settingsManager.RememberedUserId = id;
        }

        public void UnAuthorize()
        {
            _settingsManager.RememberedUserId = default(int);
        }

        #endregion
    }
}
