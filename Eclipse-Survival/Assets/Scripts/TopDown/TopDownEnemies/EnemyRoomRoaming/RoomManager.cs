using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomManager : MonoBehaviour
{
    [Tooltip("This is the RoomManager singleton")]
    static public RoomManager roomManagerSingleton;

    [Header("Set in Inspector: RoomManager")]
    [Tooltip("This will have the total list of rooms, and this class will be used to regenerate the order of rooms traveled to" +
        "\n PUT EVERYTHING HERE EXCEPT THE BASE ROOM")]
    public Room[] totalRoomList;
    [Tooltip("This holds a reference to what is considered the 'base room' for the object. " +
        "\n This is used in the process of regenerating the RoomList for EnemyRoomRoaming")]
    public Room baseRoom;

    [Header("This is only here for testing and seeing the rooms generated by the RegenerateRoomList() method")]
    public Room[] generatedRoomTest;

    [Header("Set Dynamically: RoomManager")]
    public int numberOfRoomsThisSequence;
    public int roomCount;
    public bool headBackToMainRoom;
    public int roomIndex;
    public bool completed;

    private void Awake()
    {
        //if (roomManagerSingleton == null)
        //{
        //    //Set the GPM instance
        //    roomManagerSingleton = this;
        //}
        //else if (roomManagerSingleton != this)
        //{
        //    //If the reference has already been set and
        //    //is not the right instance reference, Destroy the GameObject
        //    Destroy(gameObject);
        //}

        ////Do not Destroy this gameobject when a new scene is loaded
        //DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    generatedRoomTest = RegenerateRoomList();
        //}
    }

    public Room[] RegenerateRoomList()
    {
        LinkedList<Room> roomList = new LinkedList<Room>();
        numberOfRoomsThisSequence = Random.Range(2, totalRoomList.Length);

        roomCount = 0;
        headBackToMainRoom = false;
        completed = false;
        roomIndex = 0;

        roomList.AddLast(baseRoom);
        roomCount++;

        while (!completed)
        {
            if (roomCount > (numberOfRoomsThisSequence / 2))
            {
                headBackToMainRoom = true;
            }

            if (!headBackToMainRoom)
            {
                roomIndex = Random.Range(0, totalRoomList.Length);

                if (totalRoomList[roomIndex].sceneName == roomList.Last.Value.southSceneName ||
                    totalRoomList[roomIndex].sceneName == roomList.Last.Value.westSceneName ||
                    totalRoomList[roomIndex].sceneName == roomList.Last.Value.northSceneName ||
                    totalRoomList[roomIndex].sceneName == roomList.Last.Value.eastSceneName ||
                    totalRoomList[roomIndex].sceneName == roomList.Last.Value.stairSceneName)
                {
                    roomList.AddLast(totalRoomList[roomIndex]);
                    roomCount++;
                }
            }
            else
            {
                bool tempCompleted = false;
                Room[] tempArray = roomList.ToArray();
                roomIndex = tempArray.Length - 1;

                while (!tempCompleted)
                {
                    if (roomIndex > 0 )
                    {
                        roomIndex--;
                        roomList.AddLast(tempArray[roomIndex]);                        
                    }
                    else
                    {
                        tempCompleted = true;
                        completed = true;
                        break;
                    }
                }
            }
        }

        return roomList.ToArray();
    }
}

