using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public bool held;
    public float orbitStrength;
    Rigidbody rb;
    public Transform currentHolder;

    void Orbit(Transform target) {
        rb.AddForce((currentHolder.position - transform.position) * orbitStrength);
    }

	// Use this for initialization
	void Start () {
		rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (held) { //if held, don't have gravity. Stay still. Stick with the SUCC. Orbit the SUCC.
            rb.useGravity = false;
            rb.velocity *= 0;
            transform.SetParent(currentHolder);
            transform.position = currentHolder.position;
            transform.rotation = currentHolder.rotation;
            transform.position += (transform.forward)/2;
            Orbit(currentHolder);
        } else { //if not held, stop being stuck to the SUCC.
            transform.SetParent(null);
        }

    }

}
