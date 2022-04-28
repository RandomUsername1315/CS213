        
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    private int X;


    // Start is called before the first frame update
    void Start()
    {
        X = 10;
    }

    public void Collected(){
        MeshRenderer mr = this.GetComponent<MeshRenderer>();
        mr.enabled = false;
    }
        
    public void GemCollision(){
        MeshRenderer mr = this.GetComponent<MeshRenderer>();
        mr.enabled = true;
        this.transform.position = new Vector3(Random.Range(2, 12.0f),0.0f,Random.Range(-2.0f,-8.0f));
    }
    void OnTriggerEnter(Collider other)
    {    
        MeshRenderer mr = this.GetComponent<MeshRenderer>();
       
        if(other.attachedRigidbody.tag =="Dog" && mr.enabled){
                if (PlayerPrefs.GetInt("VolumeEnable", 1) == 1){
                    this.GetComponentInParent<AudioSource>().volume = PlayerPrefs.GetFloat("Volume", this.GetComponentInParent<AudioSource>().volume);
                    this.GetComponentInParent<AudioSource>().Play(0);
                }
                Invoke("GemCollision", X);
                other.attachedRigidbody.GetComponent<MoveWithKeyboardBehavior>().gemBonus = true;
                this.Collected();
            }
    
    }
}
