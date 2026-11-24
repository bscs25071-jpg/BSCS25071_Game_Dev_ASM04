using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Image loadingBar;
    public TMP_Text loadingText;

    void Start()
    {
        StartCoroutine(LoadSceneProgress());
    }

    IEnumerator LoadSceneProgress()
    {
        string target = string.IsNullOrEmpty(LevelLoader.TargetScene) ? "MainMenu" : LevelLoader.TargetScene;
        AsyncOperation op = SceneManager.LoadSceneAsync(target);
        op.allowSceneActivation = false;

        float displayProgress = 0f;

        while (!op.isDone)
        {
            float realProgress = Mathf.Clamp01(op.progress / 0.9f);
            displayProgress = Mathf.MoveTowards(displayProgress, realProgress, Time.unscaledDeltaTime * 1.5f);

            if (loadingBar != null)
                loadingBar.fillAmount = displayProgress;

            if (loadingText != null)
                loadingText.text = $"Loading... {Mathf.RoundToInt(displayProgress * 100f)}%";

            if (displayProgress >= 1f && op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}