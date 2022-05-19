using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerArrows : MonoBehaviour
{
    public enum ManagerStates{
        preparingLevel,
        runningLevel,
        endLevel,
        endGame,
        beforeStart
    }
    private
    public GameObject canvas;
    public GameObject winnerCanvas;
    
    public GameObject startCanvas;

    public Text text;

    private int currLevel;
    public AudioSource audioSource;
    private bool isRunning;
    private bool gameOver;
    // Start is called before the first frame update
    public void Start()
    {
        audioSource = audioSource.GetComponent<AudioSource>();
        isRunning = false;
        gameOver = false;
        currLevel = 1;
        if (PlayerPrefs.GetInt("MusicEnable", 1) == 1){
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
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().startGame();
    }

    public bool isGameRunning(){
        return isRunning;
    }

    public void nextLevel(){
        currLevel +=1;
        setLevels(currLevel);
    }
    public int getLevel(){
        return currLevel;
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

    private void setLevels(int levelNumber){
       PlayerPrefs.SetString("Level1", "level"+levelNumber);

    }

    public Vector3 nextWallPosition(){

        //Default before debug
        return new Vector3(0.0f,0.0f,0.0f);
    }

    
    public Vector3 nextTargetPosition(){

        //Default before debug
        return new Vector3(0.0f,0.0f,0.0f);
    }
}
