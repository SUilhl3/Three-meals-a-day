using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        // Defensive null-checks so the menu doesn't crash if a button is missing
        if (resumeButton != null) resumeButton.onClick.AddListener(OnResume);
        if (restartButton != null) restartButton.onClick.AddListener(OnRestart);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(OnMainMenu);
    }

    private void OnDestroy()
    {
        // Clean up listeners
        if (resumeButton != null) resumeButton.onClick.RemoveListener(OnResume);
        if (restartButton != null) restartButton.onClick.RemoveListener(OnRestart);
        if (mainMenuButton != null) mainMenuButton.onClick.RemoveListener(OnMainMenu);
    }

    private void OnResume()
    {
        if (PauseMenuManager.Instance != null)
            PauseMenuManager.Instance.Resume();
    }

    private void OnRestart()
    {
        if (PauseMenuManager.Instance != null)
            PauseMenuManager.Instance.RestartScene();
    }

    private void OnMainMenu()
    {
        if (PauseMenuManager.Instance != null)
            PauseMenuManager.Instance.LoadMainMenu();
    }


}
