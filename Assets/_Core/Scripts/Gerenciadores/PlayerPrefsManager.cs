using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public static PlayerPrefsManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        FirstInitPlayerPrefs();
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                PlayerPrefs.DeleteAll();
            }
        }
    }

    public void SetFloatPlayerPrefs(string _playerPrefsName, float _value)
    {


        PlayerPrefs.SetFloat(_playerPrefsName, _value);
    }

    public void SetBoolPlayerPrefs(string _playerPrefsName, bool _value)
    {


        int value = _value ? 1 : 0;
        PlayerPrefs.SetInt(_playerPrefsName, value);
    }

    void FirstInitPlayerPrefs()
    {

        if (!PlayerPrefs.HasKey(CONST_PARAMS.PLAYERPREFS_MUSIC_VOLUME))
        {
            PlayerPrefs.SetFloat(CONST_PARAMS.PLAYERPREFS_MUSIC_VOLUME, .05f);
        }

        if (!PlayerPrefs.HasKey(CONST_PARAMS.PLAYERPREFS_AUDIO_VOLUME))
        {
            PlayerPrefs.SetFloat(CONST_PARAMS.PLAYERPREFS_AUDIO_VOLUME, .5f);
        }

        if (!PlayerPrefs.HasKey(CONST_PARAMS.PLAYERPREFS_MUSIC_ON_OFF))
        {
            PlayerPrefs.SetInt(CONST_PARAMS.PLAYERPREFS_MUSIC_ON_OFF, 1);
        }

        if (!PlayerPrefs.HasKey(CONST_PARAMS.PLAYERPREFS_AUDIO_ON_OFF))
        {
            PlayerPrefs.SetInt(CONST_PARAMS.PLAYERPREFS_AUDIO_ON_OFF, 1);
        }
    }
}