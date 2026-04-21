using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject optionsMenu;

    public void PlayGame()
    {
        SceneManager.LoadScene("Bread");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
    }

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
    }
}
