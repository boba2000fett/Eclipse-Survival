using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyRoomRoaming : Enemy
{
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

    [Header("Set Dynamically: Enemy Roaming")]
    ////Implement this later when doing adding in the Room change functionality
    //public int currentRoomSee; //Implement this later when doing adding in the Room change functionality
    public int waypointCount = 0;
    public bool isInRoom;
    public float outsideRoomTime;

    public override void Awake()
    {                
        //this.transform.position = go.transform.position;
        GameObject go = GameObject.Find("ExitNode");
        currentWaypointDestination = go.gameObject.GetComponent<Waypoint>();

        Debug.LogWarning("Found");
        if (setRooms)
        {
            roomList = roomListSet;
            currentRoomIndex = currentRoomIndexSet;
        }
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
            currentWaypointDestination = currentWaypointDestination.nextNodeExit;
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

        //if (waypointIndex + 1 > waypoints.Length - 1)
        //{
        //    waypointIndex = 0;
        //}
        //else
        //{
        //    waypointIndex++;
        //}

        //currentWaypointDestination = waypoints[waypointIndex];
    }


    public void CalculateIfInRoom()
    {
        if (roomList[currentRoomIndex].sceneName == SceneManager.GetActiveScene().name)
        {
            //SetPositionComingBackIntoRoom();
            isInRoom = true;
        }
        else
        {
            isInRoom = false;
            this.transform.position = new Vector3(200, 200, 0);
        }
    }

    public void SwitchRoom()
    {
        currentRoomIndex++;
    }

    public void OutsideRoomMoving()
    {
        outsideRoomTime += Time.deltaTime;

        if (outsideRoomTime > outsideRoomTimeInterval)
        {
            SwitchRoom();

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
             */
        GameObject go = GameObject.Find("ExitNode");
        currentWaypointDestination = go.gameObject.GetComponent<Waypoint>();
        transform.position = currentWaypointDestination.gameObject.transform.position;
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

