using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject settingsPanel;
    public GameObject mainMenuPanel;
    public GameObject playerMenuPanel;


    public void OpenPlayerMenu()
    {
        playerMenuPanel.SetActive(true);
        Debug.Log("Opening Player Menu");
        //settingsPanel.SetActive(false);
        //mainMenuPanel.SetActive(false);
    }

    public void ClosePlayerMenu()
    {
        playerMenuPanel.SetActive(false);
        //settingsPanel.SetActive(true);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        //mainMenuPanel.SetActive(false);
    }

    public void CloeSettings()
    {
        settingsPanel.SetActive(false);
        //mainMenuPanel.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }





    // Start is called before the first frame update
    void Start()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        playerMenuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
