using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerProperties playerProperties;
    public LayerMask obstacleLayer;

    bool isInvisible;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (obstacleLayer == (obstacleLayer | (1 << collision.gameObject.layer)) && !isInvisible)//not invisible & is obstacle
        {
            playerProperties.UpdateHealth(-1);

            if(playerProperties.health <= 0)//Game over
            {
                GameManager.Instance.EndGame();
                return;
            }
            isInvisible = true;
            StartCoroutine(StartInvisibleTimerCountdown());
        }
    }

    IEnumerator StartInvisibleTimerCountdown()
    {
        float invisibleTimer = playerProperties.invisiblePeriod;

        while (invisibleTimer > 0)
        {
            invisibleTimer -= Time.deltaTime;
            yield return null;
        }

        isInvisible = false;
    }
}
