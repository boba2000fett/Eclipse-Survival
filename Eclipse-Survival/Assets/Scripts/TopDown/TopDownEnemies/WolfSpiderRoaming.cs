using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpiderRoaming : MonoBehaviour
{
    



}

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


+___++++++++++++++++++
If Spider has to leave room
DetermineNextRoom();
LeaveRoom();


public void DetermineNextRoom()
    nextRoom = roomManager.FindNextRoomSpider();

RoomManager Class
public Room FindNextRoomSpider()
    completed = false;
    
    while(!completed)
    {
        roomIndex = Random.Range(0, totalRoomList.Length);

        if (spiderRoomList[roomIndex].sceneName == roomList.Last.Value.southSceneName ||
            spiderRoomList[roomIndex].sceneName == roomList.Last.Value.westSceneName ||
            spiderRoomList[roomIndex].sceneName == roomList.Last.Value.northSceneName ||
            spiderRoomList[roomIndex].sceneName == roomList.Last.Value.eastSceneName ||
            spiderRoomList[roomIndex].sceneName == roomList.Last.Value.stairSceneName ||
            spiderRoomList[roomIndex].sceneName == roomList.Last.Value.wall1SceneName ||
            spiderRoomList[roomIndex].sceneName == roomList.Last.Value.wall2SceneName ||
            spiderRoomList[roomIndex].sceneName == roomList.Last.Value.wall3SceneName ||
            spiderRoomList[roomIndex].sceneName == roomList.Last.Value.wall4SceneName ||
            spiderRoomList[roomIndex].sceneName == roomList.Last.Value.wall5SceneName)
        {
            return totalRoomList[roomIndex];
        }
    
    }





public void LeaveRoom()
if(ExitTime)
    EnemyWaypoint waypoint;
    Room room = FindObjectOfType("Room")    

    if(NextRoom.sceneName = currentRoom.northSceneName)
    {
        pathToExit = PathToExit.North;
        waypointDestination = Room.northExit;
    }
    if(NextRoom.sceneName = currentRoom.westSceneName)
    {
        pathToExit = PathToExit.West;
        waypointDestination = Room.westExit;
    }
    if(NextRoom.sceneName = currentRoom.eastSceneName)
    {
        pathToExit = PathToExit.East;
        waypointDestination = Room.eastExit;
    }
    if(NextRoom.sceneName = currentRoom.SouthSceneName)
    {
        pathToExit = PathToExit.South;
        waypointDestination = Room.southExit;
    }
    if(NextRoom.sceneName = currentRoom.Wall1SceneName)
    {
        pathToExit = PathToExit.Wall1;
        waypointDestination = Room.wall1Exit;
    }
    if(NextRoom.sceneName = currentRoom.Wall2SceneName)
    {
        pathToExit = PathToExit.Wall2;
        waypointDestination = Room.wall1Exit;
    }

    wolfSpiderInstance.TravelTo(waypointDestination);







*/
