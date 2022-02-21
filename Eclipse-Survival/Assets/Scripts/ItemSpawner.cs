using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Set in Inspector: Item Spawner")]
    public Waypoint[] spawnWaypoints;
    public GameObject[] foodItemsRandom;
    public GameObject[] foodItemsIndex;
    public float itemSpawnChance; //The chance percent of an Item spawning at a certain position
    public int maxItemSpawn; //The Maximum number of items to spawn in a room or a cycle
    public string roomName; //This is used to identify the current scene/room name 

    [Header("Set Dynamically:")]
    private GameObject[] foodItemBeingRemoved;
    private int itemsSpawned;
    private int selectedItemNum;
    private int itemIndexValue;

    void Start()
    {
        //A switch case for identifying the current room name, which is set in the inspector
        switch (roomName)
        {
            case "mainRoom1":
                
                //If the room hasn't been visited yet, the room will generate new random items and not the old stored item info
                if (!GamePlayManager.GPM.mainRoom1Visited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.mainRoom1Visited = true;
                }
                else
                {
                    //Verify that both the items left lists of name and positions are not length of 0
                    if (GamePlayManager.GPM.itemsLeftInMainRoom1Type.Length != 0 &&
                        GamePlayManager.GPM.itemsLeftInMainRoom1Location.Length != 0)
                    {
                        //Go through each of the array value in the room list
                        for (int i = 0; i < GamePlayManager.GPM.itemsLeftInMainRoom1Type.Length; i++)
                        {
                            //Switch statement for identifying the item name, which then sets the index value of the list of items
                            switch (GamePlayManager.GPM.itemsLeftInMainRoom1Type[i])
                            {
                                case "BigCheese":
                                    itemIndexValue = 0;
                                    break;
                                case "Bread":
                                    itemIndexValue = 1;
                                    break;

                                case "DeathCheese":
                                    itemIndexValue = 2;
                                    break;
                                case "FoodA":
                                    itemIndexValue = 3;
                                    break;
                                case "FoodB":
                                    itemIndexValue = 4;
                                    break;
                            }

                            //Reinstatiate the item object and place it based on the stored vector value
                            GameObject go = Instantiate<GameObject>(foodItemsIndex[itemIndexValue]);
                            go.transform.position =
                                GamePlayManager.GPM.itemsLeftInMainRoom1Location[i];
                        }
                    }
                }
                break;

            case "mainRoom2":

                //If the room hasn't been visited yet, the room will generate new random items and not the old stored item info
                if (!GamePlayManager.GPM.mainRoom2Visited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.mainRoom2Visited = true;
                }
                else
                {
                    //Verify that both the items left lists of name and positions are not length of 0
                    if (GamePlayManager.GPM.itemsLeftInMainRoom2Type.Length != 0 &&
                        GamePlayManager.GPM.itemsLeftInMainRoom2Location.Length != 0)
                    {
                        //Go through each of the array value in the room list
                        for (int i = 0; i < GamePlayManager.GPM.itemsLeftInMainRoom2Type.Length; i++)
                        {
                            //Switch statement for identifying the item name, which then sets the index value of the list of items
                            switch (GamePlayManager.GPM.itemsLeftInMainRoom2Type[i])
                            {
                                case "BigCheese":
                                    itemIndexValue = 0;
                                    break;
                                case "Bread":
                                    itemIndexValue = 1;
                                    break;

                                case "DeathCheese":
                                    itemIndexValue = 2;
                                    break;
                                case "FoodA":
                                    itemIndexValue = 3;
                                    break;
                                case "FoodB":
                                    itemIndexValue = 4;
                                    break;
                            }

                            //Reinstatiate the item object and place it based on the stored vector value
                            GameObject go = Instantiate<GameObject>(foodItemsIndex[itemIndexValue]);
                            go.transform.position =
                                GamePlayManager.GPM.itemsLeftInMainRoom2Location[i];
                        }
                    }
                }
                break;
        }  
    }

    //This update method is in place for reseting the items in a room in which the player is located
    void Update()
    {
        //A switch case for identifying the current room name, which is set in the inspector
        //This then checks for when the bool value for the room changes
        switch (roomName)
        {
            case "mainRoom1":

                if (!GamePlayManager.GPM.mainRoom1Visited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.mainRoom1Visited = true;
                }
                break;

            case "mainRoom2":

                if (!GamePlayManager.GPM.mainRoom2Visited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.mainRoom2Visited = true;
                }
                break;
        }
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
                selectedItemNum = Random.Range(0, foodItemsRandom.Length);

                //Create an new instance of the determine item
                GameObject go = Instantiate<GameObject>(foodItemsRandom[selectedItemNum]);

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
