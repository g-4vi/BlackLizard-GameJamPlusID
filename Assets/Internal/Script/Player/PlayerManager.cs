using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
public class PlayerManager : Singleton<PlayerManager> {
    [SerializeField] GameObject playerPrefab;

    public Transform spawnPoint;
    [HideInInspector] public Player playerInstance;

    public void SpawnPlayer() {
        GameObject go = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        playerInstance = go.GetComponent<Player>();
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
}
