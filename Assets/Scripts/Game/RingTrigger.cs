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

        if (other.CompareTag("Sheep") && sheep.GetComponent<GhostSheepBehavior>().getState() == GhostSheepState.sheep)
        {
            GameObject closest = sheep.GetComponent<GhostSheepBehavior>().closestDog();

            closest.GetComponent<MoveWithKeyboardBehavior>().incrementScore(1);

        }
    }
}