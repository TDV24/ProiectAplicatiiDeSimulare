using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void quitApp()
    {
        Debug.Log("quit");
        Application.Quit();
    }
    public void playGame()
    {
        SceneManager.LoadScene("DemoScene");
    }
    public void mainMenuLoad()
    {
        SceneManager.LoadScene("Menu Scene");
    }
}
