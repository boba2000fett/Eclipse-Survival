using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyRoomRoaming : Enemy
{
    public enum PathToExit
    {
        North,
        South,
        West,
        East
    }

    [Header("Enemy Roaming : Set in Inspector")]    
    [Tooltip("This variable is to set the roomList.")]    
    public Room[] roomListSet; //This variable is to set the roomList.
    [Tooltip("This variable is only here to display the Room list.")]
    public Room[] seeRoomList; //This variable is only here to display the Room list.
    public bool setRooms = true;
    public int requiredWaypointsCount = 4;
    [Tooltip("This variable is to set current Room Index")]
    public int currentRoomIndexSet;
    [Tooltip("This variable is here to see the currentRoomIndex variable (which cannot be directly seen in the inspector because it is static)")]
    public int seeCurrentRoomIndex;
    public float outsideRoomTimeInterval;

    static public int currentRoomIndex;
    static public Room[] roomList; //This variable is here because all of the Grandmother Objects will share this.
    static public PathToExit pathToExit = PathToExit.North; //This will determine which direction the Grandmother should head in when traveling to the exit
    static public string destinationSceneName;

    [Header("Set Dynamically: Enemy Roaming")]
    //public int currentRoomSee; //Implement this later when doing adding in the Room change functionality
    public int waypointCount = 0;
    public bool isInRoom;
    public float outsideRoomTime;
    [Tooltip("This will determine which direction the Grandmother should head in when traveling to the exit.")]
    public PathToExit pathToExitSee = PathToExit.North; //This will determine which direction the Grandmother should head in when traveling to the exit
    [Tooltip("This determines if the enemy is active in the Current scene. If it is not, then move the Enemy to the appropriate position")]
    public bool isActive = false;
    [Tooltip("This determines the name of the destination scene that an Enemy is traveling to next when the Enemy is not in the current room. This will be used to determine if the enemy should be placed at one of the exits (The enemy is coming from a neighboring room to the current room), or placed at a Random position within the room (the player is enters a room where the Enemy is). \n Note: The main instance of this variable is static, so this variable is here so we can see the other one in the Inspector")]
    public string destinationSceneNameSee;


    public override void Awake()
    {
        ////this.transform.position = go.transform.position;
        //GameObject go = GameObject.Find("SouthExitNode");
        //currentWaypointDestination = go.gameObject.GetComponent<Waypoint>();

        roomList = roomListSet;
        if (setRooms)
        {
            //roomList = roomListSet;
            currentRoomIndex = currentRoomIndexSet;
        }

        CalculateIfInRoom();

        if (isInRoom)
        {
            SetPositionComingBackIntoRoom();
        }


        Debug.LogWarning("Found");
       
    }

    public override void Update()
    {
        CalculateIfInRoom();

        SeeStaticVariables();

        if (isInRoom)
        {
            base.Update();
        }
        else
        {
            OutsideRoomMoving();
        }
        
    }

    public void SeeStaticVariables()
    {
        seeRoomList = roomList;
        seeCurrentRoomIndex = currentRoomIndex;
        pathToExitSee = pathToExit;
        destinationSceneNameSee = destinationSceneName;
    }

    public override void RegularMove()
    {
        if (transform.position == currentWaypointDestination.gameObject.transform.position ||
            currentWaypointDestination == null)
        {
            DetermineDestination();
        }

        transform.position = Vector2.MoveTowards(transform.position, currentWaypointDestination.gameObject.transform.position, moveSpeed * Time.deltaTime);

    }

    public override void DetermineDestination()
    {
        int nextNodeIndex;
        if (waypoints.Length == 0)
            return;

        if (waypointCount >= requiredWaypointsCount && !currentWaypointDestination.isExitNode)
        {
            //Here we want to travel to the proper exit
            DetermineExitPath();
            //currentWaypointDestination = currentWaypointDestination.nextNodeEastExit;
        }
        else if (waypointCount >= requiredWaypointsCount && currentWaypointDestination.isExitNode)
        {
            //You have reached the entrance node, so you would switch scenes here.
            SwitchRoom();
        }
        else //You have not traveled to all of the required waypoints
        {
            nextNodeIndex = Random.Range(0, currentWaypointDestination.possibleTravelPoints.Length);
            currentWaypointDestination = currentWaypointDestination.possibleTravelPoints[nextNodeIndex];
            waypointCount++;
        }
    }

    public void DetermineExitPath()
    {
        if (currentRoomIndex == roomList.Length - 1) 
            return; //Here I will figure out the extra behavior when the Grandmother returns to the first room of the sequence

        if (roomList[currentRoomIndex + 1].sceneName == roomList[currentRoomIndex].northSceneName)
        {
            pathToExit = PathToExit.North;
            currentWaypointDestination = currentWaypointDestination.nextNodeNorthExit;
        }
        else if (roomList[currentRoomIndex + 1].sceneName == roomList[currentRoomIndex].southSceneName)
        {
            pathToExit = PathToExit.South;
            currentWaypointDestination = currentWaypointDestination.nextNodeSouthExit;
        }
        else if (roomList[currentRoomIndex + 1].sceneName == roomList[currentRoomIndex].westSceneName)
        {
            pathToExit = PathToExit.West;
            currentWaypointDestination = currentWaypointDestination.nextNodeWestExit;
        }
        else if (roomList[currentRoomIndex + 1].sceneName == roomList[currentRoomIndex].eastSceneName)
        {
            pathToExit = PathToExit.East;
            currentWaypointDestination = currentWaypointDestination.nextNodeEastExit;
        }
    }

    public void CalculateIfInRoom()
    {
        if (roomList[currentRoomIndex].sceneName == SceneManager.GetActiveScene().name)
        {
            if (!isActive)
            {
                SetPositionComingBackIntoRoom();
                isActive = true;
            }            
            isInRoom = true;
        }
        else
        {
            isInRoom = false;
            this.transform.position = new Vector3(200, 200, 0);
            isActive = false;
        }
    }

    public void SwitchRoom()
    {
        currentRoomIndex++;
    }

    public void OutsideRoomMoving()
    {
        outsideRoomTime += Time.deltaTime;
        destinationSceneName = roomList[currentRoomIndex + 1].sceneName;

        if (roomList[currentRoomIndex + 1].sceneName == roomList[currentRoomIndex].northSceneName)
        {
            pathToExit = PathToExit.North;
        }
        else if (roomList[currentRoomIndex + 1].sceneName == roomList[currentRoomIndex].southSceneName)
        {
            pathToExit = PathToExit.South;
        }
        else if (roomList[currentRoomIndex + 1].sceneName == roomList[currentRoomIndex].westSceneName)
        {
            pathToExit = PathToExit.West;
        }
        else if (roomList[currentRoomIndex + 1].sceneName == roomList[currentRoomIndex].eastSceneName)
        {
            pathToExit = PathToExit.East;
        }

        if (outsideRoomTime > outsideRoomTimeInterval)
        {
            SwitchRoom();
            outsideRoomTime = 0;
        }
    }

    public void SetPositionComingBackIntoRoom()
    {
        /*Here we will want to place the Grandmother object at the correct node. This might
             prove to be a challenge because we have to know which direction the grandma is traveling from. 
            For instance, 
            |-----||--N--|
            |  1  EW  2  |
            |-----||--S--|
             When traveling from Room 1 to Room 2, we will need to know to place the Grandmother at the W Node. 
        
     Update 2/13/2022
        Here, we would need to determine if the Enemy is either entering the room you are currently in, or if
        you are entering a room that the enemy is in.

         
         */
        Room room = GameObject.FindGameObjectWithTag("Room").GetComponent<Room>();
        this.waypoints = room.waypointsInRoom;

        if (destinationSceneName == SceneManager.GetActiveScene().name)
        {
            //Place at an Endpoint pertaining to the direction the Enemy is Traveling From
            switch(pathToExit)
            {
                case PathToExit.North:
                    currentWaypointDestination = room.southExit;
                    break;
                case PathToExit.East:
                    currentWaypointDestination = room.westExit;
                    break;
                case PathToExit.West:
                    currentWaypointDestination = room.eastExit;
                    break;
                case PathToExit.South:
                    currentWaypointDestination = room.northExit;
                    break;

            }
            transform.position = currentWaypointDestination.gameObject.transform.position;
        }
        else
        {
            //Place at a random waypoint
            int index = Random.Range(1, waypoints.Length);

            currentWaypointDestination = waypoints[index];
            transform.position = currentWaypointDestination.gameObject.transform.position;
        }

        //Reset Variables
        waypointCount = 0;
    }

}

