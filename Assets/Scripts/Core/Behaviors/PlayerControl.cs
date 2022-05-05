using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//agent
//Input Keys
public enum InputKeyboard2{
    arrows =0,
    wasd = 1
}

public enum PlayerState
{
    starting = 0,
    loading = 1,
    waiting = 2,
    flying = 3,
}
public class PlayerControl : AgentBehaviour
{
    public InputKeyboard inputKeyboard; 
    //Physical reference of the score
    public string name = "PlayerX";
    private PlayerState  currState = PlayerState.waiting;
    private static float xStart = 2.827304f;
    private static float zStart = -5.000934f;
    public PlayerState getState(){
        return currState;
    }


    public void Start(){
        Color playerColor;
        int color = 0;
        //Player 1
        inputKeyboard = (InputKeyboard)PlayerPrefs.GetInt("displacement1", 0);
        color = PlayerPrefs.GetInt("color1", 0);                 
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

        agent.isMoved = false;
    }
    public void update() {
        
    }


    void OnCollisionEnter(Collision other) {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().isGameRunning(){
            return;
        }  
    }
    
    public override void OnCelluloLongTouch(int val){
        GameManagerArrows manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerArrows>();
        if (manager.isGameRunning() == false){
            manager.startGame();
            currState = PlayerState.starting;
        } 
    }

    public override void OnCelluloTouchReleased(int key)
    {
        base.OnCelluloTouchReleased(key);
        if (currState == PlayerState.loading){
            currState = PlayerState.flying;
        }
    }


/*    public override Steering GetSteering()
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
    }*/
    
       public override Steering GetSteering()
    {
        //the whole movement (and state control)is going to be controlled by this function 
        Steering steering = new Steering();
        switch (currState){
            case PlayerState.waiting:
                steering.linear = Vector3.zero;
                steering.angular = 0f;
                break;

            case PlayerState.flying:
                // Compute
                break;

            case PlayerState.starting:
                UnityEngine.Vector3 position = transform.position;
                Vector3 speed = new Vector3(0, 0, 0); 
                speed.x = (-position.x + xStart);
                speed.z = (-position.z + zStart);
                speed = Vector3.ClampMagnitude(speed, 0.1f);
                steering.linear = speed * agent.maxAccel;
                if (rightStartPos()){
                    start()
                }
                break;

            case PlayerState.loading:
                break;
        }
        return steering;

    }
}
