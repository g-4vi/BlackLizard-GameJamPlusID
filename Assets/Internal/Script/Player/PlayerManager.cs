using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] GameObject playerPrefab;

    public Transform spawnPoint;
    public GameObject player;
    protected override void Awake()
    {
        base.Awake();//singleton
    }

    public void SpawnPlayer()
    {
       player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
    }
}
