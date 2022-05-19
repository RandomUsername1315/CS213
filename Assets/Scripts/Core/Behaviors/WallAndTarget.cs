using System.Globalization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public enum WallTargetState
{
    wall = 0,
    target = 1
}


public class WallAndTarget : AgentBehaviour
{
    private Vector3 target;
    public WallTargetState state = 0;

    public GameManagerArrows manager;

    public void Start()
    {
        agent.isMoved = false;
        if (state == WallTargetState.wall)
        {
            target = manager.nextWallPosition();
        }
        else
        {
            target = manager.nextTargetPosition();
        }

    }
    private void Update()
    {
        //if it is withing a range then it needs to ask for a new position
        if (Vector3.Distance(transform.localPosition, target) < 10.0f)
        {

        }
    }

    public override Steering GetSteering()
    {
        Vector3 position = transform.localPosition;

        Vector3 speed = new Vector3(0, 0, 0);
        speed.x = (-position.x + target.x) / 20;
        speed.z = (-position.z + target.z) / 20;

        // steering.linear = maxedVect(speed) * agent.maxAccel * this.GetComponent<Rigidbody>().mass;


        return new Steering();

    }

    private bool rightStartPos(){
        UnityEngine.Vector3 pos = transform.localPosition;
        double distance = Math.Sqrt((pos.x - startPoint.x) * (pos.x - startPoint.x) + (pos.z - startPoint.z) * (pos.z - startPoint.z));
        return distance < maxDeltaDistance;
    }



}