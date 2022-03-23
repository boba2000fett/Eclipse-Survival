using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Constants;
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
            case "MasterBedroom":
                
                //If the room hasn't been visited yet, the room will generate new random items and not the old stored item info
                if (!GamePlayManager.GPM.MasterBedRoomVisited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.MasterBedRoomVisited = true;
                }
                else
                {
                    //Verify that both the items left lists of name and positions are not length of 0
                    if (GamePlayManager.GPM.itemsLeftInMasterBedRoomType.Length != 0 &&
                        GamePlayManager.GPM.itemsLeftInMasterBedRoomLocation.Length != 0)
                    {
                        //Go through each of the array value in the room list
                        for (int i = 0; i < GamePlayManager.GPM.itemsLeftInMasterBedRoomType.Length; i++)
                        {
                            //Switch statement for identifying the item name, which then sets the index value of the list of items
                            switch (GamePlayManager.GPM.itemsLeftInMasterBedRoomType[i])
                            {
                                case BIG_CHEESE:
                                    itemIndexValue = 0;
                                    break;
                                case BREAD:
                                    itemIndexValue = 1;
                                    break;

                                case DEATH_CHEESE:
                                    itemIndexValue = 2;
                                    break;
                                case FRUIT_SEED:
                                    itemIndexValue = 3;
                                    break;
                                case BREAD_CRUMB:
                                    itemIndexValue = 4;
                                    break;
                            }

                            //Reinstatiate the item object and place it based on the stored vector value
                            GameObject go = Instantiate<GameObject>(foodItemsIndex[itemIndexValue]);
                            go.transform.position =
                                GamePlayManager.GPM.itemsLeftInMasterBedRoomLocation[i];
                        }
                    }
                }
                break;

            case "Kitchen":

                //If the room hasn't been visited yet, the room will generate new random items and not the old stored item info
                if (!GamePlayManager.GPM.KitchenRoomVisited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.KitchenRoomVisited = true;
                }
                else
                {
                    //Verify that both the items left lists of name and positions are not length of 0
                    if (GamePlayManager.GPM.itemsLeftInKitchenRoomType.Length != 0 &&
                        GamePlayManager.GPM.itemsLeftInKitchenRoomLocation.Length != 0)
                    {
                        //Go through each of the array value in the room list
                        for (int i = 0; i < GamePlayManager.GPM.itemsLeftInKitchenRoomType.Length; i++)
                        {
                            //Switch statement for identifying the item name, which then sets the index value of the list of items
                            switch (GamePlayManager.GPM.itemsLeftInKitchenRoomType[i])
                            {
                                case BIG_CHEESE:
                                    itemIndexValue = 0;
                                    break;
                                case BREAD:
                                    itemIndexValue = 1;
                                    break;

                                case DEATH_CHEESE:
                                    itemIndexValue = 2;
                                    break;
                                case FRUIT_SEED:
                                    itemIndexValue = 3;
                                    break;
                                case BREAD_CRUMB:
                                    itemIndexValue = 4;
                                    break;
                            }

                            //Reinstatiate the item object and place it based on the stored vector value
                            GameObject go = Instantiate<GameObject>(foodItemsIndex[itemIndexValue]);
                            go.transform.position =
                                GamePlayManager.GPM.itemsLeftInKitchenRoomLocation[i];
                        }
                    }
                }
                break;

            case "DiningRoom":
                if (!GamePlayManager.GPM.DinningRoomVisited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.DinningRoomVisited = true;
                }
                else
                {
                    //Verify that both the items left lists of name and positions are not length of 0
                    if (GamePlayManager.GPM.itemsLeftInDinningRoomType.Length != 0 &&
                        GamePlayManager.GPM.itemsLeftInDinningRoomLocation.Length != 0)
                    {
                        //Go through each of the array value in the room list
                        for (int i = 0; i < GamePlayManager.GPM.itemsLeftInDinningRoomType.Length; i++)
                        {
                            //Switch statement for identifying the item name, which then sets the index value of the list of items
                            switch (GamePlayManager.GPM.itemsLeftInDinningRoomType[i])
                            {
                                case BIG_CHEESE:
                                    itemIndexValue = 0;
                                    break;
                                case BREAD:
                                    itemIndexValue = 1;
                                    break;

                                case DEATH_CHEESE:
                                    itemIndexValue = 2;
                                    break;
                                case FRUIT_SEED:
                                    itemIndexValue = 3;
                                    break;
                                case BREAD_CRUMB:
                                    itemIndexValue = 4;
                                    break;
                            }

                            //Reinstatiate the item object and place it based on the stored vector value
                            GameObject go = Instantiate<GameObject>(foodItemsIndex[itemIndexValue]);
                            go.transform.position =
                                GamePlayManager.GPM.itemsLeftInDinningRoomLocation[i];
                        }
                    }
                }
                break;

            case "LivingRoom":
                if (!GamePlayManager.GPM.LivingRoomVisited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.LivingRoomVisited = true;
                }
                else
                {
                    //Verify that both the items left lists of name and positions are not length of 0
                    if (GamePlayManager.GPM.itemsLeftInLivingRoomType.Length != 0 &&
                        GamePlayManager.GPM.itemsLeftInLivingRoomLocation.Length != 0)
                    {
                        //Go through each of the array value in the room list
                        for (int i = 0; i < GamePlayManager.GPM.itemsLeftInLivingRoomType.Length; i++)
                        {
                            //Switch statement for identifying the item name, which then sets the index value of the list of items
                            switch (GamePlayManager.GPM.itemsLeftInLivingRoomType[i])
                            {
                                case BIG_CHEESE:
                                    itemIndexValue = 0;
                                    break;
                                case BREAD:
                                    itemIndexValue = 1;
                                    break;

                                case DEATH_CHEESE:
                                    itemIndexValue = 2;
                                    break;
                                case FRUIT_SEED:
                                    itemIndexValue = 3;
                                    break;
                                case BREAD_CRUMB:
                                    itemIndexValue = 4;
                                    break;
                            }

                            //Reinstatiate the item object and place it based on the stored vector value
                            GameObject go = Instantiate<GameObject>(foodItemsIndex[itemIndexValue]);
                            go.transform.position =
                                GamePlayManager.GPM.itemsLeftInLivingRoomLocation[i];
                        }
                    }
                }
                break;

            case "FirstFloorHallway":
                if (!GamePlayManager.GPM.FirstFloorHallwayVisited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.FirstFloorHallwayVisited = true;
                }
                else
                {
                    //Verify that both the items left lists of name and positions are not length of 0
                    if (GamePlayManager.GPM.itemsLeftInFirstFloorHallwayType.Length != 0 &&
                        GamePlayManager.GPM.itemsLeftInFirstFloorHallwayLocation.Length != 0)
                    {
                        //Go through each of the array value in the room list
                        for (int i = 0; i < GamePlayManager.GPM.itemsLeftInFirstFloorHallwayType.Length; i++)
                        {
                            //Switch statement for identifying the item name, which then sets the index value of the list of items
                            switch (GamePlayManager.GPM.itemsLeftInFirstFloorHallwayType[i])
                            {
                                case BIG_CHEESE:
                                    itemIndexValue = 0;
                                    break;
                                case BREAD:
                                    itemIndexValue = 1;
                                    break;

                                case DEATH_CHEESE:
                                    itemIndexValue = 2;
                                    break;
                                case FRUIT_SEED:
                                    itemIndexValue = 3;
                                    break;
                                case BREAD_CRUMB:
                                    itemIndexValue = 4;
                                    break;
                            }

                            //Reinstatiate the item object and place it based on the stored vector value
                            GameObject go = Instantiate<GameObject>(foodItemsIndex[itemIndexValue]);
                            go.transform.position =
                                GamePlayManager.GPM.itemsLeftInFirstFloorHallwayLocation[i];
                        }
                    }
                }
                break;

            case "GrandKidsBedRoom":
                if (!GamePlayManager.GPM.GrandKidsBedroomVisited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.GrandKidsBedroomVisited = true;
                }
                else
                {
                    //Verify that both the items left lists of name and positions are not length of 0
                    if (GamePlayManager.GPM.itemsLeftInGrandKidsBedRoomType.Length != 0 &&
                        GamePlayManager.GPM.itemsLeftInGrandKidsBedRoomLocation.Length != 0)
                    {
                        //Go through each of the array value in the room list
                        for (int i = 0; i < GamePlayManager.GPM.itemsLeftInGrandKidsBedRoomType.Length; i++)
                        {
                            //Switch statement for identifying the item name, which then sets the index value of the list of items
                            switch (GamePlayManager.GPM.itemsLeftInGrandKidsBedRoomType[i])
                            {
                                case BIG_CHEESE:
                                    itemIndexValue = 0;
                                    break;
                                case BREAD:
                                    itemIndexValue = 1;
                                    break;

                                case DEATH_CHEESE:
                                    itemIndexValue = 2;
                                    break;
                                case FRUIT_SEED:
                                    itemIndexValue = 3;
                                    break;
                                case BREAD_CRUMB:
                                    itemIndexValue = 4;
                                    break;
                            }

                            //Reinstatiate the item object and place it based on the stored vector value
                            GameObject go = Instantiate<GameObject>(foodItemsIndex[itemIndexValue]);
                            go.transform.position =
                                GamePlayManager.GPM.itemsLeftInGrandKidsBedRoomLocation[i];
                        }
                    }
                }
                break;

            case "GuestBedRoom1":
                if (!GamePlayManager.GPM.GuestBedRoom1Visited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.GuestBedRoom1Visited = true;
                }
                else
                {
                    //Verify that both the items left lists of name and positions are not length of 0
                    if (GamePlayManager.GPM.itemsLeftInGuestBedRoomOneType.Length != 0 &&
                        GamePlayManager.GPM.itemsLeftInGuestBedRoomOneLocation.Length != 0)
                    {
                        //Go through each of the array value in the room list
                        for (int i = 0; i < GamePlayManager.GPM.itemsLeftInGuestBedRoomOneType.Length; i++)
                        {
                            //Switch statement for identifying the item name, which then sets the index value of the list of items
                            switch (GamePlayManager.GPM.itemsLeftInGuestBedRoomOneType[i])
                            {
                                case BIG_CHEESE:
                                    itemIndexValue = 0;
                                    break;
                                case BREAD:
                                    itemIndexValue = 1;
                                    break;

                                case DEATH_CHEESE:
                                    itemIndexValue = 2;
                                    break;
                                case FRUIT_SEED:
                                    itemIndexValue = 3;
                                    break;
                                case BREAD_CRUMB:
                                    itemIndexValue = 4;
                                    break;
                            }

                            //Reinstatiate the item object and place it based on the stored vector value
                            GameObject go = Instantiate<GameObject>(foodItemsIndex[itemIndexValue]);
                            go.transform.position =
                                GamePlayManager.GPM.itemsLeftInGuestBedRoomOneLocation[i];
                        }
                    }
                }
                break;

            case "GuestBedRoom2":
                if (!GamePlayManager.GPM.GuestBedRoom2Visited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.GuestBedRoom2Visited = true;
                }
                else
                {
                    //Verify that both the items left lists of name and positions are not length of 0
                    if (GamePlayManager.GPM.itemsLeftInGuestBedRoomTwoType.Length != 0 &&
                        GamePlayManager.GPM.itemsLeftInGuestBedRoomTwoLocation.Length != 0)
                    {
                        //Go through each of the array value in the room list
                        for (int i = 0; i < GamePlayManager.GPM.itemsLeftInGuestBedRoomTwoType.Length; i++)
                        {
                            //Switch statement for identifying the item name, which then sets the index value of the list of items
                            switch (GamePlayManager.GPM.itemsLeftInGuestBedRoomTwoType[i])
                            {
                                case BIG_CHEESE:
                                    itemIndexValue = 0;
                                    break;
                                case BREAD:
                                    itemIndexValue = 1;
                                    break;

                                case DEATH_CHEESE:
                                    itemIndexValue = 2;
                                    break;
                                case FRUIT_SEED:
                                    itemIndexValue = 3;
                                    break;
                                case BREAD_CRUMB:
                                    itemIndexValue = 4;
                                    break;
                            }

                            //Reinstatiate the item object and place it based on the stored vector value
                            GameObject go = Instantiate<GameObject>(foodItemsIndex[itemIndexValue]);
                            go.transform.position =
                                GamePlayManager.GPM.itemsLeftInGuestBedRoomTwoLocation[i];
                        }
                    }
                }
                break;

            case "Bathroom":
                if (!GamePlayManager.GPM.BathroomVisited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.BathroomVisited = true;
                }
                else
                {
                    //Verify that both the items left lists of name and positions are not length of 0
                    if (GamePlayManager.GPM.itemsLeftInBathRoomType.Length != 0 &&
                        GamePlayManager.GPM.itemsLeftInBathRoomLocation.Length != 0)
                    {
                        //Go through each of the array value in the room list
                        for (int i = 0; i < GamePlayManager.GPM.itemsLeftInBathRoomType.Length; i++)
                        {
                            //Switch statement for identifying the item name, which then sets the index value of the list of items
                            switch (GamePlayManager.GPM.itemsLeftInBathRoomType[i])
                            {
                                case BIG_CHEESE:
                                    itemIndexValue = 0;
                                    break;
                                case BREAD:
                                    itemIndexValue = 1;
                                    break;

                                case DEATH_CHEESE:
                                    itemIndexValue = 2;
                                    break;
                                case FRUIT_SEED:
                                    itemIndexValue = 3;
                                    break;
                                case BREAD_CRUMB:
                                    itemIndexValue = 4;
                                    break;
                            }

                            //Reinstatiate the item object and place it based on the stored vector value
                            GameObject go = Instantiate<GameObject>(foodItemsIndex[itemIndexValue]);
                            go.transform.position =
                                GamePlayManager.GPM.itemsLeftInBathRoomLocation[i];
                        }
                    }
                }
                break;

            case "SecondFloorHallway":
                if (!GamePlayManager.GPM.SecondFloorHallwayVisited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.SecondFloorHallwayVisited = true;
                }
                else
                {
                    //Verify that both the items left lists of name and positions are not length of 0
                    if (GamePlayManager.GPM.itemsLeftInSecondFloorHallwayType.Length != 0 &&
                        GamePlayManager.GPM.itemsLeftInSecondFloorHallwayLocation.Length != 0)
                    {
                        //Go through each of the array value in the room list
                        for (int i = 0; i < GamePlayManager.GPM.itemsLeftInSecondFloorHallwayType.Length; i++)
                        {
                            //Switch statement for identifying the item name, which then sets the index value of the list of items
                            switch (GamePlayManager.GPM.itemsLeftInSecondFloorHallwayType[i])
                            {
                                case BIG_CHEESE:
                                    itemIndexValue = 0;
                                    break;
                                case BREAD:
                                    itemIndexValue = 1;
                                    break;

                                case DEATH_CHEESE:
                                    itemIndexValue = 2;
                                    break;
                                case FRUIT_SEED:
                                    itemIndexValue = 3;
                                    break;
                                case BREAD_CRUMB:
                                    itemIndexValue = 4;
                                    break;
                            }

                            //Reinstatiate the item object and place it based on the stored vector value
                            GameObject go = Instantiate<GameObject>(foodItemsIndex[itemIndexValue]);
                            go.transform.position =
                                GamePlayManager.GPM.itemsLeftInSecondFloorHallwayLocation[i];
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
            case "Bedroom":

                if (!GamePlayManager.GPM.MasterBedRoomVisited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.MasterBedRoomVisited = true;
                }
                break;

            case "Kitchen":

                if (!GamePlayManager.GPM.KitchenRoomVisited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.KitchenRoomVisited = true;
                }
                break;

            case "DiningRoom":
                if (!GamePlayManager.GPM.DinningRoomVisited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.DinningRoomVisited = true;
                }
                break;

            case "LivingRoom":
                if (!GamePlayManager.GPM.LivingRoomVisited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.LivingRoomVisited = true;
                }
                break;

            case "FirstFloorHallway":
                if (!GamePlayManager.GPM.FirstFloorHallwayVisited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.FirstFloorHallwayVisited = true;
                }
                break;

            case "GrandKidsBedRoom":
                if (!GamePlayManager.GPM.GrandKidsBedroomVisited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.GrandKidsBedroomVisited = true;
                }
                break;

            case "GuestBedRoom1":
                if (!GamePlayManager.GPM.GuestBedRoom1Visited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.GuestBedRoom1Visited = true;
                }
                break;

            case "GuestBedRoom2":
                if (!GamePlayManager.GPM.GuestBedRoom2Visited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.GuestBedRoom2Visited = true;
                }
                break;

            case "Bathroom":
                if (!GamePlayManager.GPM.BathroomVisited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.BathroomVisited = true;
                }
                break;

            case "SecondFloorHallway":
                if (!GamePlayManager.GPM.SecondFloorHallwayVisited)
                {
                    SpawnItems();
                    GamePlayManager.GPM.SecondFloorHallwayVisited = true;
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
