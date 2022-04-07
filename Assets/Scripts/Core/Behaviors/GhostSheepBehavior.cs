using System.Globalization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GhostSheepBehavior : AgentBehaviour
{    
    private enum GhostSheepState{
    ghost = 0, 
    sheep = 1
    }   

    private GhostSheepState state;
    public void Start(){
        state = GhostSheepState.sheep;
        Invoke("changeState", Random.Range(5, 20));
    }

    private void changeState() {
        if (state == GhostSheepState.sheep){
            state = GhostSheepState.ghost;
            //transform.SetVisualEffect(0, 255, 0, 0)
            } 
            else {
            state = GhostSheepState.sheep;
            Invoke("changeState", Random.Range(5, 30));
        }
    }
    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        UnityEngine.Vector3 position = transform.position;
        float distance = Mathf.Infinity;
        GameObject[] dogs = GameObject.FindGameObjectsWithTag("Dog");
        UnityEngine.Vector3 closest = new UnityEngine.Vector3(0, 0, 0);

        foreach (GameObject dog in dogs){
            float cur_distance = Vector3.Distance(position, dog.transform.position);
            
                distance += cur_distance;
             //   Debug.Log("New clossest");
            
        }  

        float defX = (position.x - closest.x )/2;
        float defZ = (position.z - closest.z)/2;

        if (state == GhostSheepState.ghost){
            defX = -defX;
            defZ = -defZ;
        }

        steering.linear = new Vector3(clamp(defX), 0, clamp(defZ)) * agent.maxAccel;
        print(steering.linear);
        steering.linear = this.transform.parent.TransformDirection (Vector3.ClampMagnitude(steering.linear , agent.maxAccel));
    return steering;
    }

    private float clamp(float diff){
        if (diff < 0.15 && diff > -0.15){return 0;}
        if (diff < -1) {return -1;}
        if (diff > 1) {return 1;}
        return 0.2f / (diff); 
    }

}
