using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    private Vector3[] initialPositions;

    private void Awake(){
        //save the initial positions of all enemies in the room
        initialPositions = new Vector3[enemies.Length];
        for(int i = 0; i < enemies.Length; i++){
            if(enemies[i] != null)
                initialPositions[i] = enemies[i].transform.position;
        }
    }
    public void ActivateRoom(bool status){
        //activate or deactivate all enemies in the room
        for(int i = 0; i < enemies.Length; i++){
            if(enemies[i] != null)
                enemies[i].SetActive(status);
                enemies[i].transform.position = initialPositions[i];
        }
    }
}
