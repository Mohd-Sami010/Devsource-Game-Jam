using UnityEngine;

public class Ability : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private SpriteRenderer abilityColorSpriteRenderer;
    [SerializeField] private GameObject abilityUnlockEffectPrefab;

    private enum AbilityType {
        MoveLeft,
        MoveRight, 
        Jump,
        DoubleJump,
        Dash,
    }
    [SerializeField] private AbilityType abilityType = AbilityType.Jump;

    private void Start(){
        playerController = PlayerController.Instance;
    }
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.GetComponent<PlayerController>() != null){
            switch (abilityType){
                case AbilityType.MoveLeft:
                    playerController.UnlockMoveLeft();
                    break;
                case AbilityType.MoveRight:
                    playerController.UnlockMoveRight();
                    break;
                case AbilityType.Jump:
                    playerController.UnlockJump();
                    break;
                case AbilityType.DoubleJump:
                    playerController.UnlockDoubleJump();
                    break;
                case AbilityType.Dash:
                    playerController.UnlockDash();
                    break;
            }
            GameObject effect = Instantiate(abilityUnlockEffectPrefab, transform.position, Quaternion.identity);
            TMPro.TextMeshPro textMesh = effect.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>();
            textMesh.color = abilityColorSpriteRenderer.color;
            textMesh.text = abilityType.ToString() + " Unlocked!";
            SoundManager.Instance.PlayAbilityPickupSound();
            
            Destroy(effect, 2f);
            Destroy(gameObject);
        }
    }
}
