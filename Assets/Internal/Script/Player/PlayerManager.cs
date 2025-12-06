using GameJamPlus;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Singleton<PlayerManager> {
    [SerializeField] GameObject playerPrefab;

    public Transform spawnPoint;
    [HideInInspector] public Player playerInstance;
    [HideInInspector] public PlayerProperties playerProperties;
    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public PlayerSkillController playerSkillController;

    public void SpawnPlayer() {
        GameObject go = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        playerInstance = go.GetComponent<Player>();
        playerInput = go.GetComponent<PlayerInput>();
        playerMovement = go.GetComponent<PlayerMovement>();
        playerSkillController = go.GetComponent<PlayerSkillController>();
        playerProperties = playerInstance.playerProperties ?? go.GetComponent<PlayerProperties>();
    }

    public void SpawnPlayerLimitMovement() {
        SpawnPlayer();
        SetInputActionMap("Player");

        playerInstance.GetComponent<PlayerMovement>().LimitMovement(true);
    }

    public void TakeDamage(int damage) {
        if (playerInstance != null) {
            playerInstance.playerProperties.UpdateHealth(-damage);

            playerInstance.TriggerInvisibility();
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
