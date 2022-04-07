using System.Globalization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum GhostSheepState{
    ghost = 0, 
    sheep = 1
    }   

public class GhostSheepBehavior : AgentBehaviour
{    
    

    private GhostSheepState state;
    public void Start(){
        state = GhostSheepState.sheep;
        agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.blue, 0);
        Invoke("changeState", Random.Range(5, 10));
    }

    private void changeState() {
        if (state == GhostSheepState.sheep){
            state = GhostSheepState.ghost;
            agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.red, 0);
        } else {
            state = GhostSheepState.sheep;
            agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.blue, 0);
            Invoke("changeState", Random.Range(5, 15));
        }
    }

    public GhostSheepState getState(){
        return state;
    }
    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        UnityEngine.Vector3 position = transform.position;
        GameObject dog = closestDog();

        Vector3 speed = new Vector3(0, 0, 0); 
        if (state == GhostSheepState.ghost){
            speed.x = (-position.x + dog.transform.position.x);
            speed.z = (-position.z + dog.transform.position.z);
        }
        else{
            speed = getAverage();
        }
        speed = Vector3.ClampMagnitude(speed, 1);
        steering.linear = speed * agent.maxAccel;
    return steering;
    }

    public UnityEngine.Vector3 getAverage(){
        GameObject[] dogs = GameObject.FindGameObjectsWithTag("Dog");
        UnityEngine.Vector3 average = new UnityEngine.Vector3(0, 0, 0);

        foreach (GameObject dog in dogs)
        {
            float cur_distance = Vector3.Distance(transform.position, dog.transform.position);
            if(cur_distance < 7){
                print(average);
                average += transform.position-dog.transform.position;
            }
        }
     
        return average / dogs.Length;
    }

    public GameObject closestDog(){
        GameObject[] dogs = GameObject.FindGameObjectsWithTag("Dog");
        UnityEngine.Vector3 position = transform.position;
        float distance = Mathf.Infinity;

        GameObject closest = dogs[0];

        foreach (GameObject dog in dogs){
            float cur_distance = Vector3.Distance(position, dog.transform.position);
            if (cur_distance < distance) {
                closest = dog;
                distance = cur_distance;
            }
        }

        return closest;
    }
    private float clamp(float val){
        return (val > 3) ? 3f : ((val<-3) ? -3f : val);
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Dog" && this.state == 0){
            other.gameObject.GetComponent<MoveWithKeyboardBehavior>().incrementScore(-1);
            changeState();
        }
    }

}
