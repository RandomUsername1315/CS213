using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeValue : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TMP_Dropdown TimeSlider;
    void Start()
    {
        int defaultTime = PlayerPrefs.GetInt("time", 0);
        TimeSlider.value = defaultTime;   
    }

    public void SetDisplacement(int displacementIndex)
    {
        PlayerPrefs.SetInt("time", displacementIndex);
    }
}
