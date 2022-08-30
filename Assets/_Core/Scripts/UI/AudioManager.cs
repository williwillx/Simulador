using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum MusicStyle
{
    GeneralMusic,
    SimulationMusic
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static event Action onFinishAudio;

    [Header("Audio config")]
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource audioAudioSource;
    [SerializeField] AudioSource SFXAudioSource;
    [SerializeField] AudioClip[] backgroundSongs;
    [SerializeField] AudioClip[] simulationBackgroundSongs;
    [SerializeField] AudioClip sfxGlassBrick;

    MusicStyle currentMusicStlye;
    bool narrationAudioFinished = false;
    public float percentAudio = 0;
    private void Update()
    {
        if (!musicAudioSource.isPlaying && IsMusicOn())
        {
            PlayRandomSong();
        }
        if (audioAudioSource.clip != null)
        {
            percentAudio = (audioAudioSource.time / audioAudioSource.clip.length) * 100;
            if (percentAudio > 97f && narrationAudioFinished == false)
            {
                narrationAudioFinished = true;
                percentAudio = 0;
                onFinishAudio?.Invoke();
            }
        }
    }

    public void PlayRandomSong()
    {
        AudioClip[] _songs = currentMusicStlye == MusicStyle.GeneralMusic ? backgroundSongs : simulationBackgroundSongs;
        musicAudioSource.clip = _songs[UnityEngine.Random.Range(0, _songs.Length)];
        musicAudioSource.Play();
    }

    public void PlayRandomSong(MusicStyle _style)
    {
        AudioClip[] _songs = _style == MusicStyle.GeneralMusic ? backgroundSongs : simulationBackgroundSongs;
        musicAudioSource.clip = _songs[UnityEngine.Random.Range(0, _songs.Length)];
        musicAudioSource.Play();

        currentMusicStlye = _style;
    }

    public void FadeOutBackgroundTrack()
    {
        musicAudioSource.DOFade(PlayerPrefs.GetFloat(CONST_PARAMS.PLAYERPREFS_MUSIC_VOLUME), 2);
    }

    public void ToggleMusic(bool _value)
    {
        musicAudioSource.enabled = _value;
        PlayerPrefsManager.instance.SetBoolPlayerPrefs(CONST_PARAMS.PLAYERPREFS_MUSIC_ON_OFF, _value);
    }

    public void ToggleAudio(bool _value)
    {
        if (_value)
        {
            audioAudioSource.volume = PlayerPrefs.GetFloat(CONST_PARAMS.PLAYERPREFS_AUDIO_VOLUME);
        }
        else
        {
            audioAudioSource.volume = 0;
        }

        PlayerPrefsManager.instance.SetBoolPlayerPrefs(CONST_PARAMS.PLAYERPREFS_AUDIO_ON_OFF, _value);
    }

    public void SetMusicVolume(float _value)
    {
        float value = IsMusicOn() ? _value : 0;
        musicAudioSource.volume = _value;
        PlayerPrefsManager.instance.SetFloatPlayerPrefs(CONST_PARAMS.PLAYERPREFS_MUSIC_VOLUME, value);
    }

    public void SetAudioVolume(float _value)
    {
        float value = IsAudioOn() ? _value : 0;
        audioAudioSource.volume = value;
        PlayerPrefsManager.instance.SetFloatPlayerPrefs(CONST_PARAMS.PLAYERPREFS_AUDIO_VOLUME, value);
    }

    public bool IsMusicOn()
    {
        return PlayerPrefs.GetInt(CONST_PARAMS.PLAYERPREFS_MUSIC_ON_OFF) == 1;
    }

    public bool IsAudioOn()
    {
        return PlayerPrefs.GetInt(CONST_PARAMS.PLAYERPREFS_AUDIO_ON_OFF) == 1;
    }

    public void PlayAudio(AudioClip _clip)
    {
        audioAudioSource.clip = _clip;
        audioAudioSource.Play();
        narrationAudioFinished = false;
    }

    public void PlayAudioGlassBrick()
    {
        SFXAudioSource.clip = sfxGlassBrick;
        SFXAudioSource.Play();
        narrationAudioFinished = false;
    }

    public float GetCurrentAudioLength()
    {
        float value = audioAudioSource.clip == null ? 0 : audioAudioSource.clip.length;
        return value;
    }

    public void StopAudio()
    {
        audioAudioSource.Stop();
    }

    /// <summary>
    /// Toggle audio and return it's current state
    /// </summary>
    /// <returns></returns>
    public void ToggleAudio()
    {
        if (IsAudioPlaying())
        {
            audioAudioSource.Pause();
        }
        else
        {
            audioAudioSource.Play();
        }
    }

    public bool IsAudioPlaying()
    {
        return audioAudioSource.isPlaying;
    }

    public bool IsAudioFinished()
    {
        return narrationAudioFinished;
    }
}
