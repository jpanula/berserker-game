using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UnityEvent GameOverEvent;
    public UnityEvent GoingBerserkEvent;
    public UnityEvent ResurrectionEvent;
    public UnityEvent EnemyKilledEvent;

    [SerializeField] private AudioSource clickAudioSource;

    [SerializeField] private AudioMixer audioMixer;

    private bool _gameIsPaused;
    private bool _playerIsBerserk = false;
    private int _totalEnemiesKilled;
    private bool _tutorialShown;
    
    private float _masterVolume = 0.7f;
    private float _sfxVolume = 0.7f;
    private float _musicVolume = 0.7f;

    public static bool EnemiesActive { get; set; }
    
    public static bool TutorialShown
    {
        get { return Instance._tutorialShown; }
        set { Instance._tutorialShown = value; }
    }
    
    public static bool PlayerIsBerserk
    {
        get { return Instance._playerIsBerserk; }
        set { Instance._playerIsBerserk = value; }
    }
    
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

    public static int TotalEnemiesKilled
    {
        get { return Instance._totalEnemiesKilled; }
        private set { Instance._totalEnemiesKilled = value; }
    }
    
    public void ChangeScene(string path)
    {
        ResetGame();
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

    public static void PlayClick()
    {
        Instance.clickAudioSource.Play();
    }
    
    public static void SetMasterVolume(float value)
    {
        MasterVolume = value;
        Instance.audioMixer.SetFloat("MasterVolume", Mathf.Log(MasterVolume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    public static void SetSfxVolume(float value)
    {
        SfxVolume = value;
        Instance.audioMixer.SetFloat("SfxVolume", Mathf.Log(SfxVolume) * 20);
        PlayerPrefs.SetFloat("SfxVolume", value);
    }

    public static void SetMusicVolume(float value)
    {
        MusicVolume = value;
        Instance.audioMixer.SetFloat("MusicVolume", Mathf.Log(MusicVolume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    private void OnGameOver()
    {
        PlayerIsBerserk = false;
        PauseGame();
    }
    
    private void OnEnemyKill()
    {
        TotalEnemiesKilled++;
    }

    private void ResetGame()
    {
        TotalEnemiesKilled = 0;
        PlayerIsBerserk = false;
        EnemiesActive = true;
        ResumeGame();
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
        
        EnemyKilledEvent.AddListener(OnEnemyKill);
        GameOverEvent.AddListener(OnGameOver);
        EnemiesActive = true;
    }

    private void Start()
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log(MasterVolume) * 20);
        audioMixer.SetFloat("MusicVolume", Mathf.Log(MusicVolume) * 20);
        audioMixer.SetFloat("SfxVolume", Mathf.Log(SfxVolume) * 20);
    }
}
