using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    void OnTriggerEnter(Collider col)
    {
        switch (col.tag)
        {
            case "RedHoop":
                Score.UpdateScore(50);
                break;
            case "GreenHoop":
                Score.UpdateScore(25);
                break;
            case "BlueHoop":
                Score.UpdateScore(10);
                break;
        }
    }
}
