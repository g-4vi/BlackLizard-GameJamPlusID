using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour {
    public PlayerProperties playerProperties;
    public LayerMask obstacleLayer;

    public bool IsInvisible { get; set; }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (obstacleLayer == (obstacleLayer | (1 << collision.gameObject.layer)) && !IsInvisible)//not invisible & is obstacle
        {
            //playerProperties.UpdateHealth(-1);

            /*if (playerProperties.health <= 0)//Game over
            {
                playerProperties.health = 0;
                GameManager.Instance.EndGame();
                return;
            }*/
            IsInvisible = true;
            StartCoroutine(StartInvisibleTimerCountdown());
        }
    }

    IEnumerator StartInvisibleTimerCountdown() {
        float invisibleTimer = playerProperties.invisiblePeriod;

        while (invisibleTimer > 0) {
            invisibleTimer -= Time.deltaTime;
            yield return null;
        }

        IsInvisible = false;
    }

/*#if UNITY_EDITOR
    [ContextMenu("Decrease Health by 1")]
    void DecreaseHealth() {
        playerProperties.UpdateHealth(-1);
    }
#endif*/
}
