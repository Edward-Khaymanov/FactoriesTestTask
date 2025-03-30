using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : ILocatorService
{
    private AudioMixer _mixer;
    private AudioSource _musicAudioSource;
    private List<AudioClip> _musicClips;

    public void Init(AudioMixer mixer, AudioSource musicSource, List<AudioClip> musicClips)
    {
        _mixer = mixer;
        _musicAudioSource = musicSource;
        _musicClips = musicClips;
    }

    public void PlayMusic()
    {
        _musicAudioSource.Stop();
        _musicAudioSource.clip = _musicClips[0];
        _musicAudioSource.Play();
    }

    public void SetMasterVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        _mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }
}