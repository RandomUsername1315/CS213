using UnityEngine;
using System;


public enum WallTargetState
{
    wall = 0,
    target = 1
}


public class WallAndTarget : AgentBehaviour
{
    private bool isMoving;
    private Vector3 target;
    private Vector3 nextTarget;
    // Whether it is the wall or the target
    public WallTargetState state = 0;
    private bool preparingLevel = false;

    // Whether the wall is a standard wall or implies loss of lives
    private bool active = false;

    private GameManagerArrows manager;

    public void Start(){
        manager =  GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerArrows>();
    }

    public void setLevel(bool isTarget, bool active, bool moving, Vector3 pos1, Vector3 pos2){
        preparingLevel = true;
        state = (isTarget) ? WallTargetState.target : this.state;
        target = pos2;
        nextTarget = pos1;
        isMoving = moving;
        this.active = active;

        Color playerColor = (isTarget) ? Color.blue : ((active) ? Color.red: Color.black);
        agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, playerColor, 0);
        agent.SetGoalPosition(pos1.x, pos1.z,agent.maxSpeed);
    }

    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        Vector3 position = transform.localPosition;
        if (rightPos() && isMoving){
            Vector3 tmp = target;
            target = nextTarget;
            nextTarget = tmp;
        }
        Vector3 speed = new Vector3(0, 0, 0);
        speed.x = -position.x + target.x;
        speed.z = -position.z + target.z;

        
        steering.linear =  Vector3.ClampMagnitude(speed, agent.maxAccel);



        return steering;

    }
    
    public bool rightPos(){
        Vector3 pos = transform.localPosition;
        double dist = Math.Sqrt((double)((pos.x-target.x)*(pos.x-target.x) + (pos.z-target.z)*(pos.z-target.z))); 
        return dist < 2;
    }

    public void OnGoalPoseReached(){
        if (preparingLevel){
            manager.imready(this);
            preparingLevel = false;
        }        
    }

    //active means that the player looses points it it comes into contact with the blocker
    public bool isActive(){
        if(state == 0 && active){
            return true;
        }
        return false;

    }



}