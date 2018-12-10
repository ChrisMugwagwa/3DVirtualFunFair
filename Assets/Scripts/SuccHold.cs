﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccHold : MonoBehaviour { //to be attached to a holding area at the end of the barrel of the SUCC

    public float succStrength; //should be very weak in this instance, just used to give the ball something of an animation while being held
    public float fireStrength;
    Rigidbody rb;
    bool sucking;
    bool primed;
    public bool rightHand; //whether or not the 1-handed succ gun is the left hand or right hand version

    void Fire() { //fires any currently held balls

        //get the rigidbody of any held balls. Only balls, or anything else that can be fired (tagged as ball), should be children of this game object.
        rb = gameObject.GetComponentInChildren<Rigidbody>(); //only balls will be children so this will grab their rigidbodies

        rb.gameObject.GetComponent<Ball>().held = false; //stops the ball from being held

        rb.AddForce(transform.forward * fireStrength); //fires the ball
    }

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (rightHand) { 
            if (Input.GetKeyUp("f") || Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger") < 0.35) { //f for now just for testing
                if (primed) {
                    Fire();
                }
                primed = false;
            }
            if (Input.GetKey("f") || Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger") > 0.55) { //g for now just for testing
                sucking = true;
                primed = true;
            }
            else {
                sucking = false;
            }
        }
        else {
            if (Input.GetKeyUp("f") || Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger") < 0.35) { //f for now just for testing
                if (primed) {
                    Fire();
                }
                primed = false;
            }
            if (Input.GetKey("f") || Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger") > 0.55) { //g for now just for testing
                sucking = true;
                primed = true;
            }
            else {
                sucking = false;
            }
        }
    }

    private void OnTriggerStay(Collider ball) {
        if (ball.gameObject.tag.Contains("ball") ) { //shouldn't suck up anything that shouldn't be

            //get the rigidbody of the ball
            rb = ball.gameObject.GetComponent<Rigidbody>();

            //turn off gravity on the ball for easier holding. Could go in OnTriggerEnter but doesn't play nice while entering one SUCC hitbox before leaving another.
            rb.useGravity = false;

            if (sucking == true) {
                //neutralise the ball's velocity so the sucking power of the SUCC doesn't just send it flying out the back of the gun
                rb.velocity *= 0;

                if (ball.gameObject.GetComponent<Ball>().held == false) { //if the ball isn't held, make it held
                    ball.gameObject.GetComponent<Ball>().held = true;
                    ball.gameObject.GetComponent<Ball>().currentHolder = transform;
                }
            }
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
