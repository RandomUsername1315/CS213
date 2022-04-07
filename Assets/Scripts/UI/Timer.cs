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
    public float maxMinutes = 5;
    public GameManager gameManager;

    private float curr_time;

    public void Awake() {
        initTimerValue = Time.time; 
    }

    // Start is called before the first frame update
    public void Start() {
        this.Awake();
        curr_time = maxMinutes * 60;
        //timerText = GetComponent<Text>();
        timerText.text = string.Format("{0:00}:{1:00}", 0, 0);
    }

    // Update is called once per frame
    public void Update() {

        curr_time = curr_time - Time.deltaTime;
        print(curr_time);
        slider.value = curr_time/(maxMinutes * 60);
        timerText.text = curr_time.ToString();
    }
}
