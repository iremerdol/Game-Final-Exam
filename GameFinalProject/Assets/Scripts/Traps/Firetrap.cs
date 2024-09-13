using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered; //when the trap gets triggered
    private bool active; //when the trap is active and can hurt the player

    private Health playerHealth;

    [Header("SFX")]
    [SerializeField] private AudioClip firetrapSound;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if(playerHealth != null && active){
            playerHealth.TakeDamage(damage);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player"){
            playerHealth = collision.GetComponent<Health>();

            if(!triggered){
                StartCoroutine(ActivateFiretrap());
            }
            if(active){
                collision.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player"){
            playerHealth = null;
        }
    }

    private IEnumerator ActivateFiretrap()
    {   //turns the trap red to notify the player and triggers the trap
        triggered = true;
        spriteRend.color = Color.red; 

        //wait for delay, activate trap, turn on animation, return color back to normal
        yield return new WaitForSeconds(activationDelay);
        SoundManager.Instance.PlaySound(firetrapSound);
        spriteRend.color = Color.white; //turns the trap back to the initial color
        active = true;
        anim.SetBool("activated", true);

        //wait until x seconds pass, then deactivate the trap and reset all the variables
        yield return new WaitForSeconds(activeTime);
        triggered = false;
        active = false;
        anim.SetBool("activated", false);
    }
}
