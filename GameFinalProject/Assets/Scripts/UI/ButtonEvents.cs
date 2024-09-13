using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Xml.Serialization;

public class ButtonEvents : MonoBehaviour
{
    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void Quit(){
        Application.Quit();//quits the game (only works on build)
        #if UNITY_EDITOR        
        UnityEditor.EditorApplication.isPlaying = false;//exit game mode (this code will only be executed in editor)
        #endif
    }

    public void NewGame()
    {
        //GameManager.instance.DeleteSave();
        //GameManager.instance.LoadTheData();
        // Get count of loaded Scenes and last index
        var lastSceneIndex = SceneManager.sceneCount - 1;
        
        // Get last Scene by index in all loaded Scenes
        var lastLoadedScene = SceneManager.GetSceneAt(lastSceneIndex);
        
        // Unload Scene
        SceneManager.UnloadSceneAsync(lastLoadedScene);
        SceneManager.LoadScene(0);
    }
    public void Back(){
        //current scene
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene - 1);
        Time.timeScale = 1;
    }
    public void Options(){
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
    public void Level1(){
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }
    public void Level2(){
        SceneManager.LoadScene(3);
        Time.timeScale = 1;
    }
}
