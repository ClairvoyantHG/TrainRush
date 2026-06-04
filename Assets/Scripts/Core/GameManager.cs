using UnityEngine;
public enum GameState
{
    WaitToStart, 
    Playing,    
    GameOver     
}

public class GameManager : SingletonBase<GameManager>
{
    public GameState CurrentState { get; private set; } = GameState.WaitToStart;

    protected override void Awake()
    {
        base.Awake();
        Time.timeScale = 0f;
        CurrentState = GameState.WaitToStart;
    }

    private void Start()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.OpenUI(UIRootType.Main, UIType.TitleUI);
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        CurrentState = GameState.Playing;
        Debug.Log("[GameManager] 게임이 시작되었습니다!");
    }

    public void TriggerGameOver()
    {
        Time.timeScale = 0f;
        CurrentState = GameState.GameOver;
    }
}