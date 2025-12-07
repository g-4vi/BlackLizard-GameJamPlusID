using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    public PlayerProperties playerProperties;
    public LayerMask obstacleLayer;

    public bool IsInvisible { get; set; }

    [HideInInspector] public Animator anim;
    public int MoveHash { get; set; }
    public int JumpHash { get; set; }
    public int IsDamagedHash { get; set; }
    public int AttackHash { get; set; }

    public Action TriggerInteract;
    public bool canInteract;

    private void Awake() {
        anim = GetComponentInChildren<Animator>();
        MoveHash = Animator.StringToHash("move");
        JumpHash = Animator.StringToHash("jump");
        IsDamagedHash = Animator.StringToHash("isDamaged");
        AttackHash = Animator.StringToHash("attack");
    }

    public void OnInteract(InputValue value)
    {
        if(canInteract && value.isPressed)
        {
            TriggerInteract?.Invoke();
        }
    }

    public void TriggerInvisibility() {
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
            if (Time.timeScale == 0f) yield return null; // stop countdown when game is paused
            invisibleTimer -= Time.unscaledDeltaTime;
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
