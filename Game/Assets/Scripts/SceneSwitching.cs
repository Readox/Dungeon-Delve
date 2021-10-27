using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitching : MonoBehaviour
{
    
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
    }

    public void GoToGameLevel(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level " + sceneNumber));
    }

    public void UnloadSceneAsync(int sceneNumber)
    {
        SceneManager.UnloadSceneAsync(1);
    }



}
