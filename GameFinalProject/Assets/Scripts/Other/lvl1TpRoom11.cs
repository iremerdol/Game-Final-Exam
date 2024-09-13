using UnityEngine;

public class lvl1TpRoom11 : MonoBehaviour
{
    // Target room position to teleport the player to
    public Transform targetRoom;

    // Reference to the player GameObject
    public GameObject player;

    // Check if the player has entered the trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the object entering the collider is the player
        if (other.gameObject == player)
        {
            // Teleport the player to the target room position
            player.transform.position = targetRoom.position;

            //reach the camera and move to the new room
            Camera.main.GetComponent<CameraController>().MoveToNewRoom(targetRoom);

        }
    }

}
