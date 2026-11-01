using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Playing, Paused, GameOver }
    public GameState CurrentGameState { get; private set; } = GameState.Playing;


    public enum GameOverType
    {
        Died,
        Won
    }
    public event EventHandler<GameOverType> OnGameOver;

    private void Awake()
    {
        Instance = this;
    }
    public void GameOver(GameOverType gameOverType)
    {
        CurrentGameState = GameState.GameOver;
        OnGameOver?.Invoke(this, gameOverType);

        if (gameOverType == GameOverType.Died)
        {
            Invoke(nameof(Respawn), .5f);
            Debug.Log("Player has died. Respawning...");
        }
    }
    private void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void PlayNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