#region Planning
/*
Planning 
Options for this code with the Grandma and Cat
-MAKE CAT AND GRANDMOTHER INHERIT FROM A DIFFERENT CLASS CALLED ENEMYROAMING, WHICH WILL INHERIT FROM ENEMY.
    THEN, THE CAT AND GRANDMA CAN SHARE THAT SAME BEHAVIOR WITHOUT CLOGGING DOWN BASE ENEMY SCRIPT WITH THINGS THAT WOULD BE
    OVERRIDEN WITH WOLFSPIDER AND HOUSE RAT



Goal: Make Grandmother be able to switch between Scenes in a sequence. Then, the grandmother will go back
to her room for a few in-game hours.
Then, the grandmother will also be able to automatically switch to the GrandmotherWaypoints object in the scene.

Make grandma visit a random number of nodes in the room, and then she will leave
-Make Waypoints have a script that has a simple list of possible waypoints to travel to. 
-Make a Waypoint on the door, which means that the Grandmother will be set at that position.

Grandma requiredWaypoints = Random number between 0 and Number of waypoints in current room
    (Do get that, find the game object with the name of GrandmotherWaypoints, and get the number of children)

If Grandma reaches a waypoint
    requiredWaypoints++;
    if reached enough waypoints
        travel to one of the random available nodes
    else
        then make way to exit (Depending on Room you are in)

-If Grandmother must travel to exit, then they will use the list from their currentWaypoint called pathToExit. 
    This path will give the Grandmother the shortest path to the exit, this will need to be set manually in the inspector.    

Movement Pattern/Pathfinding Update
Update the Grandmothers Movement pattern to make it more randomized
When arriving at node 1, the grandmother would then travel to either 2 or 4 (it would pick randomly)
    _______12______
    |1   2       6|
    | XXX  4   7  |
   13 XXX         11      
    | XXX  9   0  |
    |4   3       5|
    =======10======

Waypoint Script:
GameObject[] possibleTravelPoints;
GameObject[] pathToExit;

*****Node 1 Example*****
       :Node 1:
possibleTravelPoints: Node 2 and Node 4
pathToSouthExit: Node 4, Node 3, Node 10
pathToNorthExit: Node 2, Node 12
pathToEastExit: Node 2, Node 7, Node 11
pathToWestExit: Node 13



Next problem: The grandmother must be able to know which exit to travel to. 
Figuring out rooms
The roomTheGrandmother is in will be a static variable. 




Grandmother sequence. 

If (Grandma reaches end node)
    NextRoom()

NextRoom(): This will switch the Grandma to the next

Next Thing To DO:
Make Grandmother go to the Correct Waypoint when re-entering the room

 */
#endregion

