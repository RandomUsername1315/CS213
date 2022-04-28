using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Globalization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Animations;

public class GameManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject winnerCanvas;
    
    public GameObject startCanvas;

    public Text text;
    public AudioSource audioSource;

    private bool isRunning;
    private bool gameOver;
    // Start is called before the first frame update
    public void Start()
    {
        audioSource = audioSource.GetComponent<AudioSource>();
        isRunning = false;
        gameOver = false;
        if (PlayerPrefs.GetInt("VolumeEnable", 1) == 1){
            audioSource.enabled = true;
            audioSource.volume = PlayerPrefs.GetFloat("Volume", audioSource.volume);
        } else {
            audioSource.enabled = false;
        }
        
        winnerCanvas.SetActive(false);
        canvas.SetActive(true);
        startCanvas.SetActive(true);
    }

    public void startGame(){
        if(!isRunning){
            isRunning = true;
            startCanvas.SetActive(false);
        }
    }

    public bool isGameRunning(){
        return isRunning;
    }

    public bool gameIsOver(){
        return gameOver;
    }
    public void gameOverMode(){
        gameOver = true;
        isRunning = false;
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
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
