using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool IsGameOver { get; private set; }
    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        PlayerManager.Instance.SpawnPlayer();
    }

    public void EndGame()
    {
        IsGameOver = true;
    }
}
