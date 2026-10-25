using UnityEngine;

public class Ability : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private enum AbilityType { 
        Jump,
        DoubleJump,
        Dash,
    }
    [SerializeField] private AbilityType abilityType = AbilityType.Jump;

    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.GetComponent<PlayerController>() != null){

            switch (abilityType){
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
            Destroy(gameObject);
        }
    }
}
