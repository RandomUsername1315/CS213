using System.Globalization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;

public enum GhostSheepState{
    ghost = 0,
    sheep = 1,
    paused = 2
    }   

public class GhostSheepBehavior : AgentBehaviour
{    

    private GhostSheepState state;

    public void Start(){
        state = GhostSheepState.sheep;
        agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.blue, 0);
        Invoke("changeState", Random.Range(10, 20));
    
    }

    private void changeState() {
        // When sheep, just go to ghost
        if (state == GhostSheepState.sheep){
            state = GhostSheepState.ghost;
            agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.red, 0); 
          //  agent._celluloRobot.SetVisualEffect(1, 255, 0, 0, 0); 
            hardMode();
            Invoke("changeState", Random.Range(10, 20));
        } else {
        // When ghost, just go to sheep
            state = GhostSheepState.sheep;
            agent.SetVisualEffect(VisualEffect.VisualEffectConstAll, Color.blue, 0);
            easyMode();
            Invoke("changeState", Random.Range(10, 20));
        }
    }

    private void hardMode(){
       // GameObject cells = GameObject.FindGameObjectsWithTag("Cellulos");
        Object[] cells = GameObject.FindObjectsOfType(typeof(CelluloAgent));

        foreach(GameObject cell in cells){
            if (cell.tag == "Dog"){
                cell.GetComponent<CelluloAgent>().SetCasualBackdriveAssistEnabled(false);
                cell.GetComponent<CelluloAgent>().MoveOnStone();
            }
        }
    }

    private void easyMode(){
       // GameObject cells = GameObject.FindGameObjectsWithTag("Cellulos");
        Object[] cells = GameObject.FindObjectsOfType(typeof(CelluloAgent));

        foreach(GameObject cell in cells){
            if (cell.tag == "Dog"){
                cell.GetComponent<CelluloAgent>().SetCasualBackdriveAssistEnabled(true);
                cell.GetComponent<CelluloAgent>().MoveOnSandpaper();

            }
        }
    }

    public GhostSheepState getState(){
        return state;
    }
    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        if (!GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().isGameRunning()){
            CancelInvoke();
            steering.linear = Vector3.zero;
            steering.angular = 0f;
            return steering;
        }
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
              //  print(average);
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
        if(GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().isGameRunning() 
           && other.gameObject.tag == "Dog" && this.state == GhostSheepState.ghost){
            other.gameObject.GetComponent<MoveWithKeyboardBehavior>().incrementScore(-1);
            // Avoids having an invoke poping at bad moment
            CancelInvoke("changeState");
            changeState();
        }
    }

    void StartGame(){
        
    }

}
