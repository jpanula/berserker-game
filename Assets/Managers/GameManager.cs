using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private float _masterVolume = 0.8f;
    private float _sfxVolume = 0.8f;
    private float _musicVolume = 0.8f;

    public float MasterVolume
    {
        get { return _masterVolume; }
        private set { _masterVolume = value; }
    }

    public float SfxVolume
    {
        get { return _sfxVolume; }
        private set { _sfxVolume = value; }
    }

    public float MusicVolume
    {
        get { return _musicVolume; }
        private set { _musicVolume = value; }
    }
    
    public void ChangeScene(string path)
    {
        SceneManager.LoadSceneAsync(path, LoadSceneMode.Single);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            MasterVolume = PlayerPrefs.GetFloat("MasterVolume");
        }
        
        if (PlayerPrefs.HasKey("SfxVolume"))
        {
            SfxVolume = PlayerPrefs.GetFloat("SfxVolume");
        }
        
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
        }
    }

    public void SetMasterVolume(float value)
    {
        MasterVolume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public void SetSfxVolume(float value)
    {
        SfxVolume = value;
        PlayerPrefs.SetFloat("SfxVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
}
