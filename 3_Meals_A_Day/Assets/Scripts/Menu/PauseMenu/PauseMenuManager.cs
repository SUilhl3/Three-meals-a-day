using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private bool freezeTimeOnPause = true;

    private bool _isPaused;
    public bool IsPaused => _isPaused;

    public static PauseMenuManager Instance { get; private set; }

    public System.Action OnPaused;
    public System.Action OnResumed;

    private InputAction _pauseAction;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Keep alive across scene loads only if you want one persistent manager.
        //DontDestroyOnLoad(gameObject);

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        // Bind Escape (and gamepad Start) to toggle pause.
        // Swap these bindings or hook up an InputActionAsset if you use one.
        _pauseAction = new InputAction("Pause", binding: "<Keyboard>/escape");
        _pauseAction.AddBinding("<Gamepad>/start");
        _pauseAction.performed += _ => TogglePause();
    }

    private void OnEnable()
    {
        _pauseAction?.Enable();
    }

    private void OnDisable()
    {
        _pauseAction?.Disable();
    }

    public void TogglePause()
    {
        if (_isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        if (_isPaused) return;
        _isPaused = true;
        if (freezeTimeOnPause)
        {
            Time.timeScale = 0f;
        }
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
        OnPaused?.Invoke();
    }

    public void Resume()
    {
        if (!_isPaused) return;
        _isPaused = false;
        if (freezeTimeOnPause)
        {
            Time.timeScale = 1f;
        }
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        OnResumed?.Invoke();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Ensure time is running when loading main menu
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartScene()
    {
        Time.timeScale = 1f; // Ensure time is running when restarting scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        // Safety: always restore time if this object is destroyed mid-pause
        Time.timeScale = 1f;
    }

}
