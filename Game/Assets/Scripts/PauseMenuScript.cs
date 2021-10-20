using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    // I apparently need this here so that it shows up in the Editor
    [SerializeField] GameObject pauseMenu;



    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MainMenu(int sceneId)
    {
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(sceneId);
    }

}
