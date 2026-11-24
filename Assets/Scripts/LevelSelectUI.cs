using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectUI : MonoBehaviour
{
    public Button[] levelButtons; // 5 buttons: Btn_Level1 to 5

    void OnEnable()
    {
        int unlocked = SaveData.GetUnlockedLevel();

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelNum = i + 1;
            Button btn = levelButtons[i];
            if (btn == null) continue;

            bool isUnlocked = levelNum <= unlocked;
            btn.interactable = isUnlocked;

            TMP_Text label = btn.GetComponentInChildren<TMP_Text>();
            if (label != null)
            {
                if (!isUnlocked)
                {
                    label.text = $"Level {levelNum}\n(Locked)";
                }
                else
                {
                    float best = SaveData.GetBestTime(levelNum);
                    label.text = best > 0f
                        ? $"Level {levelNum}\nBest: {best:F1}s"
                        : $"Level {levelNum}\nBest: --";
                }
            }
        }
    }
}