using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnGameStart()
    {
        SceneManager.LoadScene("InGame");
    }

    public void OnGameExit()
    {
        Application.Quit();
    }
}