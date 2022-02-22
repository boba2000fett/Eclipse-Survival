using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    static public GamePlayManager GPM; //Singleton of the GamePlayManager

    //Static Variables being used to track if the player enter a room that has not been entered during the round
    public bool BedRoomVisited;
    public bool KitchenRoomVisited;

    [Header("Set in Inspector")]
    public int maxItemSpawn; //The Maximum number of items to spawn in a room or a cycle

    [Header("Set Dynamically")]
    public int currentNumberOfCycleSurvived;

    //These array variables are used to store the remaining item name and vector values, and are set dynamically when the perspective scene transition occurs
    public string[] itemsLeftInBedRoomType;
    public Vector2[] itemsLeftInBedRoomLocation;
    public string[] itemsLeftInKitchenRoomType;
    public Vector2[] itemsLeftInKitchenRoomLocation;
    //Dictionary<string, PersistingGameObject> persistingItems;

    // Scene Transition Variables
    public string targetTag;

    void Awake()
    {
        if (GPM == null)
        {
            //Set the GPM instance
            GPM = this;
            InitializeGame();
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

    private void InitializeGame()
    {
        targetTag = "SpawnPoint1";
        //persistingItems = new Dictionary<string, PersistingGameObject>();
    }

    //This method will be called from the scene transition when the player clicks start new game
    public void NewGame()
    {
        BedRoomVisited = false;
        KitchenRoomVisited = false;
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

        BedRoomVisited = false;
        KitchenRoomVisited = false;
    }

    public void EndGame()
    {
        //The Player is dead, switch to the Game Over Scene
        SceneManager.LoadScene(Constants.MAIN_MENU_SCENE);
    }

   
}

//public class PersistingGameObject
//{
//    string Type { get; set; }
//    Vector2 Location { get; set; }

//    public PersistingGameObject(string type, Vector2 location)
//    {
//        Type = type;
//        Location = location;
//    }
//}
