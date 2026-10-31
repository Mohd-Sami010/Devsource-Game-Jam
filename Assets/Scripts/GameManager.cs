using UnityEngine;
using System;

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
    }
    public void PlayerRespawned()
    {
        CurrentGameState = GameState.Playing;
        OnGameOver?.Invoke(this, CurrentGameState);
    }
}
