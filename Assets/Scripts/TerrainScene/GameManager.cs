using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private GameStateEnum gameState;

    private void Awake()
    {
        Instance = this;
        UpdateGameState(GameStateEnum.start);
    }

    public void UpdateGameState(GameStateEnum gameStateEnum)
    {
        gameState = gameStateEnum;
    }

    void Update()
    {
        switch (gameState)
        {
            case GameStateEnum.start:
                BoardManager.Instance.SetupBoard();
                UpdateGameState(GameStateEnum.progress);
                break;
            case GameStateEnum.progress:
                break;
            case GameStateEnum.end:
                SceneManager.LoadScene("TerrainScene");
                break;
        }

    }

    public enum GameStateEnum
    {
        start,
        progress, 
        end
    }
}
