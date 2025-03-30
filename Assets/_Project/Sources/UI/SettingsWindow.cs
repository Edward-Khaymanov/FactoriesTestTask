using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour, ILocatorService
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private TextMeshProUGUI _masterVolumeValueTextComponent;

    private SettingsData _currentSettings;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(Hide);
        _masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Hide);
        _masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
    }

    public void Show()
    {
        _currentSettings = ServiceLocator.Get<ISettingLoader>().Load();
        RenderSettings(_currentSettings);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        ServiceLocator.Get<ISettingLoader>().Save(_currentSettings);
        gameObject.SetActive(false);
    }

    private void RenderSettings(SettingsData settings)
    {
        _masterVolumeSlider.SetValueWithoutNotify(settings.MasterAudioVolumeNormalized);
        RenderMasterVolume(settings.MasterAudioVolumeNormalized);
    }

    private void RenderMasterVolume(float value)
    {
        _masterVolumeValueTextComponent.text = $"{Mathf.RoundToInt(value * 100)}%";
    }

    private void OnMasterVolumeChanged(float value)
    {
        _currentSettings.MasterAudioVolumeNormalized = value;
        RenderMasterVolume(value);
        ServiceLocator.Get<AudioManager>().SetMasterVolume(value);
    }
}