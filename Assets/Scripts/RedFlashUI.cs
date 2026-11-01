using UnityEngine;

public class RedFlashUI : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnGameOver += (_, gameOverType) => {
        if (gameOverType == GameManager.GameOverType.Died) gameObject.SetActive(true);
        };

        gameObject.SetActive(false);
    }
}
