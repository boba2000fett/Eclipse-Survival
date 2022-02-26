using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour
{
    [Header("Set In Inspector: Room")]
    [Tooltip("This will be the name of the Scene that this 'Room' is in")]
    public string sceneName; //This will be the name of the Scene that this "Room" is in
    [Tooltip("The name of the scene that is to the North of the current room")]
    public string northSceneName; //The name of the scene that is to the North of the current room
    [Tooltip("The name of the scene that is to the East of the current room")] 
    public string eastSceneName; //The name of the scene that is to the East of the current room
    [Tooltip("The name of the scene that is to the West of the current room")] 
    public string westSceneName; //The name of the scene that is to the West of the current room
    [Tooltip("The name of the scene that is to the South of the current room")] 
    public string southSceneName; //The name of the scene that is to the South of the current room    
    [Tooltip("The number of available exits that room has")]
    public int availableExits; //The number of available exits that room has
    [Header("")]
    [Tooltip("This the list of waypoints in that room")]
    //This is useful because if switched to a scene where the enemy is supposed to be in that scene, the enemy can be moved
    //To a random variable 
    public Waypoint[] waypointsInRoom;
    public Waypoint southExit;
    public Waypoint northExit;
    public Waypoint eastExit;
    public Waypoint westExit;
    public Waypoint homeNode;

    public bool isWallCrawlingStage;

    [Header("Potentially Delete Later: Room")]
    [Tooltip("This will be the name of the build index that this 'Room' is in (POTENTIALLY DELETE THIS LATER)")]
    public int roomID; //This will be the build index of the Scene that this "Room" is in
    

    private void Start()
    {
        //roomID = SceneManager.GetActiveScene().buildIndex;
        //sceneName = SceneManager.GetActiveScene().name;
    }

    /// <summary>
    /// The KeyBinds in this script are purely for testing reasons. Make sure to delete this later.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (SceneManager.GetActiveScene().name != "TrentBedroom1")
            {
                SceneManager.LoadScene("TrentBedroom1");
            }           
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (SceneManager.GetActiveScene().name != "TrentKitchen")
            {
                SceneManager.LoadScene("TrentKitchen");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (SceneManager.GetActiveScene().name != "TrentWall1")
            {
                SceneManager.LoadScene("TrentWall1");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (SceneManager.GetActiveScene().name != "Bedroom1")
            {
                SceneManager.LoadScene("Bedroom1");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (SceneManager.GetActiveScene().name != "Kitchen")
            {
                SceneManager.LoadScene("Kitchen");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (SceneManager.GetActiveScene().name != "Wall1")
            {
                SceneManager.LoadScene("Wall1");
            }
        }


        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    if (SceneManager.GetActiveScene().name == "Trent")
        //    {
        //        SceneManager.LoadScene("Trent2");
        //    }
        //    else if (SceneManager.GetActiveScene().name == "Trent2")
        //    {
        //        SceneManager.LoadScene("Trent3");
        //    }
        //    else if (SceneManager.GetActiveScene().name == "Trent3")
        //    {
        //        SceneManager.LoadScene("Trent4");
        //    }
        //    else if (SceneManager.GetActiveScene().name == "Trent4")
        //    {
        //        SceneManager.LoadScene("Trent");
        //    }
        //}
    }
}
