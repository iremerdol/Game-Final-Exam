using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, IHealthObserver
{
    //Room Camera
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    //Follow Player
    [SerializeField] private Transform player;
    //[SerializeField] private float aheadDistance;
    //[SerializeField] private float cameraSpeed;
    private float lookAhead;

    private void Awake()
    {
        // Subscribe to player's health changes
        var playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.AddObserver(this);
        }
    }

    private void Update(){
        //Room Camera
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);

        //Follow Player
        //transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
        //lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }
    public void MoveToNewRoom(Transform newRoom){
        currentPosX = newRoom.position.x;
    }

    public void OnPlayerRespawned(Transform checkpointParent)
    {
        // Move camera to the new room upon respawn
        MoveToNewRoom(checkpointParent);
    }

    public void OnHealthChanged(float currentHealth)
    {
        return;
    }

    public void OnPlayerDied()
    {
        return;
    }
}
