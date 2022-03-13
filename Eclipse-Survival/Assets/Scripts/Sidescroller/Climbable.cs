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

    // Updated so Enemies can climb too

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Climbable")
        {
            GameObject go = this.transform.parent.gameObject;
            if (go.tag == "Xander")
            {
                ClimbingMovement c = go.GetComponent<ClimbingMovement>();
                if (c.OnClimbable) numInBetween++;
                else c.OnClimbable = true;
            }
            else if (go.tag == "ClimbingEnemy")
            {
                ClimbingEnemy e = go.GetComponent<ClimbingEnemy>();
                if (e.OnClimbable) numInBetween++;
                else e.OnClimbable = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Climbable")
        {
            GameObject go = this.transform.parent.gameObject;
            if (go.tag == "Xander")
            {
                ClimbingMovement c = go.GetComponent<ClimbingMovement>();
                if (numInBetween != 0) numInBetween--;
                else c.OnClimbable = false;
            }
            else if (go.tag == "ClimbingEnemy")
            {
                ClimbingEnemy e = go.GetComponent<ClimbingEnemy>();
                if (numInBetween != 0) numInBetween--;
                else e.OnClimbable = false;
            }
        }
    }
}
