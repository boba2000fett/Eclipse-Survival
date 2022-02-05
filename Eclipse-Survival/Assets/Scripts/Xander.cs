using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Xander : MonoBehaviour
{
    [Header("Set in Inspector")]   
    public int hunger;
    public int health;
    public float hungerDecrementInterval; // in seconds
    public Text hungerText;

    private float hungerTimer;
    // Start is called before the first frame update
    void Start()
    {
        hungerTimer = hungerDecrementInterval;
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

    void UpdateUI()
    {
        hungerText.text = $"Hunger: {hunger}";
    }
}
