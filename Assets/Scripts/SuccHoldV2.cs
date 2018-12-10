using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SuccHoldV2 : MonoBehaviour { //to be attached to a holding area at the end of the barrel of the SUCC

    public float succStrength; //should be very weak in this instance, just used to give the ball something of an animation while being held
    public float minFireStrength; //the minimum power of the shot
    public float maxFireStrength; //the maximum power of the shot
    float currentFireStrength; //the current strength of the shot
    public float fireStrengthIncrement; //the amount that the fire strength increases each frame. Should ideally be able to go into max fire strength so that numbers don't get slightly off.
    Rigidbody rb; //a rigidbody of any ball, variable
    bool sucking; //whether or not the SUCC is currently sucking
    List<GameObject> heldBalls; //list of balls that are held by the SUCC. It's easier as a list because adding is good.
    GameObject[] heldBallsArr; //(the list of balls is changed into an array at a point because it's easier to get rid of them)
    bool primed; //whether or not the ball is primed to fire
    public Transform strGauge; //the transform for the gauge that displays the current fire strength
    Vector3 strGaugeOverride; //a vector 3 that needs to take current strGauge values in order to pass them in because properties such as scale.y are private

    void Fire() { //fires any currently held balls

        heldBallsArr = heldBalls.ToArray(); //converts the list to an array

        for (int i = 0; i < heldBallsArr.Length; i++) {
            rb = heldBalls[i].GetComponent<Rigidbody>(); //gets the rididbody of the current ball in the list

            heldBalls[i].GetComponent<Ball>().held = false; //sets the ball to no longer be held

            heldBalls[i].transform.SetParent(null); //de-parents the ball from the gun so the ball can be fired

            rb.AddForce(transform.forward * currentFireStrength); //fires the ball
        }
        Array.Clear(heldBallsArr, 0, heldBallsArr.Length); //clears the array of held balls
        heldBalls.Clear(); //clears the list of held balls
    }

    // Use this for initialization
    void Start () {
        currentFireStrength = minFireStrength;

        heldBalls = new List<GameObject>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Input.GetKey("s") || Input.GetAxis("Oculus_CrossPlatform_PrimaryIndexTrigger") > 0.55) { //s for testing. 
            sucking = true;
        }
        else {
            sucking = false;
        }

        if (Input.GetKeyDown("f") || Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger") > 0.55) { //f for testing. Hold down the right trigger to charge and prime the SUCC
            if (currentFireStrength < maxFireStrength) {
                currentFireStrength += fireStrengthIncrement;
                primed = true; //prime the SUCC by holding down the trigger
            }
        }
    }

    void Update() {//for some reason the last if statement, whichever it happens to be, doesn't play nice in FixedUpdate.
        if (Input.GetKeyUp("f") || Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger") < 0.55) { //f for testing. Release the right trigger to fire the SUCC.
            if (primed) { //only fire if primed, so the SUCC doesn't just fire on idle
                Fire();
                currentFireStrength = minFireStrength;
            }
            primed = false;
        }

        //map the scaling of the strGauge to currentFireStrength's value between max and min, and make it scale in the y axis the same relative amount.
        strGaugeOverride = strGauge.localScale;
        strGaugeOverride.Set(strGaugeOverride.x, (currentFireStrength/(maxFireStrength - minFireStrength))*0.15f + 0.05f, strGaugeOverride.z);
        strGauge.localScale = strGaugeOverride;

        //do the same, but map it to a translation on the z axis rather than scaling in y.
        strGaugeOverride = strGauge.localPosition;
        strGaugeOverride.Set(strGaugeOverride.x, strGaugeOverride.y, (currentFireStrength / (maxFireStrength - minFireStrength)) * 0.15f - 0.1f);
        strGauge.localPosition = strGaugeOverride;
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
                    heldBalls.Add(ball.gameObject);
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
        }
    }
}
