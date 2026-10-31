using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;

    [SerializeField] private float moveSpeed = 20;
    [SerializeField] private float dashSpeed = 30;
    [SerializeField] private float dashCooldown = 3;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;

    private int jumpCount = 0;
    private float dashCooldownRemaining = 0;
    private float coyoteTimeRemaining = 0;
    private float jumpBufferRemaining = 0;
    private bool wasGrounded = false;

    [Header("Ability Unlocks")]
    [SerializeField] private bool isMoveLeftUnlocked = true;
    [SerializeField] private bool isMoveRightUnlocked = true;
    [SerializeField] private bool isJumpUnlocked = false;
    [SerializeField] private bool isDoubleJumpUnlocked = false;
    [SerializeField] private bool isDashUnlocked = false;

    private Vector2 respawnPosition;

    private void Start(){
        playerRb = GetComponent<Rigidbody2D>();
        respawnPosition = transform.position;
        GameManager.Instance.OnGameOver += HandleGameOver;
    }
    private void HandleGameOver(object sender, GameManager.GameOverType gameOverType)
    {
        if (gameOverType == GameManager.GameOverType.Died)
        {
            playerRb.velocity = Vector2.zero;
            playerRb.gravityScale = 0;
        }
    }

    private void FixedUpdate(){
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Playing){
            playerRb.velocity = Vector2.zero;
            return;
        }
        dashCooldownRemaining = Mathf.Max(0, dashCooldownRemaining - Time.fixedDeltaTime);

        bool isGrounded = IsGrounded();
        if (isGrounded){
            if (!wasGrounded){
                jumpCount = 0;
            }
            coyoteTimeRemaining = coyoteTime;
        } else {
            coyoteTimeRemaining = Mathf.Max(0, coyoteTimeRemaining - Time.fixedDeltaTime);
        }
        wasGrounded = isGrounded;

        if (Input.GetKey(KeyCode.A) && isMoveLeftUnlocked){
            playerRb.velocity = new Vector2(-moveSpeed, playerRb.velocity.y);
        } else if (Input.GetKey(KeyCode.D) && isMoveRightUnlocked){
            playerRb.velocity = new Vector2(moveSpeed, playerRb.velocity.y);
        } else {
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isJumpUnlocked){
            jumpBufferRemaining = jumpBufferTime;
        } else {
            jumpBufferRemaining = Mathf.Max(0, jumpBufferRemaining - Time.fixedDeltaTime);
        }

        if (jumpBufferRemaining > 0 && isJumpUnlocked){
            if (coyoteTimeRemaining > 0){
                playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
                jumpCount = 1;
                coyoteTimeRemaining = 0;
                jumpBufferRemaining = 0;
            } else if (isDoubleJumpUnlocked && jumpCount < 2){
                playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
                jumpCount++;
                jumpBufferRemaining = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && isDashUnlocked && dashCooldownRemaining <= 0){
            if (Input.GetKey(KeyCode.A)){
                playerRb.AddForce(new Vector2(-dashSpeed, 0), ForceMode2D.Impulse);
                dashCooldownRemaining = dashCooldown;
            } else if (Input.GetKey(KeyCode.D)){
                playerRb.AddForce(new Vector2(dashSpeed, 0), ForceMode2D.Impulse);
                dashCooldownRemaining = dashCooldown;
            }
        }
    }
    public void UnlockMoveLeft(){
        isMoveLeftUnlocked = true;
        Debug.Log("Move Left ability unlocked!");
    }
    public void UnlockMoveRight(){
        isMoveRightUnlocked = true;
        Debug.Log("Move Right ability unlocked!");
    }
    public void UnlockJump(){
        isJumpUnlocked = true;
        Debug.Log("Jump ability unlocked!");
    }
    public void UnlockDoubleJump(){
        isDoubleJumpUnlocked = true;
        Debug.Log("Double Jump ability unlocked!");
    }
    public void UnlockDash(){
        isDashUnlocked = true;
        Debug.Log("Dash ability unlocked!");
    }
    private bool IsGrounded(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance);
        return hit.collider != null;
    }
}
