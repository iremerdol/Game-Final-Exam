using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour, IHealthObserver
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        playerHealth.AddObserver(this);
        totalHealthBar.fillAmount = playerHealth.GetCurrentHealth() / playerHealth.GetMaxHealth(); // Assuming you add a method to get starting health
    }

    private void OnDestroy()
    {
        playerHealth.RemoveObserver(this);
    }

    public void OnHealthChanged(float currentHealth)
    {
        currentHealthBar.fillAmount = currentHealth / playerHealth.GetMaxHealth(); // Assuming you add a method to get starting health
    }

    public void OnPlayerDied()
    {
        // Optionally handle player death
    }

    public void OnPlayerRespawned(Transform checkpointParent)
    {
        throw new System.NotImplementedException();
    }
}


/* public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start(){
        totalHealthBar.fillAmount = playerHealth.GetCurrentHealth() / 10;
    }

    private void Update(){
        currentHealthBar.fillAmount = playerHealth.GetCurrentHealth() / 10;
    }
} */