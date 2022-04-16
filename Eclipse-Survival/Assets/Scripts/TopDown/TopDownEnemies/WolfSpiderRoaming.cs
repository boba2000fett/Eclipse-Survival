using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState 
{ 
    InMenuScene,
    InPlayScene,
}


public class WolfSpiderRoaming : MonoBehaviour
{
    [SerializeField] public LinkedList<Room> travelList = new LinkedList<Room>();
    [SerializeField] public Room[] travelListSee;


    [Header("Set in Inspector: WolfSpiderRoaming")]
    public WolfSpiderTopDown wolfSpiderTopDownPrefab;
    public WolfSpider wolfSpiderSidePrefab;
    public float roomTimeIntervalLeftBound;
    public float roomTimeIntervalRightBound;

    //[Header("Game Over Scene Name: Set in Inspector")]
    //public string gameOverSceneName;
    //public string firstSceneName;

    [Header("Set Room Manager in Inspector")]
    public RoomManager roomManager;


    

    [Header("Set Dynamically: WolfSpiderRoaming")]
    public float roomTime;
    public float roomTimeInterval;
    public Room currentRoom;
    public WolfSpiderTopDown wolfSpiderTopDownInstance; //This is used to keep track of the Instance that is spawned
    public WolfSpider wolfSpiderSideInstance; //This is used to keep track of the Instance that is spawned
    public Room nextRoom;

    public EnemyWaypoint exitWaypoint;

    public PathToExit pathToExit = PathToExit.East;
    public PathToExit spawnEntrance = PathToExit.West;

    public bool spiderTravelingToExit = false;

    public bool exitedMenuScene = false;
    public bool awakeFunctionalityCompleted = false;

