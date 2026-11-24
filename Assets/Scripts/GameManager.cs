using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] int targetScore = 10, lives = 3;
    [SerializeField] float respawnDelay = 0.6f, spawnProtection = 1.5f;
    [SerializeField] Health player;
    [SerializeField] Transform respawnPoint;

    // Purane optional fields (agar kisi purane level mein lage hon to null error nahi aayega)
    [SerializeField] TMP_Text scoreText, livesText;
    [SerializeField] GameObject winPanel, losePanel;

    [SerializeField] float timeLimit = 0f;
    [SerializeField] TMP_Text timerText;

    int score; float timeLeft; bool ended;

    void Awake() => Instance = this;

    void Start()
    {
        timeLeft = timeLimit;
        UpdateUI();
    }

    void Update()
    {
        if (timeLimit <= 0f || ended) return;
        timeLeft -= Time.deltaTime;
        if (timerText) timerText.text = "Time: " + Mathf.CeilToInt(Mathf.Max(timeLeft, 0f));
        if (timeLeft <= 0f) Lose();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
        if (score >= targetScore) Win();
    }

    public void UpdateUI()
    {
        int hp = player != null ? Mathf.Max(player.Current, 0) : 0;
        int maxHp = player != null ? player.Max : 3;

        // Naya HUDController update karein
        if (HUDController.Instance != null)
        {
            HUDController.Instance.UpdateScore(score);
            HUDController.Instance.UpdateLives(lives);
            HUDController.Instance.UpdateHealth(hp, maxHp);
        }

        // Purane text references agar hon to unhein bhi safe rakhein
        if (scoreText) scoreText.text = $"Score: {score} / {targetScore}";
        if (livesText) livesText.text = $"Lives: {lives}   Health: {hp} / {maxHp}";
    }

    public void PlayerDied()
    {
        lives--;
        UpdateUI();
        if (lives <= 0) Lose();
        else StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        if (player != null && respawnPoint != null)
        {
            Rigidbody body = player.GetComponent<Rigidbody>();
            body.position = respawnPoint.position;
            body.linearVelocity = Vector3.zero;
            player.Restore(spawnProtection);
        }
    }

    void Win()
    {
        if (ended) return;
        ended = true;
        if (HUDController.Instance != null)
            HUDController.Instance.ShowWin();
        else if (winPanel)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    void Lose()
    {
        if (ended) return;
        ended = true;
        if (HUDController.Instance != null)
            HUDController.Instance.ShowGameOver();
        else if (losePanel)
        {
            losePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void Restart() => LevelLoader.RestartCurrentLevel();
    public void NextLevel() => LevelLoader.LoadNextLevel();
}