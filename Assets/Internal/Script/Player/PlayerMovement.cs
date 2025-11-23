using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [Header("Movement")]
    float moveSpeed = 5f;
    public float wallCheckDistance = 1.1f;
    float moveInput;

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

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();

        if (player != null) {
            moveSpeed = player.playerProperties.speed;
            jumpForce = player.playerProperties.jumpForce;
        }
    }

    public void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>().x;//get x input

        FlipSprite();
    }

    public void OnJump(InputValue value) {
        if (value.isPressed && isGrounded)
            jumpPress = true;
    }

    private void FixedUpdate() {
        if (GameManager.Instance.IsGameOver || isKnockedback) { return; }

        CheckGrounded();
        Movement();
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
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            //Play walk SFX
            //if (player.playerProperties.MoveSound != SfxID.None) AudioManager.Instance.PlaySFX(player.playerProperties.MoveSound);
        }

    }

    void Jump() {
        
        if (jumpPress && rb.linearVelocity.y < 0.1f) {
            jumpPress = false;
            //rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            rb.AddForce(jumpForce * Vector3.up, ForceMode2D.Impulse);

            //Play Jump SFX
            if (player.playerProperties.JumpSound != SfxID.None) AudioManager.Instance.PlaySFX(player.playerProperties.JumpSound);
        }
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

    public void OnDamaged(Vector2 direction, float force, float duration)
    {
        if(PlayerManager.Instance.playerInstance.IsInvisible && !isKnockedback) return;
        
        StartCoroutine(KnockBack(direction, force, duration));
    }

    IEnumerator KnockBack(Vector2 direction, float knockForce, float knockDuration)
    {
        isKnockedback = true;

        rb.linearVelocity = Vector2.zero; //reset velocity
        rb.AddForce(direction * knockForce, ForceMode2D.Impulse);//knockback player based on direction
        
        yield return new WaitForSeconds(knockDuration);

        isKnockedback=false;
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
