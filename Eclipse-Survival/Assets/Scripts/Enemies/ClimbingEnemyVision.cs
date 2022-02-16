using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingEnemyVision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Climbable")
        {
            GameObject e = transform.parent.gameObject;
            ClimbingEnemy c = e.GetComponent<ClimbingEnemy>();
            c.NearbyClimbables.Add(collision.gameObject);
        }
        else if(collision.tag == "Ground")
        {
            GameObject e = transform.parent.gameObject;
            ClimbingEnemy c = e.GetComponent<ClimbingEnemy>();
            c.NearbyGround.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Climbable")
        {
            GameObject e = transform.parent.gameObject;
            ClimbingEnemy c = e.GetComponent<ClimbingEnemy>();
            c.NearbyClimbables.Remove(collision.gameObject);
        }
        else if (collision.tag == "Ground")
        {
            GameObject e = transform.parent.gameObject;
            ClimbingEnemy c = e.GetComponent<ClimbingEnemy>();
            c.NearbyGround.Remove(collision.gameObject);
        }
    }
}
