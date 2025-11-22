using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] GameObject playerPrefab;

    public Transform spawnPoint;

    protected override void Awake()
    {
        base.Awake();//singleton
    }

    public void SpawnPlayer()
    {
        Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
    }
}
