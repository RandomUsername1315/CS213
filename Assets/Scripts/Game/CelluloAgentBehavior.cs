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
            this.GetComponent<GameManager>().startGame();
        }
    }
}