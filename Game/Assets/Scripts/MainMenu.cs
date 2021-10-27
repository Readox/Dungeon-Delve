using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Time.timeScale = 1f;

        SceneSwitching ssw = GameObject.FindObjectOfType(typeof(SceneSwitching)) as SceneSwitching;
        ssw.GoToGameLevel(2);
        ssw.UnloadSceneAsync(1);
        

        /*
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        playerMenuPanel.SetActive(false);
        */
    }

    public void ExitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void Awake()
    {
        Time.timeScale = 0f;
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        playerMenuPanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
