using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static int livesLeft;
    public static int livesFull = 5;

    public static int score;
    private Text scoreText;
    public int highScore;
    private Text highScoreText;
    public int level = 1;
    private Text levelText;

    public static bool wasShown = false;

    public GameObject[] hearts;
    public GameObject destructor;

    public static bool isGameActive = false;
    public GameObject titleScreen;

    public static bool isGameOver = false;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    private AudioSource livesAudio;
    public AudioClip gameOverSound;
    private bool isAudioReload = false;
    private float audioVolume = 1.0f;
    public Slider audioVolumeSlider;

    private GameObject pauseButton;
    private GameObject resumeButton;
    private TextMeshProUGUI pauseText;

    private bool isPaused = false;

    public static bool isDestructorAvailable = true;

    private void Awake()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // Start is called before the first frame update
    private void Start()
    {
        scoreText = GameObject.Find("Score Text").GetComponent<Text>();
        highScoreText = GameObject.Find("High Score Text").GetComponent<Text>();
        levelText = GameObject.Find("Level Text").GetComponent<Text>();

        livesAudio = GetComponent<AudioSource>();
        AudioListener.volume = audioVolumeSlider.value;

        pauseButton = GameObject.Find("Pause Button");
        resumeButton = GameObject.Find("Resume Button");
        pauseText = GameObject.Find("Pause Screen Text").GetComponent<TextMeshProUGUI>();

        StartGame();
    }

    // Update is called once per frame
    private void Update()
    {
        CapLives();
        CalculateHearts();
        CalculateDestructor();
        ShowScore();
        ShowLevel();
        GameOver();

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    public void StartGame()
    {
        score = 0;
        livesLeft = livesFull;
        isGameOver = false;

        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    private void CapLives()
    {
        // Make sure that the number of lives stays between 0 and max number of lives
        if (livesLeft <= 0)
        {
            livesLeft = 0;
            isGameActive = false;
            isGameOver = true;
        }

        if (livesLeft > livesFull)
        {
            livesLeft = livesFull;
        }
    }

    private void CalculateHearts()
    {
        // Based on the number of lives left, determine how many hearts to show on the screen
        for (int i = 0; i < hearts.Length; i++)
        {
            if (livesLeft > i)
            {
                hearts[i].GetComponent<Show>().show = true;
            }
            else
            {
                hearts[i].GetComponent<Show>().show = false;
            }
        }
    }

    private void CalculateDestructor()
    {
        // If destructor is available, show the icon on screen
        if (isDestructorAvailable)
        {
            destructor.GetComponent<Show>().show = true;
        }
        else
        {
            destructor.GetComponent<Show>().show = false;
        }
    }

    private void ShowScore()
    {
        scoreText.text = "Score: " + score.ToString();

        // Determine the highest score
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        highScoreText.text = "High Score: " + highScore.ToString();
    }

    private void ShowLevel()
    {
        if (score == 0 && isGameActive)
        {
            if (!wasShown)
            {
                level = 1;
                StartCoroutine(levelCoroutine());
            }
        }
        else if (SpawnManager.waveNumber == SpawnManager.levelTwoWave && isGameActive)
        {
            if (!wasShown)
            {
                level++;
                StartCoroutine(levelCoroutine());
            }
        }
        else if (SpawnManager.waveNumber == SpawnManager.levelThreeWave && isGameActive)
        {
            if (!wasShown)
            {
                level++;
                StartCoroutine(levelCoroutine());
            }
        }
        else
        {
            wasShown = false;
        }
    }

    public void GameOver()
    {
        if (isGameOver)
        {
            // Show the game over screen
            gameOverText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);

            if (isAudioReload == false)
            {
                livesAudio.PlayOneShot(gameOverSound, audioVolume);
                isAudioReload = true;
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void AudioVolumeChange()
    {
        AudioListener.volume = audioVolumeSlider.value;
    }

    public void PauseGame()
    {
        pauseText.enabled = true;

        pauseButton.SetActive(false);
        resumeButton.SetActive(true);

        Time.timeScale = 0.0f;

        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseText.enabled = false;

        resumeButton.SetActive(false);
        pauseButton.SetActive(true);

        Time.timeScale = 1.0f;

        isPaused = false;
    }

    public void ExitGame()
    {
        titleScreen.SetActive(false); // in case the game wasn't started

        isGameActive = false;
        isGameOver = true;

        GameOver();
    }

    IEnumerator levelCoroutine()
    {
        wasShown = true;

        levelText.text = "Level: " + level.ToString();

        levelText.enabled = true;

        isDestructorAvailable = true;

        var delay = new WaitForSeconds(2.0f);
        yield return delay;

        levelText.enabled = false;
    }
}

