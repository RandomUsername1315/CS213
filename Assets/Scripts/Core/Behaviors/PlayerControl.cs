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
    private static Vector3 startPoint = new Vector3(2.827304f, 0, -5.000934f);

    private int lives = 3;

    private int score = 0;

    private float time = float.NaN;
    
    private Vector3 force = Vector3.zero;

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
       // GetComponent<GameManagerArrows>().level();
    }

    private void incrementScore(){
        score +=1;
    }
    private void decreaseScore(){
        score -=1;
    }

    public int livesLeft(){
        return lives;
    }

    void OnCollisionEnter(Collision other) {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerArrows>().isGameRunning()){
            return;
        } 
        // and the blocker must be in an active state
        else if(other.gameObject.tag == "blocker"){
            currState = PlayerState.starting;
            //if(attackemodeOrSomethng){decreseScore()};
            //and remove a life.
            //--lives;
            if(lives < 0){
                //Ends the game if the player have not lives left
                GetComponent<GameManagerArrows>().gameOverMode();
            }
        }
        else if(other.gameObject.tag == "target"){
            incrementScore();            
            currState = PlayerState.starting;

        }
        else if(other.gameObject.tag == "wall"){
            //Reset
            currState = PlayerState.starting;
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
            agent.isMoved = false;

            Vector3 baseForce = new Vector3( startPoint.x - position.x, 0, startPoint.z - position.z);
            print(baseForce + " with magnitude " + baseForce.magnitude);
            force.x = baseForce.x * baseForce.magnitude / startPoint.magnitude * agent.maxAccel * this.GetComponent<Rigidbody>().mass; 
            force.z = baseForce.z * baseForce.magnitude / startPoint.magnitude * agent.maxAccel *this.GetComponent<Rigidbody>().mass;
        }
    }

    public void startGame(){
        currState = PlayerState.starting;
        agent.isMoved = false;
        time = float.NaN;
    }


       public override Steering GetSteering()
    {
        //the whole movement (and state control)is going to be controlled by this function 
        Steering steering = new Steering();
        UnityEngine.Vector3 position = transform.localPosition;

        switch (currState){
            case PlayerState.waiting:
                break;
            case PlayerState.flying:
                if (position.x < startPoint.x){
                    steering.linear = force;
                } else {
                    if (float.IsNaN(time)){
                        time = Time.time;
                    }
                    steering.linear = trajectory();
                    print("Temps: " + time + ", force: "+ steering.linear.ToString());
               }
                break;
            case PlayerState.starting:

                Vector3 speed = new Vector3(0, 0, 0); 
                speed.x = (-position.x + startPoint.x) / 20;
                speed.z = (-position.z + startPoint.z) / 20;
                steering.linear = maxedVect(speed) * agent.maxAccel * this.GetComponent<Rigidbody>().mass;
                if (rightStartPos()){
                    steering.linear = Vector3.zero;
                    steering.angular = 0;
                    agent.SetSteering(steering);
                    agent.isMoved = true;
                    agent.ClearHapticFeedback();
                    agent.SetCasualBackdriveAssistEnabled(false);
                    agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.blue, 0);
                    print("Now I'm playable");
                    currState = PlayerState.loading;
                }
                break;

            case PlayerState.loading:
                float horizontal = Input.GetAxis("Horizontal_SecondDog");
                float vertical = Input.GetAxis("Vertical_SecondDog");
                steering.linear = new Vector3(horizontal, 0, vertical)* agent.maxAccel * this.GetComponent<Rigidbody>().mass;
                steering.linear = this.transform.parent.TransformDirection (Vector3.ClampMagnitude(steering.linear , agent.maxAccel));
                if (Input.GetKeyDown(KeyCode.Return)){
                    OnCelluloTouchReleased(0);
                }
                break;
        }
        return steering;
    }


    //time is when the player is first launched
    private Vector3 trajectory(){
        float timeDiff = Time.time - time;
        Vector3 nextPos = new Vector3(0,0,0);

        nextPos.x = (float) (force.x);
        nextPos.z = (float) (force.z + Physics.gravity.z * timeDiff * this.GetComponent<Rigidbody>().mass); 

        return nextPos;
    }

    private bool rightStartPos(){
        UnityEngine.Vector3 pos = transform.localPosition;
        double distance = Math.Sqrt((pos.x - startPoint.x) * (pos.x - startPoint.x) + (pos.z - startPoint.z) * (pos.z - startPoint.z));
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
