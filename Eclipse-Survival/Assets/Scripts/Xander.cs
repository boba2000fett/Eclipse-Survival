using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Xander : MonoBehaviour
{
    [Header("Set in Inspector")]   
    public int hunger;
    public float health;
    public float hungerDecrementInterval; // in seconds
    public Text hungerText;

    [Header("Set Dynamically")]
    public bool isAlive;

    private float hungerTimer;
    // Start is called before the first frame update
    void Start()
    {
        hungerTimer = hungerDecrementInterval;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        hungerTimer -= Time.deltaTime;

        if (hungerTimer <= 0)
        {
            hunger -= 1;
            if (hunger < 0)
            {
                hunger = 0;
            }

            UpdateUI();
            hungerTimer = hungerDecrementInterval;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health >= 0)
        {
            isAlive = false;
            //Here Xander will die. Perhaps we could make the screen switch to the Game Over screen here. 
        }
    }

    void UpdateUI()
    {
        hungerText.text = $"Hunger: {hunger}";
    }
}
