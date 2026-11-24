using UnityEngine;

public class CreditsController : MonoBehaviour
{
    public void BackToMainMenu()
    {
        LevelLoader.LoadNextLevel(); // Credits ke baad wapas MainMenu par wrap karega
    }
}