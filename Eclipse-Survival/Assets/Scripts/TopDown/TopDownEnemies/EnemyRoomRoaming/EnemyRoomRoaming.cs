using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PathToExit
{
    North,
    South,
    West,
    East,
    Stairs,
    Bathroom,
    Wall1,
    Wall2,
    Wall3,
    Wall4,
    Wall5
}

public class EnemyRoomRoaming : Enemy
{
    #region Variables
    static public EnemyRoomRoaming enemySingleton;

    

    [Header("Enemy Roaming : Set in Inspector")]    
    [Tooltip("This variable is to set the roomList.")]    
    public Room[] roomListSet; //This variable is to set the roomList.
    [Tooltip("This variable is only here to display the Room list.")]
    public Room[] seeRoomList; //This variable is only here to display the Room list.
    public bool setRooms = true;
    public int requiredWaypointsCount = 4;
    [Tooltip("This variable is to set current Room Index")]
    private int currentRoomIndexSet;
    [Tooltip("This variable is here to see the currentRoomIndex variable (which cannot be directly seen in the inspector because it is static)")]
    private int seeCurrentRoomIndex;
    public float outsideRoomTimeInterval;

    //These are supposed to be Static
    public int currentRoomIndex;
    public Room[] roomList; //This variable is here because all of the Grandmother Objects will share this.
    public PathToExit pathToExit = PathToExit.North; //This will determine which direction the Grandmother should head in when traveling to the exit
    public string destinationSceneName;
    //These are supposed to be Static
    [Header("Set Room Manager in Inspector")]
    public RoomManager roomManager;

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
    [Tooltip("This will be used to tell the Enemy to go back to it's home node when it is supposed to. \n" +
        "It will only do this if it is in the final room of the cycle.")]
    public bool goHome = false;
    [Tooltip("This will indicate when the Enemy is at their current home node. Here, there will be a certain amout of time that" +
        "passes until the enemy will get up again. \n After that time, the enemy will reset it's room sequence, and begin the sequence anew.")]
    public bool atHome = false;

    public bool resetCollision = false;
    public float resetCollisionTimer = 0;
    public float resetCollisionTimerInterval = 2;

    public float sleepTimerInterval = 5f;
    public float sleepTimer = 0;
    #endregion

    public override void Awake()
    {
        #region Added Singleton Things
        //if (enemySingleton == null)
        //{
        //    //Set the GPM instance
        //    enemySingleton = this;
        //}
        //else if (enemySingleton != this)
        //{
        //    //If the reference has already been set and
        //    //is not the right instance reference, Destroy the GameObject
        //    Destroy(gameObject);
        //}

        ////Do not Destroy this gameobject when a new scene is loaded
        //DontDestroyOnLoad(gameObject);
        #endregion

        Debug.Log(this.gameObject.name);
        if (this.gameObject.name == "Cat")
        {
            roomList = roomListSet;
            return;
        }

        RestartCycle();
        Debug.LogWarning("Found");
    }

