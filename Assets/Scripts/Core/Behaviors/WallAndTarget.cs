using UnityEngine;


public enum WallTargetState
{
    wall = 0,
    target = 1
}


public class WallAndTarget : AgentBehaviour
{
    private bool isMoving;
    private Vector3 target;
    private Vector3 moveTarget;
    public WallTargetState state = 0;
    private bool preparingLevel = false;

    // Whether the wall is a standard wall or implies loss of lives
    private bool isActive = false;

    private GameManagerArrows manager;

    public void Start(){
        manager =  GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerArrows>();
    }

    public void setLevel(bool isTarget, bool active, bool moving, Vector3 pos1, Vector3 pos2){
        preparingLevel = true;
        state = (isTarget) ? WallTargetState.target : WallTargetState.state;
        target = pos2;
        moveTarget = pos1;
        isMoving = moving;
        isActive = active;

        Color playerColor = (isTarget) ? Color.green : ((active) ? Color.red: Color.black);
        agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, playerColor, 0);
        agent.SetGoalPose(pos1.x, pos1.z, agent.maxSpeed);
    }

    //TODO : Le code est vraiment pas clair, surtout que l'on fait ce qui est interdit. Je peux faire des states ?
    private void Update()
    {
        //if it is withing a range then it needs to ask for a new position
        //Dont know if we should care that the robot will teletransport to the new pos
        if (Vector3.Distance(transform.localPosition, target) < 10.0f && askForNextTarget())
        {
            agent.SetGoalPose(target.x, target.y, 0, getSpeed(), 120);
        }
    }

    // public override Steering GetSteering()
    // {
    //     Vector3 position = transform.localPosition;

    //     Vector3 speed = new Vector3(0, 0, 0);
    //     speed.x = (-position.x + target.x) / 20;
    //     speed.z = (-position.z + target.z) / 20;

    //     // steering.linear = maxedVect(speed) * agent.maxAccel * this.GetComponent<Rigidbody>().mass;


    //     return new Steering();

    // }
    private float getSpeed(){
        //Speed maybe based on the level

        return manager.getLevel() * 2;
    }

    private bool askForNextTarget(){
        Vector3 nextTarget;

        if (state == WallTargetState.wall)
        {
            nextTarget = manager.nextWallPosition();
        }
        else
        {
            nextTarget = manager.nextTargetPosition();
        }
        
        //Bool implemented to avoid 
        /*
        Some recurrent mistake causing the robots to freeze or not respond or being slow:
        DO NOT SEND COMMANDS TO THE ROBOT INSIDE UPDATE LOOPS.
        The update loops run at a high frequency, sending too many bluetooth messages to the robots will make its buffer fill quickly causing delays. 
        So do not include commands to robots such as SetVisualEffect or SetBackdrivability for example to the robots in the update loops. Instead, have these commands only once on change. 
        */

        if(nextTarget==target){
            return false;
        }
        else{
            target = nextTarget;
            return true;
        }
    }

    public override OnGoalPoseReached(){
        if (preparingLevel){
            manager.imready(this);
            preparingLevel = false;
        } else {
            
        }
        
    }

    //active means that the player looses points it it comes into contact with the blocker
    public bool isActive(){
        if(state == 0 && isactive){
            return true;
        }
        return false;

    }



}