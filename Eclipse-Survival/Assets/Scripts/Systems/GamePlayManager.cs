using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using static Constants;

public class GamePlayManager : MonoBehaviour
{
    static public GamePlayManager GPM; //Singleton of the GamePlayManager

    //Static Variables being used to track if the player enter a room that has not been entered during the round
    //First Floor Rooms/Scenes
    public bool KitchenRoomVisited;
    public bool DinningRoomVisited;
    public bool LivingRoomVisited;
    public bool FirstFloorHallwayVisited;
    //Second Floor Rooms/Scenes
    public bool MasterBedRoomVisited;
    public bool GrandKidsBedroomVisited;
    public bool GuestBedRoom1Visited;
    public bool GuestBedRoom2Visited;
    public bool BathroomVisited;
    public bool SecondFloorHallwayVisited;


    [Header("Set in Inspector")]
    public int maxItemSpawn; //The Maximum number of items to spawn in a room or a cycle

    [Header("Set In Inspector: UI objects for in game")]
    public GameObject PauseUI;
    public GameObject OptionsUI;
    public string[] Resolutions;
    public int ResolutionIndex;
    //public bool pauseUIActive;

    [Header("Set Dynamically")]
    public int currentNumberOfHoursSurvived;

    //These array variables are used to store the remaining item name and vector values, and are set dynamically when the perspective scene transition occurs
    //First Floor Item Management
    public string[] itemsLeftInKitchenRoomType;
    public Vector2[] itemsLeftInKitchenRoomLocation;

    public string[] itemsLeftInDinningRoomType;
    public Vector2[] itemsLeftInDinningRoomLocation;

    public string[] itemsLeftInLivingRoomType;
    public Vector2[] itemsLeftInLivingRoomLocation;

    public string[] itemsLeftInFirstFloorHallwayType;
    public Vector2[] itemsLeftInFirstFloorHallwayLocation;

    //Second Floor Item Management
    public string[] itemsLeftInMasterBedRoomType;
    public Vector2[] itemsLeftInMasterBedRoomLocation;

    public string[] itemsLeftInGrandKidsBedRoomType;
    public Vector2[] itemsLeftInGrandKidsBedRoomLocation;

    public string[] itemsLeftInGuestBedRoomOneType;
    public Vector2[] itemsLeftInGuestBedRoomOneLocation;

    public string[] itemsLeftInGuestBedRoomTwoType;
    public Vector2[] itemsLeftInGuestBedRoomTwoLocation;

    public string[] itemsLeftInBathRoomType;
    public Vector2[] itemsLeftInBathRoomLocation;

    public string[] itemsLeftInSecondFloorHallwayType;
    public Vector2[] itemsLeftInSecondFloorHallwayLocation;

    //Dictionary<string, PersistingGameObject> persistingItems;

    //Persisting Gameplay Values
    public int XanderHunger;
    public float XanderStamina;
    public int XanderHealth;
    public int CurrentHoursSurvived { get; set; }
    public int outdoorLightIntensity;
    public float hungerTimer;

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

        PauseUI.SetActive(false);
        OptionsUI.SetActive(false);
    }

    private void InitializeGame()
    {
        targetTag = "SpawnPoint1";
        if (!PlayerPrefs.HasKey(CustomizeControls.ControlNames[0])) CustomizeControls.SetDefaults();
        //persistingItems = new Dictionary<string, PersistingGameObject>();
    }

    private void FixedUpdate()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        if (Input.GetKey(KeyCode.Escape))
        {
            if (activeScene.buildIndex != SPLASH_SCENE &&
                activeScene.buildIndex != STUDIO_SCENE &&
                activeScene.buildIndex != MAIN_MENU_SCENE &&
                activeScene.buildIndex != OPTION_SCENE &&
                activeScene.buildIndex != CREDIT_SCENE &&
                activeScene.buildIndex != GAME_OVER_SCENE)
            {
                PauseGame();
            }         
        }
    }

    public void PauseGame()
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        PauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ActivateOptions()
    {
        PauseUI.SetActive(false);
        OptionsUI.SetActive(true);
    }

    public void DeactivateOptions()
    {
        OptionsUI.SetActive(false);
        PauseUI.SetActive(true);
    }

    //This method will be called from the scene transition when the player clicks start new game
    public void NewGame()
    {
        
        KitchenRoomVisited = false;
        DinningRoomVisited = false;
        LivingRoomVisited = false;
        FirstFloorHallwayVisited = false;
        MasterBedRoomVisited = false;
        GrandKidsBedroomVisited = false;
        GuestBedRoom1Visited = false;
        GuestBedRoom2Visited = false;
        SecondFloorHallwayVisited = false;
        BathroomVisited = false;
        CurrentHoursSurvived = 0;

        XanderHealth = STARTING_HEALTH;
        XanderHunger = STARTING_HUNGER;
        XanderStamina = STARTING_STAMINA;
        hungerTimer = HUNGER_DECREMENT_INTERVAL;

        //spawnWaypoints = null;
    }

    //This method is in charge of intitiating the spawn items procedure for new rounds
    public void NewCycle(int roundsPast)
    {
        KitchenRoomVisited = false;
        DinningRoomVisited = false;
        LivingRoomVisited = false;
        FirstFloorHallwayVisited = false;
        MasterBedRoomVisited = false;
        GrandKidsBedroomVisited = false;
        GuestBedRoom1Visited = false;
        GuestBedRoom2Visited = false;
        SecondFloorHallwayVisited = false;
        BathroomVisited = false;
    }

    public void EndGame()
    {
        //The Player is dead, switch to the Game Over Scene
        SceneManager.LoadScene("GameOver_Scene");
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
