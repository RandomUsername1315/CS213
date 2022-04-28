using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVolume : MonoBehaviour
{
    
    public void SaveSliderValue(System.Single volume){

        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void EnableVolumeControl(bool enableVolume){
        PlayerPrefs.SetInt("MusicEnable", (enableVolume) ? 1 : 0);
        
    }
}
