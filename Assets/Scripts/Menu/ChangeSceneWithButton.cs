using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneWithButton : MonoBehaviour
{
    public void LoadScene(string scenename)
    {
        if(scenename != "SpaceGhostSheepGame"){
            PlayerPrefs.SetString("Level", "level1");}
        else{
            PlayerPrefs.SetString("Level", scenename);
            }
        
        SceneManager.LoadScene("ArrowGame");


    }
}
