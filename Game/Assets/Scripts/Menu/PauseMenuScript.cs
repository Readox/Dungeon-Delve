using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    // I apparently need this here so that it shows up in the Editor
    [SerializeField] GameObject pauseMenu;

    private Camera mainCam;
    private PlayerSkills ps;
    public GameObject playerUpgradesPanel;
    public GameObject pauseMenuPanel;
    public GameObject settingsMenuPanel;
    public Transform player;

    //Set camera offset (to -10z)
    [SerializeField] Vector3 offset;

    bool isPauseActive;
    private string currentMenu;

    public void Pause()
    {
        isPauseActive = true;
        currentMenu = "pause";
        GameObject.FindWithTag("GameController").GetComponent<PlayerStats>().SetUIActiveState("false");
        player.gameObject.GetComponent<PlayerMovement>().enabled = false; // Prevents dodging and movement while in menu
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        mainCam.transform.position = new Vector3(960, 540, -10);
    }

    public void Resume()
    {
        isPauseActive = false;
        currentMenu = null;
        GameObject.FindWithTag("GameController").GetComponent<PlayerStats>().SetUIActiveState("true");
        GameObject.FindWithTag("GameController").GetComponent<PlayerStats>().CheckHealthMax();
        player.gameObject.GetComponent<PlayerMovement>().enabled = true;
        mainCam.transform.position = player.position + offset;
        //mainCam.position = player.position + offset;
        //mainCam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        pauseMenuPanel.SetActive(false);
        playerUpgradesPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenSettings()
    {
        currentMenu = "settings";
        settingsMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }

    public void ReturnFromSettings()
    {
        currentMenu = "pause";
        pauseMenuPanel.SetActive(true);
        settingsMenuPanel.SetActive(false);
    }


    public void MainMenu()
    {
        isPauseActive = false;
        Time.timeScale = 0f;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        StartCoroutine(SceneSwitch());
    }

    IEnumerator SceneSwitch()
    {
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
        yield return null;
        SceneManager.UnloadSceneAsync("Level 0");
    }



    public void OpenPlayerUpgrades()
    {
        currentMenu = "upgrades";
        //ps.LoadGameState();
        //ps.Load(ps.currentBuildDropdownPath);
        playerUpgradesPanel.SetActive(true);
        ps.OpenUpgradesMenuStart();
        pauseMenuPanel.SetActive(false);
    }

    public void ReturnFromPlayerUpgrades()
    {
        currentMenu = "pause";
        GameObject.FindWithTag("GameController").GetComponent<PlayerStats>().CheckHealthMax();
        pauseMenuPanel.SetActive(true);
        //ps.SaveGameState();
        ps.Save();
        playerUpgradesPanel.SetActive(false);
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPauseActive && !SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            isPauseActive = true;
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPauseActive)
        {
            if (currentMenu.Equals("upgrades"))
            {
                ReturnFromPlayerUpgrades();
            }
            else if (currentMenu.Equals("settings"))
            {
                ReturnFromSettings();
            }
            else if (currentMenu.Equals("pause"))// the user must be in the main pause menu, so take them back to the game
            {
                isPauseActive = false;
                Resume();
            }
        }
    }

    public void Awake()
    {
        ps = GetComponent<PlayerSkills>();

        pauseMenuPanel.SetActive(false);
        playerUpgradesPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
    }

    void Start()
    {
        mainCam = Camera.main;
    }

}
