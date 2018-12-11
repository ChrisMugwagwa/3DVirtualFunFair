using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class jump : MonoBehaviour {

    public Rigidbody rb;
    public float height;

    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * height);
	}
	
	// Update is called once per frame
	void Update () {
  
    }
}
