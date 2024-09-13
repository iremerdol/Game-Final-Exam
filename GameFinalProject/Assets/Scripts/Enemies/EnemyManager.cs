using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void CheckEnemiesAndLoadScene()
    {
        StartCoroutine(LateCall(5));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator LateCall(float seconds)
    {
        yield return new WaitForSeconds(seconds);
 
        gameObject.SetActive(false);
    }
}
