using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    private void Awake(){
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }
    private void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            //if pause screem already active unpause and viceversa
            if(pauseScreen.activeInHierarchy){
                PauseGame(false);
            }
            else{
                PauseGame(true);
            }
        }
    }

    #region Game Over
    public void GameOver(){
        gameOverScreen.SetActive(true);
        SoundManager.Instance.PlaySound(gameOverSound);
    }
    public void Restart(){
        SceneManager.LoadScene(2);
    }
    public void MainMenu(){
        SceneManager.LoadScene(0);
    }
    public void Quit(){
        Application.Quit();//quits the game (only works on build)
        #if UNITY_EDITOR        
        UnityEditor.EditorApplication.isPlaying = false;//exit game mode (this code will only be executed in editor)
        #endif
    }
    #endregion

    #region Pause
    public void PauseGame(bool status){
        //if true pause, if false unpause
        pauseScreen.SetActive(status);

        if(status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

    }
    public void SoundVolume(){
        SoundManager.Instance.ChangeSoundVolume(0.1f);
    }
    public void MusicVolume(){
        SoundManager.Instance.ChangeMusicVolume(0.1f);
    }
    #endregion
}
