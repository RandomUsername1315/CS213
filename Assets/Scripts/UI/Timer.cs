using System;
using UnityEngine;
using UnityEngine.UI;

/**
	This class is the implementation of the timer used in the game and how it is handled in it
*/
public class Timer : MonoBehaviour
{
    private float initTimerValue;
    public Text timerText;
    public Slider slider;
    public int maxMinutes = 5;
    public GameManagerArrows gameManager;

    private float curr_time;


    // Start is called before the first frame update
    public void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerArrows>();
        maxMinutes= PlayerPrefs.GetInt("time", maxMinutes);
        curr_time = maxMinutes * 60;
        timerText.text = string.Format("{0:00}:{1:00}", 0, 0);
    }

    public float getTime(){
        return curr_time;
    }

    // Update is called once per frame
    public void Update() {

        if(gameManager.isGameRunning()){
            curr_time = curr_time - Time.deltaTime;
            slider.value = 1 - curr_time/(maxMinutes * 60);
            timerText.text = string.Format("Remaining time: {0:00}:{1:00}", Math.Floor(curr_time / 60f), curr_time % 60);
        } else {
            timerText.text = string.Format("Paused");
        }
        

        if(curr_time <= 0){
            gameManager.gameOverMode();
        }
    }
}
