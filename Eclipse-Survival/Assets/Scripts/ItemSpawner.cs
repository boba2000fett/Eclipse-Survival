using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Set in Inspector: Item Spawner")]
    public Waypoint[] spawnWaypoints;
    public GameObject[] foodItems;
    public float itemSpawnChance; //The chance percent of an Item spawning at a certain position
    public int maxItemSpawn; //The Maximum number of items to spawn in a room or a cycle

    [Header("Set Dynamically:")]
    private GameObject[] foodItemBeingRemoved;
    private int itemsSpawned;
    private int selectedItemNum;

    public void Start()
    {
        GameObject tempOBJ = GameObject.Find("ItemSpawnsWaypoint");

        Transform tempHold = tempOBJ.transform;

        //spawnWaypoints = new Waypoint[3];

        //for (int i = 0; i < tempOBJ.transform.childCount; i++)
        //{

        //}


        //if (GamePlayManager.GPM.mainRoom1)
        //{
        //    SpawnItems();
        //    GamePlayManager.GPM.mainRoom1 = false;
        //}
    }


    public void SpawnItems()
    {
        //If any Items not collected are currently still in the scene,
        //Destroy it
        RemoveItems();

        //Restet the Item Spawned Counter
        itemsSpawned = 0;

        //Go through each of the waypoints in the room and determine if an item will spawn
        foreach (Waypoint w in spawnWaypoints)
        {
            //If the random variable allows for an Item to Spawn at the Waypoint,
            //Then spawn the item. If not, do not spawn item
            if (Random.value <= itemSpawnChance && itemsSpawned < maxItemSpawn)
            {
                //Select a Random Item from the provided list of items
                selectedItemNum = Random.Range(0, foodItems.Length);

                //Create an new instance of the determine item
                GameObject go = Instantiate<GameObject>(foodItems[selectedItemNum]);

                //Set the position of the item to the waypoint position
                go.transform.position = w.transform.position;

                //Add 1 to the item counter
                itemsSpawned++;
            }            
        }
    }

    void RemoveItems()
    {
        //Find all food/item game object within the scene
        foodItemBeingRemoved = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject go in foodItemBeingRemoved)
        {
            Destroy(go);
        }
    }
}
