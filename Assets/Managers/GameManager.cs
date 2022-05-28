using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool _gameIsPaused;
    
    private float _masterVolume = 0.8f;
    private float _sfxVolume = 0.8f;
    private float _musicVolume = 0.8f;

    public static float MasterVolume
    {
        get { return Instance._masterVolume; }
        private set { Instance._masterVolume = value; }
    }

    public static float SfxVolume
    {
        get { return Instance._sfxVolume; }
        private set { Instance._sfxVolume = value; }
    }

    public static float MusicVolume
    {
        get { return Instance._musicVolume; }
        private set { Instance._musicVolume = value; }
    }

    public static bool GameIsPaused
    {
        get { return Instance._gameIsPaused; }
        private set { Instance._gameIsPaused = value; }
    }
    
    public void ChangeScene(string path)
    {
        SceneManager.LoadSceneAsync(path, LoadSceneMode.Single);
    }

    public static void PauseGame()
    {
        
        GameIsPaused = true;
        Time.timeScale = 0;
    }

    public static void ResumeGame()
    {
        GameIsPaused = false;
        Time.timeScale = 1;
    }
    
    public static void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    public static void SetMasterVolume(float value)
    {
        MasterVolume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public static void SetSfxVolume(float value)
    {
        SfxVolume = value;
        PlayerPrefs.SetFloat("SfxVolume", value);
    }

    public static void SetMusicVolume(float value)
    {
        MusicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
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
}
