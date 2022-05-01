using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Mov : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TMP_Dropdown MvSlider;

    void Start()
    {
        int mv2 = PlayerPrefs.GetInt("player2Mv", 0);
        MvSlider.value = mv2;
    }

    // Update is called once per frame
    public void SetDisplacement(int displacementIndex)
    {
        PlayerPrefs.SetInt("player2Mv", displacementIndex);
    }
}
