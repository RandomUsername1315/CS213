using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVolume : MonoBehaviour
{
    
    public void SaveSliderValue(System.Single volume){

        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void EnableVolumeControl(bool enableVolume){
        if (enableVolume){ 
            PlayerPrefs.SetInt("VolumeEnable", 1);
            print(1);
        } else
        {
            PlayerPrefs.SetInt("VolumeEnable", 0);
        }
    }
}
