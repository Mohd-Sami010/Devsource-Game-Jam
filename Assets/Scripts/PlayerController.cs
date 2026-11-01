using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
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
    public event System.Action OnAbilityUnlocked;

    private void Awake(){
        Instance = this;
    }
    private void Start(){
        playerRb = GetComponent<Rigidbody2D>();
        GameManager.Instance.OnGameOver += HandleGameOver;
    }
    private void HandleGameOver(object sender, GameManager.GameOverType gameOverType)
    {
        playerRb.velocity = Vector2.zero;
        playerRb.gravityScale = 0;
    }

    private void Update(){
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Playing || !isJumpUnlocked){
            jumpBufferRemaining = 0;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            jumpBufferRemaining = jumpBufferTime;
        } else {
            jumpBufferRemaining = Mathf.Max(0, jumpBufferRemaining - Time.deltaTime);
        }
    }

    private void FixedUpdate(){
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.Playing){
            playerRb.velocity = Vector2.zero;
            return;
        }
        if (transform.position.y < -10){
            GameManager.Instance.GameOver(GameManager.GameOverType.Died);
            return;
        }
        dashCooldownRemaining = Mathf.Max(0, dashCooldownRemaining - Time.fixedDeltaTime);

        bool isGrounded = IsGrounded();
        if (isGrounded){
            if (!wasGrounded){
                jumpCount = 0;
                CameraController.Instance?.ShakeWeak();
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

        if (jumpBufferRemaining > 0 && isJumpUnlocked){
            bool jumped = false;
            if (jumpCount == 0 && (isGrounded || coyoteTimeRemaining > 0)){
                playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
                jumpCount = 1;
                coyoteTimeRemaining = 0;
                jumped = true;
            } else if (isDoubleJumpUnlocked && jumpCount < 2){
                playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
                jumpCount++;
                jumped = true;
            }

            if (jumped){
                jumpBufferRemaining = 0;
                CameraController.Instance?.ShakeWeak();
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
        OnAbilityUnlocked?.Invoke();
    }
    public void UnlockMoveRight(){
        isMoveRightUnlocked = true;
        OnAbilityUnlocked?.Invoke();
    }
    public void UnlockJump(){
        isJumpUnlocked = true;
        OnAbilityUnlocked?.Invoke();
    }
    public void UnlockDoubleJump(){
        isDoubleJumpUnlocked = true;
        OnAbilityUnlocked?.Invoke();
    }
    public void UnlockDash(){
        isDashUnlocked = true;
        OnAbilityUnlocked?.Invoke();
    }
    private bool IsGrounded(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, layerMask: LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }
    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }

    public bool IsMoveLeftUnlocked => isMoveLeftUnlocked;
    public bool IsMoveRightUnlocked => isMoveRightUnlocked;
    public bool IsJumpUnlocked => isJumpUnlocked;
    public bool IsDoubleJumpUnlocked => isDoubleJumpUnlocked;
    public bool IsDashUnlocked => isDashUnlocked;
}
