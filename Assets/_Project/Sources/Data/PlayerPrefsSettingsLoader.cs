using Newtonsoft.Json;
using UnityEngine;

public class PlayerPrefsSettingsLoader : ISettingLoader
{
    private readonly string _settingsKey;

    public PlayerPrefsSettingsLoader(string settingsKey = "settings")
    {
        _settingsKey = settingsKey;
    }

    public SettingsData Load()
    {
        if (PlayerPrefs.HasKey(_settingsKey) == false)
        {
            var settings = new SettingsData();
            Save(settings);
            return settings;
        }

        var json = PlayerPrefs.GetString(_settingsKey);
        return JsonConvert.DeserializeObject<SettingsData>(json);
    }

    public void Save(SettingsData data)
    {
        var json = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString(_settingsKey, json);
        PlayerPrefs.Save();
    }
}