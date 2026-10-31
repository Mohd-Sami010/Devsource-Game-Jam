using UnityEngine;

public class FinishZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.GetComponent<PlayerController>() != null){
            GameManager.Instance.GameOver(GameManager.GameOverType.Won);
        }
    }
}
