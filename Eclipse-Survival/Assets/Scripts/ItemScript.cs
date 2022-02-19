using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [Header("Set in Inspector")]
    //variable that will be used for affecting the player health and hunger
    public int healthRestore;
    public int hungerRestore;

    
    //Detect whether the character collision has hit the item, if true and is declared as Xander destroy the object
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Xander")
        {
            Destroy(this.gameObject);

        }      
    }
}
