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
        //SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
