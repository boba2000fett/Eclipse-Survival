using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [Header("Set in Inspector")]
    //variable that will be used for affecting the player health and hunger
    public float health;
    public float hunger;

    
    //Detect whether the character collision has hit the item, if true and is declared as Xander destroy the object
    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Xander")
        {
            Destroy(this.gameObject);

            //Add the items determine value to the current health and hunger values
            //This is currently set to change static variables defined in the game manager
            GamePlayManager.healthAdded += health;
            GamePlayManager.hungerRestored += hunger;

        }
    }
}
