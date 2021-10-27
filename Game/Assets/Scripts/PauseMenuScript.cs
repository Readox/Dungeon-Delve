using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    // I apparently need this here so that it shows up in the Editor
    [SerializeField] GameObject pauseMenu;

    private Camera mainCam;
    public GameObject playerUpgradesPanel;
    public GameObject pauseMenuPanel;
    public GameObject settingsMenuPanel;


    private GameObject player;

    bool isPauseActive;

    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        mainCam.transform.position = new Vector3(960, 540, -10);
    }

    public void Resume()
    {
        mainCam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        pauseMenuPanel.SetActive(false);
        playerUpgradesPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        
    }

    public void OpenSettings()
    {
        settingsMenuPanel.SetActive(true);
    }

    public void MainMenu()
    {

        SceneSwitching ssw = GameObject.FindObjectOfType(typeof(SceneSwitching)) as SceneSwitching;
        ssw.GoToMainMenu();
        ssw.UnloadSceneAsync(2);

    }



    public void OpenPlayerUpgrades()
    {
        playerUpgradesPanel.SetActive(true);



    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPauseActive)
        {
            isPauseActive = true;
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPauseActive)
        {
            isPauseActive = false;
            Resume();
        }
    }

    public void Awake()
    {
        mainCam = GameObject.Find("MainCamera").GetComponent<Camera>();
        pauseMenuPanel.SetActive(false);
        playerUpgradesPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    void Start()
    {
        
    }

}
