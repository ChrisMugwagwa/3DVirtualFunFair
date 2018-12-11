using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject mole;
    public float MinX = -6.4f;
    public float MaxX = 5.32f;
    public float MinZ = -11f;
    public float MaxZ = 14f;

    void Start(){
   
    }

    void Update(){

        float x = Random.Range(MinX, MaxX);
        float y = -.4f;
        float z = Random.Range(MinZ, MaxZ);

        if (Time.fixedTime % 4 == 0)
        {
            Instantiate(mole, new Vector3(x, y, z), Quaternion.Euler(-90,0,0));
        }
    }
       
}