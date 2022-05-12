using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject settingsPanel;
    public GameObject mainMenuPanel;
    public GameObject playerMenuPanel;

    private PlayerSkills playerSkills_script;

    private int resetPresses = 0;

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

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        //mainMenuPanel.SetActive(true);
    }

    public void ResetAllPlayerSaveFiles()
    {
        if (resetPresses < 2)
        {
            Debug.Log("WARNING: This action cannot be undone!");
            resetPresses += 1;
        }
        else
        {
            playerSkills_script.ResetAllSaveFiles();
            resetPresses = 0;
            Debug.Log("All player save files have been reset!");
        }
    }

    public void StartGame()
    {
        //SceneManager.LoadScene("Level 0", LoadSceneMode.Additive);
        //SceneManager.UnloadSceneAsync("MainMenu");
        
        Time.timeScale = 1f;
        
        StartCoroutine(SceneSwitch());
    }

    IEnumerator SceneSwitch()
    {
        if (SceneManager.GetSceneByName("Level 0").IsValid())
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level 0"));

            GameObject mainCam = GameObject.FindWithTag("MainCamera");
            GameObject.FindWithTag("GameController").GetComponent<PlayerStats>().SetUIActiveState("true");
            CameraFollowPlayer camFollow_Script = mainCam.GetComponent<CameraFollowPlayer>();
            mainCam.transform.position = camFollow_Script.GetPlayerLoc().position;

            yield return null;
            SceneManager.UnloadSceneAsync("MainMenu");
        }
        else
        {
            SceneManager.LoadScene("Level 0", LoadSceneMode.Additive);
            yield return null;
            StartCoroutine(SceneSwitch());
        }
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
        playerSkills_script = GameObject.Find("Pause Menu").GetComponent<PlayerSkills>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
