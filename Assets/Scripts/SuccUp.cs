using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccUp : MonoBehaviour {

    public float succStrength;
    public Transform gunTransform;
    Vector3 towardGunVector;
    Rigidbody rb;
    bool sucking;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("s") || Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger") > 0.5) { //s for now just for testing
            sucking = true;
        }
        else {
            sucking = false;
        }
    }

    private void OnTriggerEnter(Collider ball) {
        if (ball.gameObject.tag.Contains("ball") && sucking == true) { //shouldn't hold anything but balls
            //get the rigidbody of the ball
            rb = ball.gameObject.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerStay(Collider ball) {
        if (ball.gameObject.tag.Contains("ball") && sucking == true) { //shouldn't suck up anything that shouldn't be

            //get a vector going from the ball to the gun, then normalize it
            towardGunVector = Vector3.Normalize(gunTransform.position - ball.gameObject.transform.position);

            //get the rigidbody of the ball
            rb = ball.gameObject.GetComponent<Rigidbody>();

            //turn off gravity on the ball for easier holding. Could go in OnTriggerEnter but doesn't play nice while entering one SUCC hitbox before leaving another.
            rb.useGravity = false;

            //force the ball toward the gun
            rb.AddForce(towardGunVector * succStrength);
        }
        if (ball.gameObject.tag.Contains("ball") && sucking == false) { //if the gun isn't sucking then the ball should fall
            rb.useGravity = true;
        }
    }

    private void OnTriggerExit(Collider ball) {
        if (ball.gameObject.tag.Contains("ball")) { //shouldn't hold anything but balls
            //get the rigidbody of the ball
            rb = ball.gameObject.GetComponent<Rigidbody>();
            //turn gravity back on on the ball once it leaves the SUCC
            rb.useGravity = true;
        }
    }
}
