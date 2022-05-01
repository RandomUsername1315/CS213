using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC2 : MonoBehaviour
{
    public TMPro.TMP_Dropdown DisplacementDD;

    void Start()
    {
        int color = PlayerPrefs.GetInt("color1", 0);
        DisplacementDD.value = color;
    }

    void Update(){
        PlayerPrefs.SetInt("color1", DisplacementDD.value);
    }
        
}
