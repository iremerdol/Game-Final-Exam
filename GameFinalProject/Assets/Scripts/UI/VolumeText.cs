using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeText : MonoBehaviour
{
    [SerializeField] private string volumeName;
    [SerializeField] private string textIntro; //sounds: or music:
    private Text txt;

    private void Awake()
    {
        txt = GetComponent<Text>();
    }
    private void Update(){
        UpdateVolume();
    }
    private void UpdateVolume(){
        int volumeValue = (int)(PlayerPrefs.GetFloat(volumeName) * 100);
        txt.text = textIntro + volumeValue.ToString();
    }
}
