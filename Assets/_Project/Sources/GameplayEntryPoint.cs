using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameplayEntryPoint : MonoBehaviour
{
    [SerializeField] private ClickInputController _clickControllerTemplate;
    [SerializeField] private TouchInputController _touchControllerTemplate;
    [SerializeField] private Transform _characterSpawnPoint;
    [SerializeField] private Character _characterTemplate;
    [SerializeField] private PlayerCamera _playerCamera;
    [SerializeField] private FactoryView _factoryViewTemplate;
    [SerializeField] private StorageWindow _storageWindow;
    [SerializeField] private SettingsWindow _settingsWindow;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private List<AudioClip> _musicClips;
    [SerializeField] private List<FactoryObject> _factoryObjects;

    private List<IDisposable> _disposables;
    private IInputController _inputController;

    private void Awake()
    {
        _disposables = new List<IDisposable>();
        var saveLoadService = new DummySaveLoadService();
        var factoriesManager = new FactoriesManager();
        var audioManager = new AudioManager();
        var settingsLoader = new PlayerPrefsSettingsLoader();
        _inputController = GetInput();
        var character = GameObject.Instantiate(_characterTemplate, _characterSpawnPoint.position, _characterSpawnPoint.rotation);

        ServiceLocator.Initialize();
        ServiceLocator.Register<ISettingLoader>(settingsLoader);
        ServiceLocator.Register(factoriesManager);
        ServiceLocator.Register<ISaveLoadService>(saveLoadService);
        ServiceLocator.Register(_storageWindow);
        ServiceLocator.Register(audioManager);
        ServiceLocator.Register(_settingsWindow);

        saveLoadService.Init();
        factoriesManager.Init(_factoryViewTemplate, _factoryObjects);
        _inputController.Init(character, _playerCamera);
        audioManager.Init(_audioMixer, _musicSource, _musicClips);
        _storageWindow.Init();
        _disposables.Add(factoriesManager);
    }

    private void Start()
    {
        var settings = ServiceLocator.Get<ISettingLoader>().Load();
        ServiceLocator.Get<AudioManager>().SetMasterVolume(settings.MasterAudioVolumeNormalized);
        ServiceLocator.Get<AudioManager>().PlayMusic();
        ServiceLocator.Get<FactoriesManager>().StartProduce();
        _inputController.Enable();
    }

    private void OnDestroy()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
    }

    private IInputController GetInput()
    {
#if UNITY_ANDROID
        return GameObject.Instantiate(_touchControllerTemplate);
#endif
#if UNITY_WEBGL || UNITY_STANDALONE
        return GameObject.Instantiate(_clickControllerTemplate);
#endif
    }
}