using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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

    public void UpdateGameState(string state)
    {
        // Handle game state updates
        Debug.Log("Game state updated to: " + state);
    }

    public void UpdateGlobalSettings(string setting, float value)
    {
        // Handle global settings updates
        Debug.Log("Setting: " + setting + " updated to: " + value);
    }
}
