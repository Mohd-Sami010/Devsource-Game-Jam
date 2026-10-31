using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Playing, Paused, GameOver }
    public GameState CurrentGameState { get; private set; } = GameState.Playing;

    public event EventHandler<GameState> OnGameOver;

    public enum GameOverType
    {
        Died,
        Won
    }

    private void Awake()
    {
        Instance = this;
    }
    public void GameOver(GameOverType gameOverType)
    {
        CurrentGameState = GameState.GameOver;
        OnGameOver?.Invoke(this, CurrentGameState);

        if (gameOverType == GameOverType.Died)
        {
            Invoke(nameof(Respawn), 2f);
            Debug.Log("Player has died. Respawning...");
        }
    }
    private void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
