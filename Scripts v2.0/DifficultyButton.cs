using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    private Button button;

    public enum Difficulty
    {
        Easy, // 0
        Normal, // 1
        Hard // 2
    }
    public Difficulty difficultyLevel;

    private float easy = 1.5f;
    private float normal = 3.0f;
    private float hard = 5.0f;

    private GameManager gameManager;

    // Start is called before the first frame update
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void SetDifficulty()
    {
        gameManager.StartGame();

        // Reset the variables for each game
        SpawnManager.waveNumber = 0;
        TrollController.movementSpeed = SetSpeed(difficultyLevel);
        GameManager.isGameActive = true;

        gameManager.titleScreen.gameObject.SetActive(false);
    }

    private float SetSpeed(Difficulty difficultySelection)
    {
        if (difficultySelection == Difficulty.Easy)
        {
            return easy;
        }
        else if (difficultySelection == Difficulty.Normal)
        {
            return normal;
        }
        else
        {
            return hard;
        }
    }
}
