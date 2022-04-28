using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelluloAgentBehavior : AgentBehaviour
{
    private static AgentBehaviour ready = null;
    
    public override void OnCelluloLongTouch(int val){
        if (ready == null){
            ready = this;
            return;
        }
        if (ready != this){
            ready = null;
            if (!GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().isGameRunning() 
             && !GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().gameIsOver() ){
                this.GetComponent<GameManager>().startGame();
            }
            
        }
    }
}