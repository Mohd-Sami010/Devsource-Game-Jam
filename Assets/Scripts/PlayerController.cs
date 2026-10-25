using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;

    [SerializeField] private float moveSpeed = 20;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float groundCheckDistance = 0.1f;

    private int jumpCount = 0;

    private bool isJumpUnlocked = false;
    private bool isDoubleJumpUnlocked = false;
    private bool isDashUnlocked = false;

    private void Start(){
        playerRb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate(){

        if (Input.GetKey(KeyCode.A)){
            playerRb.velocity = new Vector2(-moveSpeed, playerRb.velocity.y);
        } else if (Input.GetKey(KeyCode.D)){
            playerRb.velocity = new Vector2(moveSpeed, playerRb.velocity.y);
        } else {
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isJumpUnlocked){
            if (IsGrounded()){
                playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
                jumpCount = 1;
            } else if (isDoubleJumpUnlocked && jumpCount < 2){
                playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
                jumpCount++;
            }
        }
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
        // Implement dash logic here
    }
    private bool IsGrounded(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance);
        return hit.collider != null;
    }
}
