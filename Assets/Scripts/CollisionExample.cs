using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void OnCollisionEnter(Collision other) {
        print(gameObject.name + " and " + other.gameObject.name + " have a new collision");
        print("There is " + other.contactCount + " differents contacts points for this collision.");
        print("The impact velocity is " + other.relativeVelocity + ".");
    }

    void OnCollisionExit(Collision other) {
        print("The collision between " + gameObject.name + " and " + other.gameObject.name + " has ended.");
    }

    void OnCollisionStay(Collision other) {
       print("The collision between " + gameObject.name + " and " + other.gameObject.name + " is still ongoing"); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
