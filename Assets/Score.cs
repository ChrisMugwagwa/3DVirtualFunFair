using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    static Text scoreText;
    static int currentScore = 0;

	void Start () {
        scoreText = GameObject.FindGameObjectWithTag("ScoreBoard").GetComponent<Text>();
        UpdateScore(currentScore);
	}

    public static void UpdateScore(int addedValue)
    {
        currentScore += addedValue;
        scoreText.text = "" + currentScore;
    }
}
