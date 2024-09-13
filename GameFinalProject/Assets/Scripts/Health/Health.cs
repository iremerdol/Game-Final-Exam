using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthObserver
{
    void OnHealthChanged(float currentHealth);
    void OnPlayerDied();
    void OnPlayerRespawned(Transform checkpointParent);
}

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;

    [SerializeField] private int maxHealth = 10;
    private float currentHealth;
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFrameDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    private List<IHealthObserver> observers = new List<IHealthObserver>();

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void AddObserver(IHealthObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IHealthObserver observer)
    {
        observers.Remove(observer);
    }

    public void TakeDamage(float damage)
    {
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        NotifyHealthChanged();
        
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invulnerability());
            SoundManager.Instance.PlaySound(hurtSound);
        }
        else
        {
            if (!dead)
            {
                foreach (Behaviour component in components)
                {
                    component.enabled = false;
                }
                anim.SetBool("grounded", true);
                anim.SetTrigger("die");
                dead = true;
                SoundManager.Instance.PlaySound(deathSound);
                NotifyPlayerDied();
                if(!gameObject.CompareTag("Player"))
                {
                    StartCoroutine(LateCall(1));
                }
            }
        }
    }

    IEnumerator LateCall(float seconds)
    {
        yield return new WaitForSeconds(seconds);
 
        gameObject.SetActive(false);
    }

    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
        NotifyHealthChanged();
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetStartingHealth()
    {
        return startingHealth;
    }

    public void Respawn(Transform checkpointParent)
    {
        dead = false;
        AddHealth(startingHealth);
        anim.ResetTrigger("die");
        anim.Play("Idle");
        StartCoroutine(Invulnerability());

        foreach (Behaviour component in components)
        {
            component.enabled = true;
        }

        // Notify observers that the player has respawned with the room (checkpoint's parent)
        NotifyPlayerRespawned(checkpointParent);
    }

    private void NotifyPlayerRespawned(Transform checkpointParent)
    {
        foreach (var observer in observers)
        {
            if (observer is IHealthObserver healthObserver)
            {
                healthObserver.OnPlayerRespawned(checkpointParent);
            }
        }
    }

    private IEnumerator Invulnerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
        }
        
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }

    private void NotifyHealthChanged()
    {
        foreach (var observer in observers)
        {
            observer.OnHealthChanged(currentHealth);
        }
    }

    private void NotifyPlayerDied()
    {
        foreach (var observer in observers)
        {
            observer.OnPlayerDied();
        }
    }
}



/* public class Health : MonoBehaviour
{
    [Header ("Health")] 
    [SerializeField] private float startingHealth;

    private float currentHealth;
    private Animator anim;
    private bool dead;
    
    [Header ("iFrames")]
    [SerializeField] private float iFrameDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header ("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    [Header ("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    private void Awake(){
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage){
        if(invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        if(currentHealth > 0){
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
            SoundManager.instance.PlaySound(hurtSound);
        }
        else{
            if(!dead){                
                //deactivate all attached component classes
                foreach(Behaviour component in components){
                    component.enabled = false;
                }
                anim.SetBool("grounded", true);
                anim.SetTrigger("die");

                dead = true;
                SoundManager.instance.PlaySound(deathSound);
            }
        }  
    }
    public void AddHealth(float value){
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
    }
    public float GetCurrentHealth(){
        return currentHealth;
    }
    public void Respawn(){
        dead = false;
        AddHealth(startingHealth);
        anim.ResetTrigger("die");
        anim.Play("Idle");
        StartCoroutine(Invunerability());

        //activate all attached component classes
        foreach(Behaviour component in components){
            component.enabled = true;
        }
    }
    private IEnumerator Invunerability(){
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        //invurnerability duration
        for(int i = 0; i < numberOfFlashes; i++){
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFrameDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);  
        invulnerable = false; 
    }
    private void Deactivate(){
        gameObject.SetActive(false);
    }
} */
