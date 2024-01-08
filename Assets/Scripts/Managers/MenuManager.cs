using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private GameObject hud;

    [Header("HUD Elements")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Button hud_PauseButton;

    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;

    [Header("Main Menu Buttons")]
    [SerializeField] private Button mm_PlayButton;
    [SerializeField] private Button mm_SettingsButton;
    [SerializeField] private Button mm_QuitButton;

    [Header("Pause Menu Buttons")]
    [SerializeField] private Button pm_ResumeButton;
    [SerializeField] private Button pm_SettingsButton;
    [SerializeField] private Button pm_MainMenuButton;
    [SerializeField] private Button pm_QuitButton;

    [Header("GameOver Menu Buttons")]
    [SerializeField] private Button go_RestartButton;
    [SerializeField] private Button go_SettingsButton;
    [SerializeField] private Button go_MainMenuButton;
    [SerializeField] private Button go_QuitButton;

    [Header("Settings Menu Buttons, Sliders, Texts")]
    [SerializeField] private Button closeButton;

    [Header("Sliders")]
    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private Slider sfxVolSlider;
    [SerializeField] private Slider musicVolSlider;

    [Header("Texts")]
    [SerializeField] private TMP_Text masterVolText;
    [SerializeField] private TMP_Text sfxVolText;
    [SerializeField] private TMP_Text musicVolText;

    private GameObject lastActiveMenu;

    #region Unity Methods
    private void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreDisplay(highScore);

        SettingsManager.Instance.LoadVolumeSettings();
        InitializeButtons();
        InitializeSliders();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        GameManager.Instance.OnScoreChanged += UpdateScoreDisplay;
        GameManager.Instance.OnTimerUpdated += UpdateTimeDisplay;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        GameManager.Instance.OnScoreChanged -= UpdateScoreDisplay;
        GameManager.Instance.OnTimerUpdated -= UpdateTimeDisplay;
    }
    #endregion

    private void HandleGameStateChanged()
    {
        // Initialize menu based on game state
        switch (GameManager.Instance.CurrentGameState)
        {
            case GameState.MAIN:
                ActivateMainMenu();
                break;
            case GameState.PLAY:
                PlayGame();
                break;
            case GameState.PAUSE:
                TogglePause();
                break;
            case GameState.GAMEOVER:
                ActivateGameOverMenu();
                break;
            default:
                break;
        }
    }

    private void InitializeButtons()
    {
        hud_PauseButton.onClick.AddListener(TogglePause);

        mm_PlayButton.onClick.AddListener(PlayGame);
        mm_SettingsButton.onClick.AddListener(ActivateSettingsMenu);
        mm_QuitButton.onClick.AddListener(QuitGame);

        pm_ResumeButton.onClick.AddListener(ResumeGame);
        pm_SettingsButton.onClick.AddListener(ActivateSettingsMenu);
        pm_MainMenuButton.onClick.AddListener(GoToMainMenu);
        pm_QuitButton.onClick.AddListener(QuitGame);

        go_RestartButton.onClick.AddListener(RestartGame);
        go_SettingsButton.onClick.AddListener(ActivateSettingsMenu);
        go_MainMenuButton.onClick.AddListener(GoToMainMenu);
        go_QuitButton.onClick.AddListener(QuitGame);

        closeButton.onClick.AddListener(Close);
    }

    private void InitializeSliders()
    {
        // Load saved settings
        masterVolSlider.value = SettingsManager.Instance.MasterVolume;
        sfxVolSlider.value = SettingsManager.Instance.SFXVolume;
        musicVolSlider.value = SettingsManager.Instance.MusicVolume;

        // Initialize slider texts
        UpdateSliderText(masterVolSlider.value, masterVolText);
        UpdateSliderText(sfxVolSlider.value, sfxVolText);
        UpdateSliderText(musicVolSlider.value, musicVolText);

        // Subscribe to slider value changes
        masterVolSlider.onValueChanged.AddListener(value => SetVolume(value, masterVolText, "Master"));
        sfxVolSlider.onValueChanged.AddListener(value => SetVolume(value, sfxVolText, "SFX"));
        musicVolSlider.onValueChanged.AddListener(value => SetVolume(value, musicVolText, "Music"));
    }

    private void UpdateSliderText(float value, TMP_Text text)
    {
        text.text = (value * 100).ToString("0");
    }

    private void SetVolume(float value, TMP_Text text, string name)
    {
        UpdateSliderText(value, text);

        SettingsManager.Instance.SaveVolumeSettings(masterVolSlider.value, sfxVolSlider.value, musicVolSlider.value);

        AudioManager.Instance.InitializeMixer();
    }

    private void HideAllMenus()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        hud.SetActive(false);
    }

    private void ActivateMainMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    private void ActivateSettingsMenu()
    {
        if (mainMenu.activeSelf)
        {
            lastActiveMenu = mainMenu;
            ToggleMenu(mainMenu, settingsMenu);
        }
        else if (pauseMenu.activeSelf)
        {
            lastActiveMenu = pauseMenu;
            ToggleMenu(pauseMenu, settingsMenu);
        }
        else if (gameOverMenu.activeSelf)
        {
            lastActiveMenu = gameOverMenu;
            ToggleMenu(gameOverMenu, settingsMenu);
        }
    }

    private void Close()
    {
        if (settingsMenu.activeSelf && lastActiveMenu != null)
        {
            ToggleMenu(settingsMenu, lastActiveMenu);        
        }
    }

    public void TogglePause()
    {
        if (GameManager.Instance.CurrentGameState == GameState.PLAY)
        {
            GameManager.Instance.ChangeGameState(GameState.PAUSE);
            pauseMenu.SetActive(true); // Show pause menu
        }
    }

    private void ActivateGameOverMenu()
    {
        if (GameManager.Instance.CurrentGameState == GameState.GAMEOVER)
        {
            gameOverMenu.SetActive(true);
        }
    }

    private void PlayGame()
    {
        if (GameManager.Instance.CurrentGameState == GameState.PLAY)
        {
            HideAllMenus();
            hud.SetActive(true);
        }
        else if (GameManager.Instance.CurrentGameState == GameState.MAIN || GameManager.Instance.CurrentGameState == GameState.GAMEOVER)
        {
            GameManager.Instance.LoadPlayScene();
        }
    }

    private void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    public void ResumeGame()
    {
        GameManager.Instance.ChangeGameState(GameState.PLAY);
        pauseMenu.SetActive(false); // Hide pause menu
    }

    private void GoToMainMenu()
    {
        GameManager.Instance.LoadTitleScene();
    }

    private void ToggleMenu(GameObject off, GameObject on)
    {
        off.SetActive(false);
        on.SetActive(true);
    }

    private void UpdateScoreDisplay(int newScore)
    {
        scoreText.text = "SCORE\n" + newScore.ToString();
    }

    public void UpdateHighScoreDisplay(int highScore)
    {
        highScoreText.text = "HIGH SCORE\n" + highScore.ToString();
    }

    private void UpdateTimeDisplay(float elapsedTime)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
        timeText.text = "TIME\n" + string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    private void QuitGame()
    {
        // Quit game logic
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}