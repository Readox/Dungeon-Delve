using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    // I apparently need this here so that it shows up in the Editor
    [SerializeField] GameObject pauseMenu;

    private Camera mainCam;
    public GameObject playerUpgradesPanel;
    public GameObject pauseMenuPanel;
    public GameObject settingsMenuPanel;
    public Transform player;

    //Set camera offset (to -10z)
    [SerializeField] Vector3 offset;

    bool isPauseActive;

    public void Pause()
    {
        GameObject.FindWithTag("GameController").GetComponent<PlayerStats>().SetUIActiveState("false");
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        mainCam.transform.position = new Vector3(960, 540, -10);
    }

    public void Resume()
    {
        GameObject.FindWithTag("GameController").GetComponent<PlayerStats>().SetUIActiveState("true");
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
        settingsMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }

    public void ReturnFromSettings()
    {
        pauseMenuPanel.SetActive(true);
        settingsMenuPanel.SetActive(false);
    }


    public void MainMenu()
    {
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
        playerUpgradesPanel.SetActive(true);
        GetComponent<PlayerSkills>().OpenUpgradesMenuStart();
        pauseMenuPanel.SetActive(false);
    }

    public void ReturnFromPlayerUpgrades()
    {
        pauseMenuPanel.SetActive(true);
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
            isPauseActive = false;
            Resume();
        }
    }

    public void Awake()
    {
        pauseMenuPanel.SetActive(false);
        playerUpgradesPanel.SetActive(false);
        settingsMenuPanel.SetActive(false);
    }

    void Start()
    {
        mainCam = Camera.main;
    }

}
