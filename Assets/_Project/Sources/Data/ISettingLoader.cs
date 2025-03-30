public interface ISettingLoader : ILocatorService
{
    public SettingsData Load();
    public void Save(SettingsData data);
}