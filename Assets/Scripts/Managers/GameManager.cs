using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MAIN,
    PLAY,
    PAUSE,
    GAMEOVER,
}

public class GameManager : Singleton<GameManager>
{
    //[SerializeField] private PlayerController pc;
    [SerializeField] private MenuManager mm;

    public int Score { get; set; }
    public float ElapsedTime { get; set; }

    public event Action<int> OnScoreChanged;
    public event Action<float> OnTimerUpdated;

    public event Action OnGameStateChanged;
    private GameState _currentGameState;

    public GameState CurrentGameState
    {
        get => _currentGameState;
        set
        {
            if (_currentGameState != value)
            {
                _currentGameState = value;
                OnGameStateChanged?.Invoke();
            }
        }
    }

    #region Unity Methods
    private void Update()
    {
        if (CurrentGameState == GameState.PLAY)
        {
            ElapsedTime += Time.deltaTime;
            OnTimerUpdated?.Invoke(ElapsedTime);
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;        
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    public void ChangeGameState(GameState newState)
    {
        if (newState == GameState.PAUSE)
        {
            Time.timeScale = 0; // Pauses the game
        }
        else if (_currentGameState == GameState.PAUSE && newState == GameState.PLAY)
        {
            Time.timeScale = 1; // Resumes the game
        }

        _currentGameState = newState;
        Debug.Log("Current Game State: " + _currentGameState);
        OnGameStateChanged?.Invoke();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TitleScene")
        {
            ChangeGameState(GameState.MAIN);
            Time.timeScale = 1;
        }
        else if (scene.name == "PlayScene")
        {
            //pc = FindObjectOfType<PlayerController>();
            mm = FindObjectOfType<MenuManager>();

            ChangeGameState(GameState.PLAY);
            ResetScore();
            ResetTimer();
            Time.timeScale = 1;
        }
    }

    public void RestartGame()
    {
        ResetScore();
        ResetTimer();
        LoadPlayScene();
    }

    public void LoadPlayScene()
    {        
        SceneManager.LoadScene("PlayScene");
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void GameOver()
    {
        ChangeGameState(GameState.GAMEOVER);
        Time.timeScale = 0;

        CheckForHighScore();

        OnGameStateChanged?.Invoke();
    }

    public void AddScore(int points)
    {
        Score += points;
        OnScoreChanged?.Invoke(Score);
    }

    private void CheckForHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (Score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", Score);
            PlayerPrefs.Save();

            mm.UpdateHighScoreDisplay(Score);
        }
    }

    public void ResetScore()
    {
        Score = 0;
        OnScoreChanged?.Invoke(Score);
    }

    public void ResetTimer()
    {
        ElapsedTime = 0;
    }
}