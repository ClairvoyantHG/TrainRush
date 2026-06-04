using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    WaitToStart, 
    Playing,    
    GameOver     
}

public class GameManager : SingletonBase<GameManager>
{
    private static bool skipTitleAndRestart = false;

    public GameState CurrentState { get; private set; } = GameState.WaitToStart;
    public int CurrentScore { get; private set; } = 0;

    protected override void Awake()
    {
        base.Awake();
        Time.timeScale = 0f;
        CurrentState = GameState.WaitToStart;
        CurrentScore = 0;
    }

    private void Start()
    {
        if (UIManager.Instance == null) return;

        if (skipTitleAndRestart)
        {
            skipTitleAndRestart = false; 
            UIManager.Instance.OpenUI(UIRootType.VeryFront, UIType.CountdownUI);
        }
        else
        {
            UIManager.Instance.OpenUI(UIRootType.Main, UIType.TitleUI);
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        CurrentState = GameState.Playing;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.OpenUI(UIRootType.Content, UIType.ScoreUI);
        }
    }

    public void TriggerGameOver()
    {
        Time.timeScale = 0f;
        CurrentState = GameState.GameOver;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.CloseUI(UIType.ScoreUI);
            UIManager.Instance.OpenUI(UIRootType.Popup, UIType.GameOverUI);
        }

    }

    public void UpdateScoreByZPosition(float playerZ)
    {
        if (CurrentState != GameState.Playing) return;

        int calculatedScore = Mathf.FloorToInt(playerZ);

        if (calculatedScore > CurrentScore)
        {
            CurrentScore = calculatedScore;
        }
    }

    public void RestartGame()
    {
        skipTitleAndRestart = true; 
        ReloadCurrentScene();      
    }

    public void GoToTitle()
    {
        skipTitleAndRestart = false; 
        ReloadCurrentScene();        
    }

    private void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}