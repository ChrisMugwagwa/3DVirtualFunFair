using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccHold : MonoBehaviour { //to be attached to a holding area at the end of the barrel of the SUCC

    public float succStrength; //should be very weak in this instance, just used to give the ball something of an animation while being held
    public float fireStrength;
    Vector3 towardCentreVector;
    Rigidbody rb;
    bool sucking;

    void Fire() { //fires any currently held balls

        //get the rigidbody of any held balls. Only balls, or anything else that can be fired (tagged as ball), should be children of this game object.
        rb = gameObject.GetComponentInChildren<Rigidbody>(); //only balls will be children so this will grab their rigidbodies

        rb.gameObject.transform.SetParent(null); //de-parents the gun so the ball can be fired

        rb.AddForce(transform.forward * fireStrength); //fires the ball
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("f") || Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger") > 0.5) { //f for now just for testing
            Fire();
        }
        if (Input.GetKey("s") || Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger") > 0.5) { //s for now just for testing
            sucking = true;
        } else {
            sucking = false;
        }
    }

    private void OnTriggerEnter(Collider ball) {
        if (ball.gameObject.tag.Contains("ball") && sucking == true) { //shouldn't hold anything but balls

            //get the rigidbody of the ball
            rb = ball.gameObject.GetComponent<Rigidbody>();

            //makes the ball a child of the holding box so that it moves along with the gun until it is fired.
            ball.gameObject.transform.SetParent(transform);

            //turn off gravity on the ball for easier holding. Could go in OnTriggerEnter but doesn't play nice while entering one SUCC hitbox before leaving another.
            rb.useGravity = false;

            if (sucking == true) {
                //neutralise the ball's velocity so the sucking power of the SUCC doesn't just send it flying out the back of the gun
                rb.velocity *= 0;
            }
        }
    }

    private void OnTriggerStay(Collider ball) {
        if (ball.gameObject.tag.Contains("ball") ) { //shouldn't suck up anything that shouldn't be

            //get a vector going from the ball to the gun, then normalize it
            towardCentreVector = Vector3.Normalize(transform.position + GetComponent<BoxCollider>().center - ball.gameObject.transform.position);

            //get the rigidbody of the ball
            rb = ball.gameObject.GetComponent<Rigidbody>();

            //turn off gravity on the ball for easier holding. Could go in OnTriggerEnter but doesn't play nice while entering one SUCC hitbox before leaving another.
            rb.useGravity = false;

            if (sucking == true) {
                //neutralise the ball's velocity so the sucking power of the SUCC doesn't just send it flying out the back of the gun
                rb.velocity *= 0;
            }

            //force the ball toward the gun
            //rb.AddForce(towardCentreVector * succStrength);
        }
    }

    private void OnTriggerExit(Collider ball) {
        if (ball.gameObject.tag.Contains("ball")) { //shouldn't hold anything but balls
            //get the rigidbody of the ball
            rb = ball.gameObject.GetComponent<Rigidbody>();
            //turn gravity back on on the ball once it leaves the SUCC
            rb.useGravity = true;
            rb.gameObject.transform.SetParent(null); //de-parents the gun so no oofs
        }
    }
}
