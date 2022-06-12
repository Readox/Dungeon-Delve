using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam.transform.position = new Vector3(960, 540, -10);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        //StartCoroutine(LoadMainMenu(0.1f));
        StartCoroutine(SetActiveSceneToMainMenu(0.5f));
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
    }

    IEnumerator SetActiveSceneToMainMenu(float t)
    {
        yield return new WaitForSeconds(t);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
    }

    IEnumerator LoadMainMenu(float t)
    {
        yield return new WaitForSeconds(t);
        Debug.Log("Over Here");
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
