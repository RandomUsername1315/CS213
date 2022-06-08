using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SC : MonoBehaviour
{
    public void Start(){
        TMP_Dropdown controls = GameObject.FindGameObjectWithTag("dropdown1").GetComponent<TMP_Dropdown>();
        controls.value = PlayerPrefs.GetInt("displacement1", 0);
        controls.RefreshShownValue();
    }
    public void SetDisplacement(int displacementIndex)
    {
        PlayerPrefs.SetInt("displacement1", displacementIndex);
    }
}
