using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Color : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TMP_Dropdown ColorSlider;

    void Start()
    {
        int color = PlayerPrefs.GetInt("player2Color", 0);
        ColorSlider.value = color;
    }

    // Update is called once per frame
    public void SetColor(int colorIndex)
    {
        PlayerPrefs.SetInt("player2Color", colorIndex);
    }
}
