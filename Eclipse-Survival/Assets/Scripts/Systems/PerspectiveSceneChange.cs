using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PerspectiveSceneChange : MonoBehaviour
{
    [Header("Set in Inspector")]
    public string targetPerspectiveScene;
    public string targetPerspectiveSpawnPoint;
    public DirectionKey transitionKeyNeeded;
    public string nameOfItem;
    public GameObject spawnPoint;

    [Header("Set Dynamically")]
    public string[] tempArray;
    GameObject[] tempItemHolder;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Xander")
        {
            if (PressingEntranceKey())
            {
                GameObject go = GameObject.Find("ItemSpawnPlaceholder");
                ItemSpawner temp = go.GetComponent<ItemSpawner>();
                int i = 0;

                //A switch case for identifying the current room name, which is set in the inspector
                switch (temp.roomName)
                {
                    case "MasterBedroom":

                        GameObject[] tempItemHolder = GameObject.FindGameObjectsWithTag("Item");
                        GamePlayManager.GPM.itemsLeftInMasterBedRoomType = new string[tempItemHolder.Length];
                        GamePlayManager.GPM.itemsLeftInMasterBedRoomLocation = new Vector2[tempItemHolder.Length];


                        foreach (GameObject item in tempItemHolder)
                        {


                            nameOfItem = item.gameObject.name;

                            tempArray = nameOfItem.Split(char.Parse("("));

                            GamePlayManager.GPM.itemsLeftInMasterBedRoomType[i] = tempArray[0];
                            GamePlayManager.GPM.itemsLeftInMasterBedRoomLocation[i] = item.gameObject.transform.position;

                            i++;
                        }

                        break;

                    case "Kitchen":
                        tempItemHolder = GameObject.FindGameObjectsWithTag("Item");

                        GamePlayManager.GPM.itemsLeftInKitchenRoomType = new string[tempItemHolder.Length];
                        GamePlayManager.GPM.itemsLeftInKitchenRoomLocation = new Vector2[tempItemHolder.Length];

                        foreach (GameObject item in tempItemHolder)
                        {


                            nameOfItem = item.gameObject.name;

                            tempArray = nameOfItem.Split(char.Parse("("));

                            GamePlayManager.GPM.itemsLeftInKitchenRoomType[i] = tempArray[0];
                            GamePlayManager.GPM.itemsLeftInKitchenRoomLocation[i] = item.gameObject.transform.position;

                            i++;
                        }
                        break;

                    case "DiningRoom":
                        tempItemHolder = GameObject.FindGameObjectsWithTag("Item");
                        GamePlayManager.GPM.itemsLeftInDinningRoomType = new string[tempItemHolder.Length];
                        GamePlayManager.GPM.itemsLeftInDinningRoomLocation = new Vector2[tempItemHolder.Length];


                        foreach (GameObject item in tempItemHolder)
                        {


                            nameOfItem = item.gameObject.name;

                            tempArray = nameOfItem.Split(char.Parse("("));

                            GamePlayManager.GPM.itemsLeftInDinningRoomType[i] = tempArray[0];
                            GamePlayManager.GPM.itemsLeftInDinningRoomLocation[i] = item.gameObject.transform.position;

                            i++;
                        }
                        break;

                    case "LivingRoom":
                        tempItemHolder = GameObject.FindGameObjectsWithTag("Item");
                        GamePlayManager.GPM.itemsLeftInLivingRoomType = new string[tempItemHolder.Length];
                        GamePlayManager.GPM.itemsLeftInLivingRoomLocation = new Vector2[tempItemHolder.Length];


                        foreach (GameObject item in tempItemHolder)
                        {


                            nameOfItem = item.gameObject.name;

                            tempArray = nameOfItem.Split(char.Parse("("));

                            GamePlayManager.GPM.itemsLeftInLivingRoomType[i] = tempArray[0];
                            GamePlayManager.GPM.itemsLeftInLivingRoomLocation[i] = item.gameObject.transform.position;

                            i++;
                        }
                        break;

                    case "FirstFloorHallway":
                        tempItemHolder = GameObject.FindGameObjectsWithTag("Item");
                        GamePlayManager.GPM.itemsLeftInFirstFloorHallwayType = new string[tempItemHolder.Length];
                        GamePlayManager.GPM.itemsLeftInFirstFloorHallwayLocation = new Vector2[tempItemHolder.Length];


                        foreach (GameObject item in tempItemHolder)
                        {


                            nameOfItem = item.gameObject.name;

                            tempArray = nameOfItem.Split(char.Parse("("));

                            GamePlayManager.GPM.itemsLeftInFirstFloorHallwayType[i] = tempArray[0];
                            GamePlayManager.GPM.itemsLeftInFirstFloorHallwayLocation[i] = item.gameObject.transform.position;

                            i++;
                        }
                        break;

                    case "GrandKidsBedRoom":
                        tempItemHolder = GameObject.FindGameObjectsWithTag("Item");
                        GamePlayManager.GPM.itemsLeftInGrandKidsBedRoomType = new string[tempItemHolder.Length];
                        GamePlayManager.GPM.itemsLeftInGrandKidsBedRoomLocation = new Vector2[tempItemHolder.Length];


                        foreach (GameObject item in tempItemHolder)
                        {


                            nameOfItem = item.gameObject.name;

                            tempArray = nameOfItem.Split(char.Parse("("));

                            GamePlayManager.GPM.itemsLeftInGrandKidsBedRoomType[i] = tempArray[0];
                            GamePlayManager.GPM.itemsLeftInGrandKidsBedRoomLocation[i] = item.gameObject.transform.position;

                            i++;
                        }
                        break;

                    case "GuestBedRoom1":
                        tempItemHolder = GameObject.FindGameObjectsWithTag("Item");
                        GamePlayManager.GPM.itemsLeftInGuestBedRoomOneType = new string[tempItemHolder.Length];
                        GamePlayManager.GPM.itemsLeftInGuestBedRoomOneLocation = new Vector2[tempItemHolder.Length];


                        foreach (GameObject item in tempItemHolder)
                        {


                            nameOfItem = item.gameObject.name;

                            tempArray = nameOfItem.Split(char.Parse("("));

                            GamePlayManager.GPM.itemsLeftInGuestBedRoomOneType[i] = tempArray[0];
                            GamePlayManager.GPM.itemsLeftInGuestBedRoomOneLocation[i] = item.gameObject.transform.position;

                            i++;
                        }
                        break;

                    case "GuestBedRoom2":
                        tempItemHolder = GameObject.FindGameObjectsWithTag("Item");
                        GamePlayManager.GPM.itemsLeftInGuestBedRoomTwoType = new string[tempItemHolder.Length];
                        GamePlayManager.GPM.itemsLeftInGuestBedRoomTwoLocation = new Vector2[tempItemHolder.Length];


                        foreach (GameObject item in tempItemHolder)
                        {


                            nameOfItem = item.gameObject.name;

                            tempArray = nameOfItem.Split(char.Parse("("));

                            GamePlayManager.GPM.itemsLeftInGuestBedRoomTwoType[i] = tempArray[0];
                            GamePlayManager.GPM.itemsLeftInGuestBedRoomTwoLocation[i] = item.gameObject.transform.position;

                            i++;
                        }
                        break;

                    case "Bathroom":
                        tempItemHolder = GameObject.FindGameObjectsWithTag("Item");
                        GamePlayManager.GPM.itemsLeftInBathRoomType = new string[tempItemHolder.Length];
                        GamePlayManager.GPM.itemsLeftInBathRoomLocation = new Vector2[tempItemHolder.Length];


                        foreach (GameObject item in tempItemHolder)
                        {


                            nameOfItem = item.gameObject.name;

                            tempArray = nameOfItem.Split(char.Parse("("));

                            GamePlayManager.GPM.itemsLeftInBathRoomType[i] = tempArray[0];
                            GamePlayManager.GPM.itemsLeftInBathRoomLocation[i] = item.gameObject.transform.position;

                            i++;
                        }
                        break;

                    case "SecondFloorHallway":
                        tempItemHolder = GameObject.FindGameObjectsWithTag("Item");
                        GamePlayManager.GPM.itemsLeftInSecondFloorHallwayType = new string[tempItemHolder.Length];
                        GamePlayManager.GPM.itemsLeftInSecondFloorHallwayLocation = new Vector2[tempItemHolder.Length];


                        foreach (GameObject item in tempItemHolder)
                        {


                            nameOfItem = item.gameObject.name;

                            tempArray = nameOfItem.Split(char.Parse("("));

                            GamePlayManager.GPM.itemsLeftInSecondFloorHallwayType[i] = tempArray[0];
                            GamePlayManager.GPM.itemsLeftInSecondFloorHallwayLocation[i] = item.gameObject.transform.position;

                            i++;
                        }
                        break;
                }

                //if (SceneManager.GetActiveScene().buildIndex == 5)
                //{
                //    SceneManager.LoadScene(13);
                //}
                //else
                //{
                //    SceneManager.LoadScene(5);
                //}
                GamePlayManager.GPM.targetTag = targetPerspectiveSpawnPoint;
                //Physics2D.gravity = new Vector2(0, 0);
                SceneManager.LoadScene(targetPerspectiveScene);
            }
        }           
    }

    private bool PressingEntranceKey()
    {
        if (transitionKeyNeeded == DirectionKey.Up && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
        {
            return true;
        }
        else if (transitionKeyNeeded == DirectionKey.Down && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
        {
            return true;
        }
        else if (transitionKeyNeeded == DirectionKey.Right && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
        {
            return true;
        }
        else if (transitionKeyNeeded == DirectionKey.Left && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public enum DirectionKey
{
    Up,
    Down,
    Left,
    Right
}


