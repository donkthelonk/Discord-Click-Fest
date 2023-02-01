using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI livesText;
    public bool isGameActive;
    public Button restartButton;
    public GameObject titleScreen;
    public int lives;
    public GameObject pauseScreen;
    public AudioClip targetExplosionSound;

    private float spawnRate = 1.0f;
    private int score;
    private bool paused;
    private AudioSource targetAudio;

    // Start is called before the first frame update
    void Start()
    {
        targetAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            ChangePaused();
        }
    }

    IEnumerator SpawnTarget()
    {
        while(isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void UpdateLives()
    {
        lives--;
        livesText.text = "Lives: " + lives;

        // Check if Game Over
        if (lives <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        // Deactivate Game state
        isGameActive = false;

        // Display Game Over screen elements
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty)
    {
        // Activate Game state
        isGameActive = true;

        // Start Spawning Targets
        StartCoroutine(SpawnTarget());

        // Initialize score 
        score = 0;
        UpdateScore(0);

        // Initialize lives text
        livesText.text = "Lives: " + lives;

        // Hide Title Screen elements
        titleScreen.gameObject.SetActive(false);

        // Set Spawn Rate based on Difficulty
        spawnRate /= difficulty;
    }

    void ChangePaused()
    {
        if(!paused)
        {
            // Flip bool
            paused = true;

            // Display pause panel
            pauseScreen.SetActive(true);

            // Stop time
            Time.timeScale = 0;
        }
        else
        {
            // Flip bool
            paused = false;

            // Hide pause panel
            pauseScreen.SetActive(false);

            // Start time
            Time.timeScale = 1;

        }
    }

    // Public method to play target explosion sound
    public void PlayTargetExplosionSound()
    {
        targetAudio.PlayOneShot(targetExplosionSound, 1.0f);
    }
}
