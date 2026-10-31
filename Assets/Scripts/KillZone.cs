using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.GetComponent<PlayerController>() != null){
            GameManager.Instance.GameOver(GameManager.GameOverType.Died);
        }
    }
}
