using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static Constants;

public class Xander : MonoBehaviour
{
    public int Hunger { get; set; }

    public int Health { get; set; }

    [Header("Set in Inspector")]   
    public GameObject hungerBar;
    public GameObject healthBar;

    [Header("Set Dynamically")]
    public bool isAlive;

    private float hungerTimer;
    // Start is called before the first frame update
    void Start()
    {
        Hunger = STARTING_HUNGER;
        Health = STARTING_HEALTH;
        hungerTimer = HUNGER_DECREMENT_INTERVAL;
        isAlive = true;
        GameObject spawnPoint = GameObject.FindGameObjectWithTag(GamePlayManager.GPM.targetTag);
        gameObject.transform.position = spawnPoint.GetComponent<PerspectiveSceneChange>().spawnPoint.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hungerTimer -= Time.deltaTime;

        if (hungerTimer <= 0)
        {
            Hunger -= 1;
            if (Hunger < 0)
            {
                Hunger = 0;
                Health -= 1;
            }

            UpdateUI();
            hungerTimer = HUNGER_DECREMENT_INTERVAL;
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health >= 0)
        {
            isAlive = false;
            //Here Xander will die. Perhaps we could make the screen switch to the Game Over screen here. 
        }
    }

    void UpdateUI()
    {
        hungerBar.GetComponent<Slider>().value = (float)Hunger / STARTING_HUNGER;
        healthBar.GetComponent<Slider>().value = (float)Health / STARTING_HEALTH;
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            //Get the Script Component Value for the item
            ItemScript itemValue = collision.gameObject.GetComponent<ItemScript>();

            //Add the items determine value to the current health and hunger values
            Health += itemValue.healthRestore;
            Hunger += itemValue.hungerRestore;

            //Update the UI with changed values
            UpdateUI();
        }
    }
}
