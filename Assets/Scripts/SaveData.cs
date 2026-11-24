using UnityEngine;

public static class SaveData
{
    private const string KEY_BEST_SCORE = "BestScore";
    private const string KEY_UNLOCKED_LEVEL = "UnlockedLevel";
    private const string KEY_MUSIC_VOL = "MusicVolume";
    private const string KEY_SFX_VOL = "SfxVolume";

    public static int GetBestScore() => PlayerPrefs.GetInt(KEY_BEST_SCORE, 0);

    public static void TrySaveBestScore(int score)
    {
        if (score > GetBestScore())
        {
            PlayerPrefs.SetInt(KEY_BEST_SCORE, score);
            PlayerPrefs.Save();
        }
    }

    public static int GetUnlockedLevel() => PlayerPrefs.GetInt(KEY_UNLOCKED_LEVEL, 1);

    public static void UnlockLevel(int levelIndex)
    {
        if (levelIndex > GetUnlockedLevel())
        {
            PlayerPrefs.SetInt(KEY_UNLOCKED_LEVEL, levelIndex);
            PlayerPrefs.Save();
        }
    }

    public static float GetMusicVolume() => PlayerPrefs.GetFloat(KEY_MUSIC_VOL, 0.8f);
    public static void SetMusicVolume(float v)
    {
        PlayerPrefs.SetFloat(KEY_MUSIC_VOL, Mathf.Clamp01(v));
        PlayerPrefs.Save();
    }

    public static float GetSfxVolume() => PlayerPrefs.GetFloat(KEY_SFX_VOL, 0.8f);
    public static void SetSfxVolume(float v)
    {
        PlayerPrefs.SetFloat(KEY_SFX_VOL, Mathf.Clamp01(v));
        PlayerPrefs.Save();
    }
}