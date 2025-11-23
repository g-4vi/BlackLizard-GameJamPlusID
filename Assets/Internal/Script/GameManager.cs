using UnityEngine;

public class GameManager : Singleton<GameManager> {

    [Header("UI References")]
    public GameObject pauseMenuUI;
    public GameObject gameOverUI;

    public bool IsGameOver { get; private set; }
    protected override void Awake() {
        base.Awake();
        StartGame();
    }

    void StartGame() {
        PlayerManager.Instance.SpawnPlayer();
        PlayerManager.Instance.SetInputActionMap("Player");
    }

    public void PauseGame() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        PlayerManager.Instance.SetInputActionMap("UI");
    }

    public void ResumeGame() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        PlayerManager.Instance.SetInputActionMap("Player");
    }

    public void EndGame() {
        Time.timeScale = 0f;
        PlayerManager.Instance.SetInputActionMap("UI");
        IsGameOver = true;
        gameOverUI.SetActive(true);
    }

    public void BackToMainMenu(string mainMenuSceneName) {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuSceneName);
    }
}
