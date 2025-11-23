using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour {
    public PlayerProperties playerProperties;
    public LayerMask obstacleLayer;

    public bool IsInvisible { get; set; }

    [HideInInspector] public Animator anim;
    public int MoveHash { get; set; }
    public int JumpHash { get; set; }
    public int IsDamagedHash {  get; set; }
    public int AttackHash {  get; set; }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        MoveHash = Animator.StringToHash("move");
        JumpHash = Animator.StringToHash("jump");
        IsDamagedHash = Animator.StringToHash("isDamaged");
        AttackHash = Animator.StringToHash("attack");
    }

    public void TriggerInvisibility()
    {
        if (!IsInvisible)//not invisible & is obstacle
        {
            Debug.Log("Invisible trigger");
            IsInvisible = true;

            //Hurt animation
            anim.SetBool(IsDamagedHash, true);

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
        anim.SetBool(IsDamagedHash, false);
        Debug.Log("Damage off");
    }

    void OnDestroy() {
        playerProperties.onHealthChanged = null;
        playerProperties.onManaChanged = null;
    }

/*#if UNITY_EDITOR
    [ContextMenu("Decrease Health by 1")]
    void DecreaseHealth() {
        playerProperties.UpdateHealth(-1);
    }
#endif*/
}
