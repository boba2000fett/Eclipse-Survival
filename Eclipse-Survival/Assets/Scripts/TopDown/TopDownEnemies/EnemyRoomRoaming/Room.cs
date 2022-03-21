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
    public string stairSceneName;
    public string wall1SceneName;
    public string wall2SceneName;
    public string wall3SceneName;
    public string wall4SceneName;
    public string wall5SceneName;
    [Tooltip("The number of available exits that room has")]
    public int availableExits; //The number of available exits that room has
    
    [Header("Exit Waypoint Exit Prefab")]
    public EnemyWaypoint enemyWaypointExitPrefab;

    [Header("Set Dynamically")]
    [Tooltip("This the list of waypoints in that room")]
    //This is useful because if switched to a scene where the enemy is supposed to be in that scene, the enemy can be moved
    //To a random variable 
    public EnemyWaypoint[] waypointsInRoom;
    public EnemyWaypoint southExit;
    public EnemyWaypoint northExit;
    public EnemyWaypoint eastExit;
    public EnemyWaypoint westExit;
    public EnemyWaypoint stairsExit;
    public EnemyWaypoint homeNode;
    public EnemyWaypoint wall1Exit;
    public EnemyWaypoint wall2Exit;
    public EnemyWaypoint wall3Exit;
    public EnemyWaypoint wall4Exit;
    public EnemyWaypoint wall5Exit;

    //public int[] nodeIntList;
    //public int northExitInt;
    //public int southExitInt;
    //public int westExitInt;
    //public int eastExitInt;
    //public int startsExitInt;

    public bool isWallCrawlingStage;

    public void Start()
    {
        if (!isWallCrawlingStage)
        {
            FindNodes();
        }
        else
        {
            FindExitNodesSideViewScreen();
        }
        
    }

    /// <summary>
    /// The KeyBinds in this script are purely for testing reasons. Make sure to delete this later.
    /// </summary>
    private void Update()
    {
#if UNITY_EDITOR
        TestingRooms();
#endif
    }

    public void TestingRooms()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (SceneManager.GetActiveScene().name != "UpstairsTopLeftBedroom")
            {
                SceneManager.LoadScene("UpstairsTopLeftBedroom");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (SceneManager.GetActiveScene().name != "UpstairsTopRightBedroom")
            {
                SceneManager.LoadScene("UpstairsTopRightBedroom");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (SceneManager.GetActiveScene().name != "UpstairsBottomRightBedroom")
            {
                SceneManager.LoadScene("UpstairsBottomRightBedroom");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (SceneManager.GetActiveScene().name != "UpstairsBottomLeftBedroom")
            {
                SceneManager.LoadScene("UpstairsBottomLeftBedroom");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (SceneManager.GetActiveScene().name != "UpstairsCenterHallway")
            {
                SceneManager.LoadScene("UpstairsCenterHallway");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (SceneManager.GetActiveScene().name != "UpstairsCenterRightBathroom")
            {
                SceneManager.LoadScene("UpstairsCenterRightBathroom");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (SceneManager.GetActiveScene().name != "DownstairsTopLeftKitchen")
            {
                SceneManager.LoadScene("DownstairsTopLeftKitchen");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (SceneManager.GetActiveScene().name != "DownstairsTopRightDiningRoom")
            {
                SceneManager.LoadScene("DownstairsTopRightDiningRoom");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (SceneManager.GetActiveScene().name != "DownstairsBottomLeftHallway")
            {
                SceneManager.LoadScene("DownstairsBottomLeftHallway");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (SceneManager.GetActiveScene().name != "DownstairsBottomRightLivingRoom")
            {
                SceneManager.LoadScene("DownstairsBottomRightLivingRoom");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            if (SceneManager.GetActiveScene().name != "TrentBedroom1")
            {
                SceneManager.LoadScene("TrentBedroom1");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            if (SceneManager.GetActiveScene().name != "TrentWall1")
            {
                SceneManager.LoadScene("TrentWall1");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (SceneManager.GetActiveScene().name != "TrentKitchen")
            {
                SceneManager.LoadScene("TrentKitchen");
            }
        }
        
    }

    public void FindNodes()
    {       
        GameObject[] tempArray = GameObject.FindGameObjectsWithTag("EnemyWaypoint");
        EnemyWaypoint[] waypointList = new EnemyWaypoint[tempArray.Length];

        for (int i = 0; i < tempArray.Length; i++)
        {
            waypointList[i] = tempArray[i].GetComponent<EnemyWaypoint>();
            if (waypointList[i].gameObject.name == "SouthExit")
            {
                southExit = waypointList[i];
            }
            else if (waypointList[i].gameObject.name == "EastExit")
            {
                eastExit = waypointList[i];
            }
            else if (waypointList[i].gameObject.name == "NorthExit")
            {
                northExit = waypointList[i];
            }
            else if (waypointList[i].gameObject.name == "WestExit")
            {
                westExit = waypointList[i];
            }
            else if (waypointList[i].gameObject.name == "StairsExit")
            {
                stairsExit = waypointList[i];
            }
            else if (waypointList[i].gameObject.name == "HomeNode")
            {
                homeNode = waypointList[i];
            }
            else if (waypointList[i].gameObject.name == "Wall1Exit")
            {
                wall1Exit = waypointList[i];
            }
            else if (waypointList[i].gameObject.name == "Wall2Exit")
            {
                wall2Exit = waypointList[i];
            }
            else if (waypointList[i].gameObject.name == "Wall3Exit")
            {
                wall3Exit = waypointList[i];
            }
            else if (waypointList[i].gameObject.name == "Wall4Exit")
            {
                wall4Exit = waypointList[i];
            }
            else if (waypointList[i].gameObject.name == "Wall5Exit")
            {
                wall5Exit = waypointList[i];
            }
            
        }

        waypointsInRoom = waypointList;
    }

    /// <summary>
    /// This method is a bit different than the one above, which is primarily utalized for the Side View Screens. 
    /// </summary>
    public void FindExitNodesSideViewScreen()
    {
        /* 
         Essentially, the goal with this method is to pretty much just grab the information from the SceneTransition
        Objects to create EnemyWaypoint objects that are at the same position as the SpawnPoint.

        PerspectiveSceneChange[] sceneTransitionArray = GameObject.FindObjectsOfType<PerspectiveSceneChange>();

        sceneTransitionArray[i] Elements:
            targetPerspectiveScene: Gives the name of the Top Down Scene that is right Next to the hole
            spawnPoint: Actual reference to the SpawnPoint object in the Hierarchy that coincides with that Scene Change.

        foreach(s in sceneTransitionArray)
        if(Wall1.sceneName == null)
            -wall1.sceneName = s.targetPerspectiveScene
            -GameObject go = Instantiate();
            -go.addcomponent<EnemyWaypoint>();
            go.name = wall1Exit;
            go.position = s.spawnPoint.transform.position;

         */

        this.sceneName = SceneManager.GetActiveScene().name;

        PerspectiveSceneChange[] sceneTransitionArray = GameObject.FindObjectsOfType<PerspectiveSceneChange>();

        foreach (PerspectiveSceneChange s in sceneTransitionArray)
        {
            if (wall1SceneName == null || wall1SceneName == "")
            {
                wall1SceneName = s.targetPerspectiveScene;
                EnemyWaypoint go = GameObject.Instantiate(enemyWaypointExitPrefab);
                go.name = "Wall1Exit";
                go.gameObject.transform.position = s.spawnPoint.transform.position;
                wall1Exit = go;
            }
            else if (wall2SceneName == null || wall2SceneName == "")
            {
                wall2SceneName = s.targetPerspectiveScene;
                EnemyWaypoint go = GameObject.Instantiate(enemyWaypointExitPrefab);
                go.name = "Wall2Exit";
                go.gameObject.transform.position = s.spawnPoint.transform.position;
                wall2Exit = go;
            }
            else if (wall3SceneName == null || wall3SceneName == "")
            {
                wall3SceneName = s.targetPerspectiveScene;
                EnemyWaypoint go = GameObject.Instantiate(enemyWaypointExitPrefab);
                go.name = "Wall1Exit";
                go.gameObject.transform.position = s.spawnPoint.transform.position;
                wall3Exit = go;
            }
            else if (wall4SceneName == null || wall4SceneName == "")
            {
                wall4SceneName = s.targetPerspectiveScene;
                EnemyWaypoint go = GameObject.Instantiate(enemyWaypointExitPrefab);
                go.name = "Wall1Exit";
                go.gameObject.transform.position = s.spawnPoint.transform.position;
                wall4Exit = go;
            }
            else if (wall5SceneName == null || wall5SceneName == "")
            {
                wall5SceneName = s.targetPerspectiveScene;
                EnemyWaypoint go = GameObject.Instantiate(enemyWaypointExitPrefab);
                go.name = "Wall1Exit";
                go.gameObject.transform.position = s.spawnPoint.transform.position;
                wall5Exit = go;
            }
        }
    }

}
