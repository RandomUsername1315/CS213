using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC2 : MonoBehaviour
{
    public TMPro.TMP_Dropdown DisplacementDD;

    void Start()
    {
        int color = PlayerPrefs.GetInt("_qualityIndex", 0);
        DisplacementDD.value = color;
    }

    public void SetColor(int colorIndex)
    {
        PlayerPrefs.SetInt("_qualityIndex", colorIndex);
    }
}
