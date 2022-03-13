using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{    
    [HideInInspector]
    [Tooltip("For the Frying Pan Object, this is set by the Grandmother class, via the fryingPanStrength variable")]
    public int strength;

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Xander")
    //    {
    //        Xander xander = collision.gameObject.GetComponent<Xander>();
    //        xander.TakeDamage(strength);
    //        //Maybe Knock Xander Back?
    //    }
    //    else if (collision.gameObject.tag == "Cockroach")
    //    {
    //        collision.gameObject.GetComponent<Enemy>().TakeDamage(strength);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Xander")
        {
            Xander xander = collision.gameObject.GetComponent<Xander>();
            xander.TakeDamage(strength);
            //Maybe Knock Xander Back?
        }
        else if (collision.gameObject.tag == "Cockroach")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(strength);
        }
    }
}
