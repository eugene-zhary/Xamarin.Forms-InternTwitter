using Xamarin.Essentials;

namespace InterTwitter.Services.Settings
{
    public class SettingsManager : ISettingsManager
    {
        #region -- ISettingsManager implementation --

        public int RememberedUserId
        {
            get => Preferences.Get(nameof(RememberedUserId), default(int));
            set => Preferences.Set(nameof(RememberedUserId), value);
        }

        #endregion
    }
}
