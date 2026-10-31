using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteUI : MonoBehaviour
{
    [SerializeField] private Button nextLevelButton;

    private void Start()
    {
        GameManager.Instance.OnGameOver += HandleGameOver;
        gameObject.SetActive(false);
    }
    private void HandleGameOver(object sender, GameManager.GameOverType gameOverType)
    {
        if (gameOverType == GameManager.GameOverType.Won)
        {
            gameObject.SetActive(true);
            nextLevelButton.onClick.AddListener(() => {
                GameManager.Instance.PlayNextLevel();
                nextLevelButton.onClick.RemoveAllListeners();
            });
        }
    }
}
