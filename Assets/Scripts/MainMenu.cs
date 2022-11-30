using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string gameScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void QuiteGame()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {

    }

    public void CloseOptions()
    {

    }
}
