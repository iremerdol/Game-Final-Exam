using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake(){
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn(){
        if(currentCheckpoint == null){
            uiManager.GameOver();
            return;
        }

        transform.position = currentCheckpoint.position;
        playerHealth.Respawn(currentCheckpoint.parent); // Pass checkpoint's parent

        // Camera will now move to the checkpoint's parent automatically via observer
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.transform.tag == "Checkpoint"){
            currentCheckpoint = collision.transform;
            SoundManager.Instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }
}

