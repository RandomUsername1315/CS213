using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
           
        GameObject sheep = GameObject.FindGameObjectsWithTag("Sheep")[0];
        // print("Green circle Triggered : " + other.attachedRigidbody.name);

        if (other.attachedRigidbody.tag =="Sheep" && sheep.GetComponent<GhostSheepBehavior>().getState() == GhostSheepState.sheep)
        {
            GameObject closest = sheep.GetComponent<GhostSheepBehavior>().closestDog();
           // print("Closest is " + closest.ToString() );
            closest.GetComponent<MoveWithKeyboardBehavior>().incrementScore(1);

        }
    }
}