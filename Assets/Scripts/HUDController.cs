using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDController : MonoBehaviour
{
    public static HUDController Instance { get; private set; }

    [Header("Health UI")]
    public Image healthFill;
    public Image lagFill;
    public TMP_Text healthText;
    public float lagSpeed = 1.5f;

    [Header("Top Right Info")]
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text levelText;

    [Header("Overlay Panels")]
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject gameOverPanel;

    [Header("Panel Texts")]
    public TMP_Text winScoreText;
    public TMP_Text gameOverScoreText;

    [Header("Pause Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private float targetFill = 1f;
    private int currentScore = 0;
    private bool isPaused = false;
    private bool isEnded = false;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        if (pausePanel) pausePanel.SetActive(false);
        if (winPanel) winPanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);

        if (levelText)
            levelText.text = SceneManager.GetActiveScene().name;

        if (musicSlider)
        {
            musicSlider.value = SaveData.GetMusicVolume();
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        if (sfxSlider)
        {
            sfxSlider.value = SaveData.GetSfxVolume();
            sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        }

        UpdateScore(0);
        UpdateLives(3);
        UpdateHealth(100, 100);
    }

    void Update()
    {
        if (!isEnded && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (lagFill != null && lagFill.fillAmount > targetFill)
        {
            lagFill.fillAmount = Mathf.MoveTowards(lagFill.fillAmount, targetFill, lagSpeed * Time.unscaledDeltaTime);
        }
    }

    public void UpdateHealth(float current, float max)
    {
        targetFill = max > 0 ? Mathf.Clamp01(current / max) : 0f;
        if (healthFill) healthFill.fillAmount = targetFill;
        if (lagFill && lagFill.fillAmount < targetFill) lagFill.fillAmount = targetFill;
        if (healthText) healthText.text = $"HP: {Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }

    public void UpdateScore(int score)
    {
        currentScore = score;
        if (scoreText) scoreText.text = $"Score: {score}";
    }

    public void UpdateLives(int lives)
    {
        if (livesText) livesText.text = $"Lives: {lives}";
    }

    public void TogglePause()
    {
        if (isEnded) return;
        isPaused = !isPaused;
        if (pausePanel) pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ShowWin()
    {
        if (isEnded) return;
        isEnded = true;
        Time.timeScale = 0f;
        SaveData.TrySaveBestScore(currentScore);
        if (winScoreText) winScoreText.text = $"Score: {currentScore}";
        if (winPanel) winPanel.SetActive(true);
    }

    public void ShowGameOver()
    {
        if (isEnded) return;
        isEnded = true;
        Time.timeScale = 0f;
        SaveData.TrySaveBestScore(currentScore);
        if (gameOverScoreText) gameOverScoreText.text = $"Score: {currentScore}";
        if (gameOverPanel) gameOverPanel.SetActive(true);
    }

    public void OnMusicVolumeChanged(float val)
    {
        SaveData.SetMusicVolume(val);
        AudioListener.volume = val;
    }

    public void OnSfxVolumeChanged(float val)
    {
        SaveData.SetSfxVolume(val);
    }

    // Button Callbacks
    public void OnResumeClicked() => TogglePause();
    public void OnRestartClicked() => LevelLoader.RestartCurrentLevel();
    public void OnNextLevelClicked() => LevelLoader.LoadNextLevel();
    public void OnMainMenuClicked() => LevelLoader.LoadMainMenu();
}