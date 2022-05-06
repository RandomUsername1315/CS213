using System;
using UnityEngine;

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

    private static float maxDeltaDistance = 0.1f;
    private static float xStart = 2.827304f;
    private static float zStart = -5.000934f;

    private float time;
    private float initZPos;
    private float initXSpos;
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
   //     this.GetComponentInParent<AudioSource>().volume = PlayerPrefs.GetFloat("Volume", this.GetComponentInParent<AudioSource>().volume);
        agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, playerColor, 0);
        agent.isMoved = false;
    }
    public void update() {
    }


    void OnCollisionEnter(Collision other) {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerArrows>().isGameRunning()){
            return;
        }  
    }
    
    public override void OnCelluloLongTouch(int val){
        base.OnCelluloLongTouch(val);
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
            Vector3 position = transform.localPosition;
            
            currState = PlayerState.flying;
            agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.white, 0);
            time = Time.fixedTime;
            agent.isMoved = false;
        }
    }

    public void startGame(){
        currState = PlayerState.starting;
        print("Start game");
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
        UnityEngine.Vector3 position = transform.localPosition;
        switch (currState){
            case PlayerState.waiting:
                //print("Position" + transform.localPosition);
                steering.linear = Vector3.zero;
                steering.angular = 0f;
                break;

            case PlayerState.flying:
                if (position.x < xStart){
                    Vector3 speed = Vector3.zero;
                        speed.x = (xStart - )
                    speed;
                }
                break;
            case PlayerState.starting:
                Vector3 speed = new Vector3(0, 0, 0); 
                speed.x = (-position.x + xStart) / 20;
                speed.z = (-position.z + zStart) / 20;
                print (speed.ToString());
                steering.linear = maxedVect(speed) * agent.maxAccel;
                if (rightStartPos()){
                    currState = PlayerState.loading;
                    steering.linear = Vector3.zero;
                    steering.angular = 0;
                    agent.SetSteering(steering);
                    agent.isMoved = true;
                    agent.ClearHapticFeedback();
                    agent.SetCasualBackdriveAssistEnabled(false);
                    agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.blue, 0);
                    print("Now I'm playable");
                }
                break;

            case PlayerState.loading:
                steering.linear = Vector3.zero;
                steering.angular = 0;
                print("loading");
                break;
        }
        return steering;
    }

    private bool rightStartPos(){
        UnityEngine.Vector3 pos = transform.localPosition;
        double distance = Math.Sqrt((pos.x - xStart) * (pos.x - xStart) + (pos.z - zStart) * (pos.z - zStart));
        return distance < maxDeltaDistance;
    }

    private Vector3 maxedVect(Vector3 vect){
        float mag =  vect.magnitude;
        if (mag <= 1) {
           return vect;
        }
        vect.x = vect.x / mag;
        vect.y = vect.y / mag;
        vect.z = vect.z / mag;
        return vect;
    }
}
