using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Singleton<PlayerManager> {
    [SerializeField] GameObject playerPrefab;

    public Transform spawnPoint;
    [HideInInspector] public Player playerInstance;
    [HideInInspector] public PlayerInput playerInput;

    public void SpawnPlayer() {
        GameObject go = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        playerInstance = go.GetComponent<Player>();
        playerInput = go.GetComponent<PlayerInput>();
    }

    public void TakeDamage(int damage) {
        if (playerInstance != null) {
            playerInstance.playerProperties.UpdateHealth(-damage);
        }
    }

    public void HealPlayer(int healAmount) {
        if (playerInstance != null) {
            playerInstance.playerProperties.UpdateHealth(healAmount);
        }
    }

    public int GetMana() {
        if (playerInstance != null) {
            return playerInstance.playerProperties.mana;
        }
        return 0;
    }

    public void SetInputActionMap(string actionMapName) {
        if (playerInstance != null) {
            if (playerInput != null) {
                playerInput.SwitchCurrentActionMap(actionMapName);
            }
        }
    }
}
