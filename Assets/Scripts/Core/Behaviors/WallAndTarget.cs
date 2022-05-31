using UnityEngine;


public enum WallTargetState
{
    wall = 0,
    target = 1
}


public class WallAndTarget : AgentBehaviour
{
    private Vector3 target;
    public WallTargetState state = 0;

    private bool isactive = false;

    private GameManagerArrows manager;

    public void Start()
    {
        askForNextTarget();
        agent.SetGoalPosition(target.x, target.y, agent.maxSpeed);
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
        manager.imready(this);
    }

    //active means that the player looses points it it comes into contact with the blocker
    public bool isActive(){
        if(state == 0 && isactive){
            return true;
        }
        return false;

    }



}