    public override void Update()
    {
        if (resetCollision)
        {
            ResetCollision();
        }

        CalculateIfInRoom();

        if (atHome)
        {
            SleepTimer();
            return;
        }

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

    public void ResetCollision()
    {
        resetCollisionTimer += Time.deltaTime;

        if (resetCollisionTimer >= resetCollisionTimerInterval)
        {
            resetCollision = false;
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
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

        if (goHome)
        {
            if (currentWaypointDestination.isHomeNode)
            //This will be used when the Enemy must return to the Homde Node, and has reached it.
            {
                atHome = true;
                //Here we would want to disable the collision and start the timer
            }
            else if (!currentWaypointDestination.isHomeNode)
            //This will be used when the Enemy must return to it's home node (in it's home room), but still has to reach it
            {
                currentWaypointDestination = currentWaypointDestination.nextNodeHome;
            }
            if (currentWaypointDestination.isHomeNode) ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            {
                DisableCollision();
            }
            return;
        }
        
        if (waypointCount >= requiredWaypointsCount && !currentWaypointDestination.isExitNode)
        //This is used when the enemy has reached all of it's required waypoints, and must now go to the exit
        {
            //Here we want to travel to the proper exit
            DetermineExitPath();
            //currentWaypointDestination = currentWaypointDestination.nextNodeEastExit;
        }
        else if (waypointCount >= requiredWaypointsCount && currentWaypointDestination.isExitNode)
        //This is used when the enemy has reached the exitNode and has traveled to all of the required waypoints. 
        //Thus, this makes the enemy switch rooms
        {
            //You have reached the entrance node, so you would switch scenes here.
            SwitchRoom();
        }
        else 
        //This is when the enemy has not traveled to the required waypoints. 
        {

            //Possilby add in fix here ()

            if (currentWaypointDestination.possibleTravelPoints.Length == 0)
            {
                this.GetComponent<BoxCollider2D>().enabled = false;
                currentWaypointDestination.FindPotentialWaypoints();
                this.GetComponent<BoxCollider2D>().enabled = true;
            }

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
        else if (roomList[currentRoomIndex + 1].sceneName == roomList[currentRoomIndex].stairSceneName)
        {
            pathToExit = PathToExit.Stairs;
            currentWaypointDestination = currentWaypointDestination.nextNodeStairsExit;
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

            if (goHome)
            //This will automatically set the atHome variable true, so the Enemy will automatically be at home
            {
                atHome = true;
            }
        }

        if (currentRoomIndex == roomList.Length - 1)
        {
            goHome = true;
            //atHome = true;
        }
    }

    public void SwitchRoom()
    {
        currentRoomIndex++;
    }

    public void OutsideRoomMoving()
    {
        if (atHome || goHome)
        {
            return;
        }
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


    public virtual void SwitchAttackingAnimation(bool setBool)
    {
        if (setBool)
        {
            switch (direction)
            {
                case Facing.Down:
                    anim.SetBool("isAttackingDown", true);
                    break;
                case Facing.Right:
                    anim.SetBool("isAttackingRight", true);
                    break;
                case Facing.Left:
                    anim.SetBool("isAttackingLeft", true);
                    break;
                case Facing.Up:
                    anim.SetBool("isAttackingUp", true);
                    break;
            }
        }
        else
        {
            anim.SetBool("isAttackingDown", false);
            anim.SetBool("isAttackingRight", false);
            anim.SetBool("isAttackingLeft", false);
            anim.SetBool("isAttackingUp", false);

            //switch (direction)
            //{
            //    case Facing.Down:
            //        anim.SetBool("isAttackingDown", false);
            //        break;
            //    case Facing.Right:
            //        anim.SetBool("isAttackingRight", false);
            //        break;
            //    case Facing.Left:
            //        anim.SetBool("isAttackingLeft", false);
            //        break;
            //    case Facing.Up:
            //        anim.SetBool("isAttackingUp", false);
            //        break;
            //}
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
        string name = SceneManager.GetActiveScene().name;
        string enemyisIn = roomList[currentRoomIndex].sceneName;

        Room room = GameObject.FindGameObjectWithTag("Room").GetComponent<Room>();
        this.waypoints = room.waypointsInRoom;
       

        if (atHome || goHome)
        {
            //Disable Collision and Sprite Renderer
            DisableCollision();
            transform.position = room.homeNode.transform.position;
            currentRoomIndex = 0;
            return;
        }

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
                case PathToExit.Stairs:
                    currentWaypointDestination = room.stairsExit;
                    break;
                case PathToExit.Bathroom:
                    currentWaypointDestination = room.bathroomExit;
                    break;

            }
            transform.position = currentWaypointDestination.gameObject.transform.position;
        }       
        else
        {
            //Place at a random waypoint
            //Use room.waypointsInRoom? 
            Room roomInstance = GameObject.FindGameObjectWithTag("Room").GetComponent<Room>();

            int index = UnityEngine.Random.Range(1, roomInstance.spawnWaypoints.Count);

            currentWaypointDestination = roomInstance.spawnWaypoints[index];

            transform.position = currentWaypointDestination.gameObject.transform.position;



            //int index = UnityEngine.Random.Range(1, roomInstance.waypointsInRoom.Length);

            //wolfSpiderTopDownInstance.transform.position = roomInstance.waypointsInRoom[index].gameObject.transform.position;
        }

        //Reset Variables
        waypointCount = 0;
    }

    public void AtHome()
    {
        /* Set timer for how long the Game Object should be disabled*/
    }

    public void SleepTimer()
    {
        
        sleepTimer += Time.deltaTime;
        if (sleepTimer >= sleepTimerInterval)
        {
            RestartCycle();
        }
    }
    public void RestartCycle()
    {
        //roomList = GameObject.Find("RoomManager").GetComponent<RoomManager>().RegenerateRoomList();
        
        //if(atHome)        
        //    currentWaypointDestination = FindObjectOfType<Room>().homeNode;

        roomList = roomManager.RegenerateRoomList();

        atHome = false;
        goHome = false;
        sleepTimer = 0;
        currentRoomIndex = 0;
        resetCollision = true;

        GetComponent<SpriteRenderer>().enabled = true;

        currentWaypointDestination = FindObjectOfType<Room>().homeNode;
    }

    public void DisableCollision()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }


}

#region Planning
/*
______________ Planning ______________
Now I want to implement the Randomization of rounds into the EnemyRoomRoaming class. Essentially I will make the object return back to a "Home" node whenever they are in their
    Base room. For the Grandma, this will be her bed room. 



 */
#endregion

