using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Input Keys
public enum InputKeyboard{
    arrows =0,
    wasd = 1
}
public class MoveWithKeyboardBehavior : AgentBehaviour
{
    public InputKeyboard inputKeyboard; 
    //Physical reference of the score
    public Text score;
    public int points = 0;
    public string name = "PlayerX";
    public int getScore(){
        return points; 
    }

    public void resetScore(){
        points = 0;
    }

    public void incrementScore(int val){
        points += val;

    }

    public void Start(){
        points = 0;
        score.text = string.Format("{0,3}", points);
    }
    public void Update() {
        score.text = string.Format("{0,3}", points);
    }
    
    public override Steering GetSteering()
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
    }

}
