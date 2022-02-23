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

    public float hungerTimer;
    // Start is called before the first frame update
    void Start()
    {
        Hunger = GamePlayManager.GPM.XanderHunger;
        Health = GamePlayManager.GPM.XanderHealth;
        hungerTimer = GamePlayManager.GPM.hungerTimer;
        isAlive = true;
        GameObject spawnPoint = GameObject.FindGameObjectWithTag(GamePlayManager.GPM.targetTag);
        gameObject.transform.position = spawnPoint.GetComponent<PerspectiveSceneChange>().spawnPoint.transform.position;
        UpdateUI();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hungerTimer -= Time.deltaTime;

        GamePlayManager.GPM.hungerTimer = hungerTimer;

        if (hungerTimer <= 0)
        {
            Hunger -= 1;
            if (Hunger < 0)
            {
                Hunger = 0;
                Health -= 1;
            }

            UpdateManager();
            UpdateUI();
            hungerTimer = HUNGER_DECREMENT_INTERVAL;
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            isAlive = false;
            GamePlayManager.GPM.EndGame();
        }
        UpdateManager();
        UpdateUI();
    }

    void UpdateManager()
    {
        GamePlayManager.GPM.XanderHealth = Health;
        GamePlayManager.GPM.XanderHunger = Hunger;
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

            if (itemValue.isDamage)
            {
                TakeDamage(itemValue.healthValue);
            }
            else
            {
                Health += itemValue.healthValue;
            }
            Hunger += itemValue.hungerValue;

            UpdateManager();
            //Update the UI with changed values
            UpdateUI();
        }
    }
}