    public void Awake()
    {
        ArrivedInNewRoom();

        roomTime = 0;
        roomTimeInterval = UnityEngine.Random.Range(roomTimeIntervalLeftBound, roomTimeIntervalRightBound);

        travelList = new LinkedList<Room>();

        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

    //public void AwakeFunctionality()
    //{
    //    ArrivedInNewRoom();

    //    roomTime = 0;
    //    roomTimeInterval = UnityEngine.Random.Range(roomTimeIntervalLeftBound, roomTimeIntervalRightBound);

    //    travelList = new LinkedList<Room>();
    //}

    public void CheckingIfNotInLevelScenes()
    {
        
    }

    private void Update()
    {
        #region If Not in Menu Scene
        //if (!exitedMenuScene)
        //{
        //    return;
        //}
        //if (exitedMenuScene && !awakeFunctionalityCompleted)
        //{
        //    AwakeFunctionality();
        //    awakeFunctionalityCompleted = true;
        //    return;
        //}
        #endregion

        #region Debug Testing
        travelListSee = travelList.ToArray();
        #endregion

        if (!spiderTravelingToExit)
        {
            RecordTime();
        }
        else
        {
            DetermineIfSpiderHasReachedExitNode();
        }
    }

    /// <summary>
    /// This is the method that is called when this script is determining whether the Spider has reached the waypoint.
    /// The goal with this method is to switch the room the spider is in (call SwitchRoom()) once the Spider in the
    /// scene (whether it is a top-down or side scene) has reached the exitWaypoint
    /// </summary>
    public void DetermineIfSpiderHasReachedExitNode()
    {
        if (currentRoom.isWallCrawlingStage)
        {
            //See if Side Spider has reached the Exit node;
            //If so, call SwitchRoom(), and set spiderTravelingToExit to false;

            //Will use a property on the WolfSpider (use this for when determining if the Spider has retrieved
            //the end of path)
            if (SceneManager.GetActiveScene().name != currentRoom.sceneName)
            {
                SwitchRoom();
                return;
            }

            if (wolfSpiderSideInstance.ReachedExit)
            {
                Destroy(wolfSpiderSideInstance.gameObject);
                spiderTravelingToExit = false;
                SwitchRoom();
            }
        }
        else
        {
            //See if the Top-Down spider instance has reached the exit node
            //If so, call SwitchRoom(), and set spiderTravelingToExit to false;

            if (SceneManager.GetActiveScene().name != currentRoom.sceneName)
            {
                SwitchRoom();
                return;
            }

            if (wolfSpiderTopDownInstance.reachedExit)
            {
                Destroy(wolfSpiderTopDownInstance.gameObject);
                spiderTravelingToExit = false;
                SwitchRoom();
            }            
        }
    }


    /// <summary>
    /// This method is called when the Spider has arrived in a new room. This will essentially determine the Spawn
    /// Direction (the Direction the Spider is coming from), the Exit Direction (the Direction the spider is supposed 
    /// to go next). 
    /// </summary>
    public void ArrivedInNewRoom()
    {
        Debug.LogWarning("ArrivedInNewRoom(): Spider has arrived in new Scene");
        /* TravelToNewRoom()   When Spider Travels To New Room
If spider is currently in the same Room as player
    Find the Destination Node (node) that will get you to that next room, and keep track of it*/

        DetermineNextRoom();

        FindSpawnDirection();
        FindExitDirection();

        if (currentRoom.sceneName== SceneManager.GetActiveScene().name)
        {
            Debug.LogWarning("ArrivedInNewRoom(): Spider Has Arrived in the Currently Loaded Scene");
            FindExitNode();
            SpawnSpider(!currentRoom.isWallCrawlingStage, false);
            //Spawn in Spider at one of the entrances, found with the spawnEntrance variable in the FindSpawnDirection Method;
        }
    }

    /// <summary>
    /// This determines the Direction, or waypoint essentially, that the spider will spawn from if it is 
    /// traveling into the same room that you are. This is recorded with the spawnEntrance variable. 
    /// </summary>
    public void FindSpawnDirection()
    {
        if (travelList.Count < 1)
            return;

        if (travelList.Last.Value.sceneName == currentRoom.northSceneName)
        {
            spawnEntrance = PathToExit.North;
        }
        else if (travelList.Last.Value.sceneName == currentRoom.westSceneName)
        {
            spawnEntrance = PathToExit.West;
        }
        else if (travelList.Last.Value.sceneName == currentRoom.eastSceneName)
        {
            spawnEntrance = PathToExit.East;
        }
        else if (travelList.Last.Value.sceneName == currentRoom.southSceneName)
        {
            spawnEntrance = PathToExit.South;
        }
        else if (travelList.Last.Value.sceneName == currentRoom.wall1SceneName)
        {
            spawnEntrance = PathToExit.Wall1;
        }
        else if (travelList.Last.Value.sceneName == currentRoom.wall2SceneName)
        {
            spawnEntrance = PathToExit.Wall2;
        }
        else if (travelList.Last.Value.sceneName == currentRoom.wall3SceneName)
        {
            spawnEntrance = PathToExit.Wall3;
        }
        else if (travelList.Last.Value.sceneName == currentRoom.wall4SceneName)
        {
            spawnEntrance = PathToExit.Wall4;
        }
        else if (travelList.Last.Value.sceneName == currentRoom.wall5SceneName)
        {
            spawnEntrance = PathToExit.Wall5;
        }
    }

    /// <summary>
    /// This finds the Exit Direction that the spider will take. Essentially, this is the direction the Spider must
    /// go to get to the next room. This is recorded with the pathToExit variable.
    /// </summary>
    public void FindExitDirection()
    {
        if (nextRoom.sceneName == currentRoom.northSceneName)
        {
            pathToExit = PathToExit.North;
        }
        else if (nextRoom.sceneName == currentRoom.westSceneName)
        {
            pathToExit = PathToExit.West;
        }
        else if (nextRoom.sceneName == currentRoom.eastSceneName)
        {
            pathToExit = PathToExit.East;
        }
        else if (nextRoom.sceneName == currentRoom.southSceneName)
        {
            pathToExit = PathToExit.South;
        }
        else if (nextRoom.sceneName == currentRoom.wall1SceneName)
        {
            pathToExit = PathToExit.Wall1;
        }
        else if (nextRoom.sceneName == currentRoom.wall2SceneName)
        {
            pathToExit = PathToExit.Wall2;
        }
        else if (nextRoom.sceneName == currentRoom.wall3SceneName)
        {
            pathToExit = PathToExit.Wall3;
        }
        else if (nextRoom.sceneName == currentRoom.wall4SceneName)
        {
            pathToExit = PathToExit.Wall4;
        }
        else if (nextRoom.sceneName == currentRoom.wall5SceneName)
        {
            pathToExit = PathToExit.Wall5;
        }

    }

    /// <summary>
    /// This Method will find the Exit Node when the Spider either enters in the room Xander is currently in,
    /// or if Xander enters a room that the Spider is currently in.
    /// </summary>
    public void FindExitNode()
    {
        Room currentRoomInstance = GameObject.FindGameObjectWithTag("Room").GetComponent<Room>();
        
        if (currentRoomInstance.waypointsInRoom.Length == 0)
        {
            currentRoomInstance.Start();
        }
        
        Debug.LogWarning($"FindExitNode: Found Exit Node");
        switch (pathToExit) 
        {
            case PathToExit.North:
                exitWaypoint = currentRoomInstance.northExit;
                break;
            case PathToExit.East:
                exitWaypoint = currentRoomInstance.eastExit;
                break;
            case PathToExit.West:
                exitWaypoint = currentRoomInstance.westExit;
                break;
            case PathToExit.South:
                exitWaypoint = currentRoomInstance.southExit;
                break;
            case PathToExit.Stairs:
                exitWaypoint = currentRoomInstance.stairsExit;
                break;
            case PathToExit.Wall1:
                exitWaypoint = currentRoomInstance.wall1Exit;
                break;
            case PathToExit.Wall2:
                exitWaypoint = currentRoomInstance.wall2Exit;
                break;
            case PathToExit.Wall3:
                exitWaypoint = currentRoomInstance.wall3Exit;
                break;
            case PathToExit.Wall4:
                exitWaypoint = currentRoomInstance.wall4Exit;
                break;
            case PathToExit.Wall5:
                exitWaypoint = currentRoomInstance.wall5Exit;
                break;
        }


        #region Planning Notes
        /*    
         *    OLD CODE
       if (nextRoom.sceneName == currentRoom.northSceneName)
        {
            pathToExit = PathToExit.North;
            exitWaypoint = currentRoom.northExit;
        }
        else if (travelList.Last.Value.sceneName == currentRoom.westSceneName)
        {
            pathToExit = PathToExit.West;
            exitWaypoint = currentRoom.westExit;
        }
        else if (travelList.Last.Value.sceneName == currentRoom.eastSceneName)
        {
            pathToExit = PathToExit.East;
            exitWaypoint = currentRoomInstance.eastExit;
        }
        else if (nextRoom.sceneName == currentRoom.southSceneName)
        {
            pathToExit = PathToExit.South;
            exitWaypoint = currentRoom.southExit;
        }
        else if (travelList.Last.Value.sceneName == currentRoom.wall1SceneName)
        {
            pathToExit = PathToExit.Wall1;
            exitWaypoint = currentRoom.wall1Exit;
        }
        else if (travelList.Last.Value.sceneName == currentRoom.wall2SceneName)
        {
            pathToExit = PathToExit.Wall2;
            exitWaypoint = currentRoom.wall2Exit;
        }
    
    if(NextRoom.sceneName = currentRoom.Wall1SceneName)
    {
        pathToExit = PathToExit.Wall1;
        waypointDestination = Room.wall1Exit;
    }
    if(NextRoom.sceneName = currentRoom.Wall2SceneName)
    {
        pathToExit = PathToExit.Wall2;
        waypointDestination = Room.wall2Exit;
    }

    wolfSpiderInstance.TravelTo(waypointDestination);
*/
        #endregion
    }


    /// <summary>
    /// This is the method that records the amount of Time the Spider is supposed to be in the current room.
    /// If it is time for the spider to leave, it calls the TimeToLeave() function
    /// </summary>
    public void RecordTime()
    {
        roomTime += Time.deltaTime;

        if (roomTime >= roomTimeInterval)
        {
            roomTime = 0;
            roomTimeInterval = UnityEngine.Random.Range(roomTimeIntervalLeftBound, roomTimeIntervalRightBound);
            TimeToLeave();
        }
    }

    /// <summary>
    /// This method is called when the amount of time the Wolf Spider is supposed to be in the room has passed.
    /// This is the first method called when the time limit (roomTime variable) is passed
    /// </summary>
    public void TimeToLeave()
    {
        #region Planning
        /*    TimeToLeave()       Have Spider be in a certain room for period of time, and then signal for the Spider to leave
    -If Spider is in current room
    -    Give Waypoint Destination for Spider to Travel towards
    -        //Note: node is found when you are generating
    -        If(Current Room is a Wall Crawling scene)
    -            wolfSpiderSideInstance.TravelToExit(node); 
    -        else
    -            wolfSpiderTopDownInstance.TravelToExit(node);
   - Else(if spider is not in current room)
    -    Switch the Spider into the Next Room (call Method called SwitchRoom())
    */
        #endregion

        if (currentRoom.sceneName == SceneManager.GetActiveScene().name)
        {
            if (currentRoom.isWallCrawlingStage)
            {
                wolfSpiderSideInstance.ExitRoom((Vector2)exitWaypoint.gameObject.transform.position);
                spiderTravelingToExit = true;
            }
            else
            {
                wolfSpiderTopDownInstance.TravelToNode(exitWaypoint);
                spiderTravelingToExit = true;
            }
        }
        else
        {
            SwitchRoom();
        }
    }

    /// <summary>
    /// This method is called when the current Spider instance reaches the Exit Waypoint if they are in the 
    /// same scene as Xander.
    /// </summary>
    public void ReachedExit()
    {
        /* 
         ReachedExit() 
        If Spider has reached the Exit of a Room()s
            Delete the Spider Instance in the current room
            Call SwitchRoom()
         */
    }

    /// <summary>
    /// This Method Switches the Room that the Wolf Spider is in. This is where the currentRoom is addded to 
    /// travelList, currentRoom = nextRoom (the currentRoom is switched over to the nextRoom), and nextRoom is set
    /// to null;
    /// </summary>
    public void SwitchRoom()
    {
        /*     Add the currentRoom to the travelList
            Set nextRoom = currentRoom
        */
        travelList.AddLast(currentRoom);
        currentRoom = nextRoom;
        nextRoom = null;

        ArrivedInNewRoom();
    }


    /// <summary>
    /// This method determines the next Room Xander will travel to. We will want to make sure to call this method
    /// whenever Xander enters a new Room.
    /// </summary>
    public void DetermineNextRoom()
    {
        /* 
         public void DetermineNextRoom()
    nextRoom = roomManager.FindNextRoomSpider();
         */
        nextRoom = roomManager.FindNextRoomSpider(currentRoom);
    }

   
    /// <summary>
    /// This scene will be loaded whenever we need to Spawn the spider. This would only happen if the Spider is 
    /// entering a scene that Xander is currently in, OR if Xander is entering a scene that the Spider is currently in.
    /// </summary>
    /// <param name="isTopDownScene"></param>
    private void SpawnSpider(bool isTopDownScene, bool isAlreadyInScene)
    {
        if (isTopDownScene && !isAlreadyInScene)
        {
            GameObject go = GameObject.Instantiate(wolfSpiderTopDownPrefab.gameObject);
            wolfSpiderTopDownInstance = go.GetComponent<WolfSpiderTopDown>();
            Room currentRoomInstance = GameObject.FindGameObjectWithTag("Room").GetComponent<Room>();

            //Place at an Endpoint pertaining to the direction the Enemy is Traveling From
            switch (spawnEntrance)
            {
                case PathToExit.North:
                    wolfSpiderTopDownInstance.transform.position = currentRoomInstance.northExit.gameObject.transform.position;
                    break;
                case PathToExit.East:
                    wolfSpiderTopDownInstance.transform.position = currentRoomInstance.eastExit.gameObject.transform.position;
                    break;
                case PathToExit.West:
                    wolfSpiderTopDownInstance.transform.position = currentRoomInstance.westExit.gameObject.transform.position;
                    break;
                case PathToExit.South:
                    wolfSpiderTopDownInstance.transform.position = currentRoomInstance.southExit.gameObject.transform.position;
                    break;
                case PathToExit.Stairs:
                    wolfSpiderTopDownInstance.transform.position = currentRoomInstance.stairsExit.gameObject.transform.position;
                    break;
                case PathToExit.Bathroom:
                    wolfSpiderTopDownInstance.transform.position = currentRoomInstance.bathroomExit.gameObject.transform.position;
                    break;
                case PathToExit.Wall1:
                    wolfSpiderTopDownInstance.transform.position = currentRoomInstance.wall1Exit.gameObject.transform.position;
                    break;
                case PathToExit.Wall2:
                    wolfSpiderTopDownInstance.transform.position = currentRoomInstance.wall2Exit.gameObject.transform.position;
                    break;
                case PathToExit.Wall3:
                    wolfSpiderTopDownInstance.transform.position = currentRoomInstance.wall3Exit.gameObject.transform.position;
                    break;
                case PathToExit.Wall4:
                    wolfSpiderTopDownInstance.transform.position = currentRoomInstance.wall4Exit.gameObject.transform.position;
                    break;
                case PathToExit.Wall5:
                    wolfSpiderTopDownInstance.transform.position = currentRoomInstance.wall5Exit.gameObject.transform.position;
                    break;
            }
        }
        else if (isTopDownScene && isAlreadyInScene)
        {
            FindExitNode();

            GameObject go = GameObject.Instantiate(wolfSpiderTopDownPrefab.gameObject);
            wolfSpiderTopDownInstance = go.GetComponent<WolfSpiderTopDown>();

            Room roomInstance = GameObject.FindGameObjectWithTag("Room").GetComponent<Room>();

            //int index = UnityEngine.Random.Range(1, roomInstance.waypointsInRoom.Length);
            int index = UnityEngine.Random.Range(1, roomInstance.spawnWaypoints.Count);

            //wolfSpiderTopDownInstance.transform.position = roomInstance.waypointsInRoom[index].gameObject.transform.position;
            wolfSpiderTopDownInstance.transform.position = roomInstance.spawnWaypoints[index].gameObject.transform.position;

            //Optional: Make Wolf Spider spawn away from one of the exits (so it doesn't spawn right next to Xander
            //when Xander is entering a room
        }
        else if (!isTopDownScene && !isAlreadyInScene)
        {
            GameObject go = GameObject.Instantiate(wolfSpiderSidePrefab.gameObject);
            wolfSpiderSideInstance = go.GetComponent<WolfSpider>();
            Room currentRoomInstance = GameObject.FindGameObjectWithTag("Room").GetComponent<Room>();

            Debug.LogWarning($"SIDE Spider Position Before Switch: {wolfSpiderTopDownInstance.transform.position}");

            //Place at an Endpoint pertaining to the direction the Enemy is Traveling From
            switch (spawnEntrance)
            {
                case PathToExit.Wall1:
                    wolfSpiderTopDownInstance.transform.position = currentRoomInstance.wall1Exit.gameObject.transform.position;
                    break;
                case PathToExit.Wall2:
                    wolfSpiderTopDownInstance.transform.position = currentRoomInstance.wall2Exit.gameObject.transform.position;
                    break;
                case PathToExit.Wall3:
                    wolfSpiderTopDownInstance.transform.position = currentRoomInstance.wall3Exit.gameObject.transform.position;
                    break;
                case PathToExit.Wall4:
                    wolfSpiderTopDownInstance.transform.position = currentRoomInstance.wall4Exit.gameObject.transform.position;
                    break;
                case PathToExit.Wall5:
                    wolfSpiderTopDownInstance.transform.position = currentRoomInstance.wall5Exit.gameObject.transform.position;
                    break;
            }
        }
        else if (!isTopDownScene && isAlreadyInScene)
        {
            FindExitNode();

            GameObject go = GameObject.Instantiate(wolfSpiderSidePrefab.gameObject);
            wolfSpiderSideInstance = go.GetComponent<WolfSpider>();

            List<Node> spawnLocations = AIPathfinding.GenerateNodesForLevel();

            int index = UnityEngine.Random.Range(1, spawnLocations.Count);

            wolfSpiderSideInstance.transform.position = spawnLocations[index].position;
        }

        #region Possibly Place at a Random Waypoint

        #endregion
    }

    public void RefreshVariables()
    {

    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        #region Planning
        /*         
If Scene Changes
-    If (GetActiveScene().name != currentRoom.sceneName)
        Set inRoom to false
-        Set spiderInstances to null
    else //If you switched to a scene that the Spider is in
        Spawn the Spider at a random location (node) in the Room //I think this can be retrieved from the Room Object
            if (FindObjectOfType(Room).isWallCrawlingScene)
                Spawn Side Spider (Spawn an instance of wolfSpiderSidePrefab, and set it equal to wolfSpiderSideInstance)
            else
                Spawn Top-Down Spider (Spawn an instance of wolfSpiderTopDownPrefab, and set it equal to wolfSpiderTopDownPrefabInstance)
            if(node == null)
            //If you traveled to a Room that the spider is already in, and since the spider did not record
            //a node (it did not find an exit node because the Spider was not in the same scene), 
            //you will now find the node
                Find Node based on the Spider's exit
         */
        #endregion

        //if (SceneManager.GetActiveScene().name == firstSceneName && !exitedMenuScene)
        //{
        //    exitedMenuScene = true;
        //    return;
        //}

        if (SceneManager.GetActiveScene().name != currentRoom.sceneName)
        {
            Debug.LogWarning("Spider is no longer in scene");
            wolfSpiderSideInstance = null;
            wolfSpiderTopDownInstance = null;
        }
        else
        {
            Debug.LogWarning("You entered the same scene that the Spider is in");
            SpawnSpider(!currentRoom.isWallCrawlingStage, true);
        }

    }

    //private void OnDestroy()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}
}
#region Planning
/* 
 Planning: 

For this, I am thinking that I can re-use the Room objects that are already made. 
Now, I would just need to put room objects for the WallCrawling scenes, and add in some extra logic to make 
the enemies do the extra stuff.

Essentially, I think that we can have this stuff in a seperate class, and then we can put it on both the Top-Down
and side-scrolling versions of the Wolf Spider. 
The main objectives of this class 
-Record how long the Spider has been in a room
-Once Spider has been in room for longer than TimeLimit
    -Perhaps use RoomManager to get a List of Rooms? 
    -Randomly select a Room from the List of Rooms that connects to the current room
    -Determine the Endpoint to Travel to (If the room you need to go to is North, travel to the NorthExit node, which is already on the Room Object)
    -Travel to that Endpoint


WolfSpiderInstance.GoToNode();



Things to think about:
-How are we going to get the WallCrawling Scenes to Connect with the Top-Down Scenes
    -Perhaps to MouseExit1, MouseExit2, MouseExit3

-Idea: I don't have to make this a Singleton, and all I need to really do is put this in DontDestroyOnLoad();





>>>>>>>>
/ Wall1/
/_xx___/___
| xx      |
|Kitchen  |
|_________|

Wall1:
MouseExit1 = Kitchen

Kitchen:
MouseExit1 = Wall1

Key Details;
1. I fleshed out more of the Design for the Wolf Spider Component.
2. This object will NOT be a Singleton, I will just put it in the DontDestroyOnLoad, which means the object will not
    be destroyed when a new Scene loads. The key detail here is we can have two WolfSpiderRoaming objects placed
    in the first Scene, and then it will be processing in the background.
3. This object would spawn the appripropriate Wolf Spider version (Top-Down or Side) at an entrance node.

Things to Talk about with Michael and Cameron
1. Do we need to have the spider have a Base Room
    Personally, I do not quite think we would really need to have the Spider have a base room. With the Grandmother
    and Cat, we needed to have the act of them returning to their room, because that was how their roomLists were 
    refreshed. However, I think it would be more interesting to 
2. Waypoints I would Need in the Rooms
    I would need certain waypoints in each room that would correlate with what exit they are. The main thing I would
    need to do is put these on the Room objects, as the room object gives a reference to the node at each of the exits.
3. Would we want Spider to be targeted by grandmother/cat
4. Methods I would need for this functionality to work
    Traveling to Waypoint

Things to implement if I have time:
Chasing Xander through different rooms
Having base room and traveling back to it

Variables:


Record What Room the Spider is In
Have RoomList (LinkedList<Room> travelList) that keeps track of the rooms that the Spider has visited

*** I'm thinking we don't have to have the Spider go back to it's home room.
When Spider Travels to a Room
    If (goHome)
        Set Next room to travel to
***



After a certain period of time, have the Spider go back to it's Home Scene 
    Trigger a bool (goHome) that tells the Spider it's time to go home
    Have the Spider start retracing it's steps back through the List of Rooms
    Once the Spider is at it's home room (it can't go back any more rooms)
        Sleep for a bit
    Once stopped Sleeping
        Start traveling to next rooms like normal again.



Implementing Chasing Between Scenes:
When Xander Leaves Room
    if spider is alerted
        if spider is far enough from Xander
            Dont chase Xander
        If close enough to Xander
            Chase Xander to Next Scene 
            
Chasing Xander between Scenes:
Record Distance (distance) between Xander and Alerted Spider
    If Too Far
        Don't have Spider Chase Xander through scene
    else
        Delay spider spawning by ratio based on distance

Cameron will potentially utalize this in the SceneManagement system he built.
Potentially handle Spawning 






+___++++++++++++++++++
If Spider has to leave room
public TimeToLeave()
    nextRoom = DetermineNextRoom();
    LeaveRoom();












Room ideas:+++++++++++++++
Make it so the Room Object will have a List of Room Objects (List<Room> nextRoomList) that the object can travel to.
    Dynamically find this by going through and seeing what names are not null (northScene = Wall1, westScene = null)
    If a name is Not Null
        Find the Room Instance of that object in the roomList array, and then add it to the nextRoomList (type List)


Things I need to discuss:
Show them the UML Diagram I made of how it works
1. Discuss the Traveling to Waypoint (Node) Fucnctionality (TravelToNode(EnemyWaypoint waypoint) method)
2. Talk about Spawning the Enemy
3. Michael: Talk about adding an Enemy Waypoints to each of the Exits in the WallCrawling Scenes
4. Something we will need to do is on the Room objects for every room in the Build, we will need to 
    put the Wall1, Wall2, Wall3, etc. names of the Wall Crawling Scenes, and then we will also need to make it so 
    All of the Wall Crawling scenes are daisy chained together with the Top-Down ones (with the scene names I 
    just mentioned)
5. Talk about how I temporarily omitted the Going Back to Home Room functionality, and Chasing Xander between Scenes
    functionality. I am thinking that we can get it working, get all of the Room objects updated, and then test it.
    Then, we can try and add it. I am almost thinking we should just delay adding that functionality until after 
    Beta. 

*/
#endregion
