using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip interactSound;
    private RectTransform rect;
    private int currentPosition;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    private void Update(){
        //change position
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
            ChangePosition(-1);
        }
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
            ChangePosition(1);
        }

        //interact with options
        if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E)){
            Interact();
        }
    }
    private void Interact(){
        SoundManager.Instance.PlaySound(interactSound);

        //acces the button component and call the onClick event
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    } 
    private void ChangePosition(int change){
        currentPosition += change;

        if(change != 0){
            SoundManager.Instance.PlaySound(changeSound);
        }

        if(currentPosition < 0){
            currentPosition = options.Length - 1;
        }
        else if(currentPosition > options.Length - 1){
            currentPosition = 0;
        }

        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0);
    }  
}
