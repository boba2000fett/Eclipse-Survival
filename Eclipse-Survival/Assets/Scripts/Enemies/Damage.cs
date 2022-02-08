using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float strength;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Xander")
        {
            Xander xander = collision.gameObject.GetComponent<Xander>();
            xander.TakeDamage(strength);
            //Maybe Knock Xander Back?
        }
    }
}
