using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PerspectiveSceneChange : MonoBehaviour
{
    [Header("Set in Inspector")]
    public string targetPerspectiveScene;
    public string targetPerspectiveSpawnPoint;
    public string nameOfItem;
    public GameObject spawnPoint;

    [Header("Set Dynamically")]
    public string[] tempArray;
    GameObject[] tempItemHolder;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Xander")
        {
            GameObject go = GameObject.Find("ItemSpawnPlaceholder");
            ItemSpawner temp = go.GetComponent<ItemSpawner>();
            int i = 0;

            //A switch case for identifying the current room name, which is set in the inspector
            switch (temp.roomName)
            {
                case "Bedroom":

                    GameObject[] tempItemHolder = GameObject.FindGameObjectsWithTag("Item");
                    GamePlayManager.GPM.itemsLeftInBedRoomType = new string[tempItemHolder.Length];
                    GamePlayManager.GPM.itemsLeftInBedRoomLocation = new Vector2[tempItemHolder.Length];


                    foreach (GameObject item in tempItemHolder)
                    {


                        nameOfItem = item.gameObject.name;

                        tempArray = nameOfItem.Split(char.Parse("("));

                        GamePlayManager.GPM.itemsLeftInBedRoomType[i] = tempArray[0];
                        GamePlayManager.GPM.itemsLeftInBedRoomLocation[i] = item.gameObject.transform.position;

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
            SceneManager.LoadScene(targetPerspectiveScene);
        }
    }
}
