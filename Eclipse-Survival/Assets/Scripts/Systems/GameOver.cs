using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    void Start()
    {
        GameObject score = GameObject.Find("Final Score");

        int r = GamePlayManager.GPM.currentNumberOfCycleSurvived;

        Text txtScore = score.GetComponent<Text>();

        txtScore.text = "Score: "+r+" Days\nHigh Score: "+r+" Days";
    }

}
