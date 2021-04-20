using Xamarin.Essentials;

namespace InterTwitter.Services
{
    public class SettingsManager : ISettingsManager
    {
        public int CurrentUserId
        {
            get => Preferences.Get(nameof(CurrentUserId), 1);
            set => Preferences.Set(nameof(CurrentUserId), value);
        }
    }
}
