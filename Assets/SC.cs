using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC : MonoBehaviour
{
    public void SetDisplacement(int displacementIndex)
    {
        PlayerPrefs.SetInt("displacement1", displacementIndex);
    }
}
