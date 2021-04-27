namespace InterTwitter.Services.Settings
{
    public interface ISettingsManager
    {
        int RememberedUserId { get; set; }
        void ClearLocalSettings();
    }
}
