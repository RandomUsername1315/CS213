using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneWithButton : MonoBehaviour
{
    public void LoadScene(string name)
    {
        if (name != null){
            SceneManager.LoadScene(name);
        } else {
            SceneManager.LoadScene("ArrowGame");
        }
        
    }
}
