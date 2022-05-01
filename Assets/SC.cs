using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC : MonoBehaviour
{
    public TMPro.TMP_Dropdown DisplacementDD;

    void Start()
    {
        int Displacement = PlayerPrefs.GetInt("_qualityIndex", 0);
        DisplacementDD.value = Displacement;
    }

    public void SetDisplacement(int displacementIndex)
    {
        PlayerPrefs.SetInt("_qualityIndex", displacementIndex);
    }
}
