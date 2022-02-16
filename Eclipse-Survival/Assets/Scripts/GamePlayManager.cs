using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    static public GamePlayManager GPM; //Singleton of the GamePlayManager

    //Static Variables being used to track if the player enter a room that has not been entered during the round
    public bool mainRoom1;
    public bool mainRoom2;

    [Header("Set in Inspector")]
    //The values below inpact the spawn rates and type of items being spawned throughout the round
    //Determine if these sould be in the Game Manager or its own script
    public Waypoint[] spawnWaypoints;
    public GameObject[] foodItems;
    public float itemSpawnChance; //The chance percent of an Item spawning at a certain position
    public int maxItemSpawn; //The Maximum number of items to spawn in a room or a cycle

    [Header("Set Dynamically")]
    public int currentNumberOfCycleSurvived;
    //The values below inpact the spawn rates and type of items being spawned throughout the round
    //Determine if these sould be in the Game Manager or its own script
    private GameObject[] foodItemBeingRemoved;
    private int itemsSpawned;
    private int selectedItemNum;

    void Awake()
    {
        if (GPM == null)
        {
            //Set the GPM instance
            GPM = this;
        }
        else if(GPM != this)
        {
            //If the reference has already been set and
            //is not the right instance reference, Destroy the GameObject
            Destroy(gameObject);
        }

        //Do not Destroy this gameobject when a new scene is loaded
        DontDestroyOnLoad(gameObject);
    }

    //This method will be called from the scene transition when the player clicks start new game
    public void NewGame()
    {
        mainRoom1 = true;
        mainRoom2 = true;
        currentNumberOfCycleSurvived = 0;

        //spawnWaypoints = null;
    }

    //This method is in charge of intitiating the spawn items procedure for new rounds
    public void NewCycle(int roundsPast)
    {
        if (currentNumberOfCycleSurvived < roundsPast)
        {
            currentNumberOfCycleSurvived = roundsPast;
        }

        SpawnItems();
    }

    public void EndGame()
    {
        //The Player is dead, switch to the Game Over Scene
        SceneManager.LoadScene(Constants.MAIN_MENU_SCENE);
    }

    //Determine if these sould be in the Game Manager or its own script
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

    //Determine if these sould be in the Game Manager or its own script
    void RemoveItems()
    {
        //Find all food/item game object within the scene
        foodItemBeingRemoved = GameObject.FindGameObjectsWithTag("Item");

        //For Eachfood Game Object identified, we destroy them since a new round has start
        foreach (GameObject go in foodItemBeingRemoved)
        {
            Destroy(go);
        }
    }
}
