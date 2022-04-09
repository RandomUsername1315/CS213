using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject winnerCanvas;
    public Text text;
    // Start is called before the first frame update
    public void Start()
    {
        winnerCanvas.SetActive(false);
        canvas.SetActive(true);
    }


    public void gameOverMode(){
        Time.timeScale = 0;
        canvas.SetActive(false);

        GameObject[] dogs = GameObject.FindGameObjectsWithTag("Dog");
        GameObject winner = dogs[0];

        foreach(GameObject dog in dogs){
            if(dog.GetComponent<MoveWithKeyboardBehavior>().getScore() > winner.GetComponent<MoveWithKeyboardBehavior>().getScore()){
                winner = dog;
            }
        }

        text.text = string.Format("{0} won the game", winner.GetComponent<MoveWithKeyboardBehavior>().name);
        winnerCanvas.SetActive(true);
    }

    public void loadMenu(){
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }
}
