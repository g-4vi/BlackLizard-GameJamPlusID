using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [Header("Movement")]
    float moveSpeed = 5f;
    public float wallCheckDistance = 1.1f;
    float moveInput;
    float yMoveInput;

    [Header("Jump")]
    float jumpForce = 10f;
    public float groundCheckDistance = 1.1f;
    public float groundCheckDiff = 0.3f;
    public LayerMask groundLayer;
    bool jumpPress;

    Player player;
    Rigidbody2D rb;
    bool isGrounded = true;
    bool isKnockedback = false;
    float defaultGravityScale;
    public bool limitMovement = false;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();

        if (rb != null) {
            defaultGravityScale = rb.gravityScale;
        }

        if (player != null) {
            moveSpeed = player.playerProperties.speed;
            jumpForce = player.playerProperties.jumpForce;
        }
    }

    public void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>().x;//get x input
        yMoveInput = value.Get<Vector2>().y;//get y input

        FlipSprite();
    }

    public void OnJump(InputValue value) {
        if (value.isPressed && isGrounded)
            jumpPress = true;
    }

    public void OnCancel(InputValue value) {
        if (value.isPressed) {
            if (GameManager.Instance.IsGameOver) return;

            if (Time.timeScale == 1f) {
                GameManager.Instance.PauseGame();
            } else {
                GameManager.Instance.ResumeGame();
            }
        }
    }

    private void FixedUpdate() {
        if (GameManager.Instance && GameManager.Instance.IsGameOver || isKnockedback || Time.timeScale == 0f) { return; }

        Movement();

        if (limitMovement) {
            VerticalMovement();
            return;
        }

        CheckGrounded();
        if (isGrounded) {
            Jump();
        }
    }

    void FlipSprite() {
        if (moveInput != 0) {
            transform.localScale = new Vector3(
                Mathf.Sign(moveInput) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }

    void Movement() {
        //Control Player Horizontal Movement
        if (TouchingWall()) {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        } else {
            float targetMoveSpeed = moveInput * moveSpeed / Time.timeScale;  // adjust movement based on time scale
            rb.linearVelocity = new Vector2(targetMoveSpeed, rb.linearVelocity.y);
        }

        //Float/Walk animation
        player.anim.SetFloat(player.MoveHash, Mathf.Abs(rb.linearVelocity.x));
    }

    void Jump() {

        if (jumpPress && rb.linearVelocity.y < 0.1f) {
            jumpPress = false;
            //rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            float compensatedJumpForce = jumpForce / Time.timeScale;
            rb.AddForce(compensatedJumpForce * Vector3.up, ForceMode2D.Impulse);

            //Jump Animation
            player.anim.SetTrigger(player.JumpHash);

            //Play Jump SFX
            if (player.playerProperties.JumpSound != SfxID.None) AudioManager.Instance.PlaySFX(player.playerProperties.JumpSound);
        }
    }

    void VerticalMovement() {
        //Control Player Vertical Movement
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, yMoveInput * moveSpeed);

        //Play walk SFX
        //if (player.playerProperties.MoveSound != SfxID.None) AudioManager.Instance.PlaySFX(player.playerProperties.MoveSound);

        //Float/Walk animation
        player.anim.SetFloat(player.MoveHash, Mathf.Abs(rb.linearVelocity.y));
    }

    void CheckGrounded() {
        bool midRay = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        bool leftRay = Physics2D.Raycast(transform.position +
            new Vector3(-groundCheckDiff, 0, 0), Vector2.down, groundCheckDistance, groundLayer);
        bool rightRay = Physics2D.Raycast(transform.position +
            new Vector3(groundCheckDiff, 0, 0), Vector2.down, groundCheckDistance, groundLayer);

        if (midRay || leftRay || rightRay)//collision with ground
        {
            isGrounded = true;
        } else {
            isGrounded = false;
        }
    }

    bool TouchingWall() {
        bool touchingWall = Physics2D.Raycast(transform.position, new Vector3(Mathf.Sign(transform.localScale.x), 0, 0), wallCheckDistance, groundLayer);

        return touchingWall;
    }

    public void OnDamaged(Vector2 direction, float force, float duration) {
        if (isKnockedback) return;
        StartCoroutine(KnockBack(direction, force, duration));
    }

    IEnumerator KnockBack(Vector2 direction, float knockForce, float knockDuration) {
        isKnockedback = true;

        rb.linearVelocity = Vector2.zero; //reset velocity
        rb.AddForce(direction * knockForce, ForceMode2D.Impulse);//knockback player based on direction

        yield return new WaitForSeconds(knockDuration);

        isKnockedback = false;
    }

    public void LimitMovement(bool isLimited) {
        limitMovement = isLimited;
        if (limitMovement)
            rb.gravityScale = 0;
        else
            rb.gravityScale = defaultGravityScale;
    }

    public void ComputeGravityByTimeScale(float targetTimeScale) {
        if (targetTimeScale <= 0.001f || rb == null) return;
        rb.gravityScale = defaultGravityScale * (1f / (targetTimeScale * targetTimeScale));
    }

    public void RescaleVelocityY(float multiplier) {
        if (rb == null) return;
        rb.linearVelocityY *= multiplier;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        //ground rays
        Gizmos.DrawLine(transform.position + new Vector3(-groundCheckDiff, 0, 0),
            transform.position + new Vector3(-groundCheckDiff, 0, 0) + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(transform.position + new Vector3(groundCheckDiff, 0, 0),
            transform.position + new Vector3(groundCheckDiff, 0, 0) + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);//midray


        Gizmos.DrawLine(transform.position, transform.position + new Vector3(Mathf.Sign(transform.localScale.x), 0, 0) * wallCheckDistance);
    }
}
