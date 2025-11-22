using UnityEngine;

public class GameManager : Singleton<GameManager> {
    public bool IsGameOver { get; private set; }
    protected override void Awake() {
        base.Awake();
        StartGame();
    }

    void StartGame() {
        PlayerManager.Instance.SpawnPlayer();
    }

    public void EndGame() {
        IsGameOver = true;
    }
}
