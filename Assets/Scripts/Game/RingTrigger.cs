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

        if (other.CompareTag("Sheep") && sheep.getState == 1)
        {
            float distance = Mathf.Infinity;
            GameObject[] dogs = GameObject.FindGameObjectsWithTag("Dog");
            GameObject closest = dogs[0];

            foreach (GameObject dog in dogs)
            {
                float cur_distance = Vector3.Distance(sheep.transform.position, dog.transform.position);

                if (cur_distance < distance)
                {
                    closest = dog;
                }
            }

            closest.incrementScore(1.0);

        }
    }
}