using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour
{
    [Header("Set Dynamically")]
    private int numInBetween;

    private void Start()
    {
        numInBetween = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Climbable")
        {
            GameObject x = GameObject.FindGameObjectWithTag("Xander");
            ClimbingMovement c = x.GetComponent<ClimbingMovement>();
            if (c.OnClimbable) numInBetween++;
            else c.OnClimbable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Climbable")
        {
            GameObject x = GameObject.FindGameObjectWithTag("Xander");
            ClimbingMovement c = x.GetComponent<ClimbingMovement>();
            if(numInBetween != 0) numInBetween--;
            else c.OnClimbable = false;
        }
    }
}
