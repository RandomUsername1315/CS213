using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Input Keys
public enum InputKeyboard{
    arrows =0,
    wasd = 1
}

public class MoveWithKeyboardBehavior : AgentBehaviour
{
    public InputKeyboard inputKeyboard; 
    //Physical reference of the score
    public Text score;

    public int points = 0;
    public string name = "PlayerX";

    public bool gemBonus = false;

    public AudioClip winSound;
    public AudioClip loseSound;
    public int getScore(){
        return points; 
    }

    public void resetScore(){
        points = 0;
    }

    public void incrementScore(int val){
        if (val < 0){
            this.GetComponentInParent<AudioSource>().clip = loseSound;
        } else
        {
            this.GetComponentInParent<AudioSource>().clip = winSound;
        }
        this.GetComponentInParent<AudioSource>().Play(0);
        points += val;

    }

    public void Start(){
        Color playerColor;
        int color = 0;
        //Player 1
        if(this.gameObject.name == "CelluloAgent_1"){
            inputKeyboard = (InputKeyboard)PlayerPrefs.GetInt("displacement1", 0);
            color = PlayerPrefs.GetInt("color1", 0);            
        }
        else{
            inputKeyboard = (InputKeyboard)(PlayerPrefs.GetInt("displacement1", 0) == 0 ? 1 :0);
            color = PlayerPrefs.GetInt("player2Color", 0);
        }

        if(color == 0){
            playerColor = Color.red;
            }
        else if(color == 1){
            playerColor = Color.green;
        }
        else{
            playerColor = Color.blue;
        }
        this.GetComponentInParent<AudioSource>().volume = PlayerPrefs.GetFloat("Volume", this.GetComponentInParent<AudioSource>().volume);
        agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, playerColor, 0);
        points = 0;
        score.text = string.Format("{0,3}", points);
    }
    public void Update() {
        score.text = string.Format("{0,3}", points);
    }

    void OnCollisionEnter(Collision other) {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().isGameRunning() &&
            other.gameObject.tag == "Dog" && gemBonus){
            other.gameObject.GetComponent<MoveWithKeyboardBehavior>().incrementScore(-2);
            incrementScore(2);
            gemBonus = false;
        }
        
    }
    
    public override void OnCelluloLongTouch(int key){
        GameManager manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        if (manager.GetReadyToStart() == null){
            manager.SetReadyToStart(this);
        } else {
            if (manager.GetReadyToStart() != this){
                manager.SetReadyToStart(null);
                manager.startGame();
            }
        }
    }
   /* private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Border"){
            if (other.gameObject.name == "Top") {
                GetComponentInParent<CelluloAgent>.
            }
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.tag == "Border"){

        }
    } */
    
    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        float horizontal = 0;
        float vertical = 0;

        // Gets the right Input, depending of the control mode
        if (inputKeyboard == InputKeyboard.wasd){
            horizontal = Input.GetAxis("Horizontal_FirstDog");
            vertical = Input.GetAxis("Vertical_FirstDog");
        } 
        else if (inputKeyboard == InputKeyboard.arrows){
            horizontal = Input.GetAxis("Horizontal_SecondDog");
            vertical = Input.GetAxis("Vertical_SecondDog");
        }
        // Applies the input to the cellulo
        steering.linear = new Vector3(horizontal, 0, vertical)* agent.maxAccel;
        steering.linear = this.transform.parent.TransformDirection (Vector3.ClampMagnitude(steering.linear , agent.maxAccel));
        return steering;
    }
}
