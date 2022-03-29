using System.Globalization;
using System;
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
    }
    public override Steering GetSteering()
    {
        float horizontal = 0;
        float vertical = 0;

        Steering steering = new Steering();
        UnityEngine.Vector3 position = transform.position;
        float distance = Mathf.Infinity;
        GameObject[] dogs = GameObject.FindGameObjectsWithTag("Dog");
        UnityEngine.Vector3 closest = new UnityEngine.Vector3(0, 0, 0);

        foreach (GameObject dog in dogs){
            float cur_distance = Vector3.Distance(position, dog.transform.position);
            if (cur_distance != 0 && cur_distance < distance) {
                closest = dog.transform.position;
                distance = cur_distance;
                Debug.Log("New clossest");
            }
        }  

        
        horizontal = 10 / (position.x - closest.x);
        vertical = 10 / (position.z - closest.z); 

        steering.linear = new Vector3(horizontal, 0, vertical) * agent.maxAccel;

        steering.linear = this.transform.parent.TransformDirection (Vector3.ClampMagnitude(steering.linear , agent.maxAccel));
        return steering;
    }



}
