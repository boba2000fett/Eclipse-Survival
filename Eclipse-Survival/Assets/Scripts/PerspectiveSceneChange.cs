using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PerspectiveSceneChange : MonoBehaviour
{
    [Header("Set in Inspector")]
    public string targetPerspectiveScene;
    public string nameOfItem;

    [Header("Set Dynamically")]
    public string[] tempArray;
    GameObject[] tempItemHolder;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Xander")
        {

            GameObject go = GameObject.Find("ItemSpawnPlaceholder");
            ItemSpawner temp = go.GetComponent<ItemSpawner>();
            int i = 0;

            //A switch case for identifying the current room name, which is set in the inspector
            switch (temp.roomName)
            {
                case "mainRoom1":
                    
                    GameObject[] tempItemHolder = GameObject.FindGameObjectsWithTag("Item");
                    GamePlayManager.GPM.itemsLeftInMainRoom1Type = new string[tempItemHolder.Length];
                    GamePlayManager.GPM.itemsLeftInMainRoom1Location = new Vector2[tempItemHolder.Length];


                    foreach(GameObject item in tempItemHolder)
                    {


                        nameOfItem = item.gameObject.name;

                        tempArray = nameOfItem.Split(char.Parse("("));

                        GamePlayManager.GPM.itemsLeftInMainRoom1Type[i] = tempArray[0];
                        GamePlayManager.GPM.itemsLeftInMainRoom1Location[i] = item.gameObject.transform.position;

                        i++;
                    }

                    break;

                case "mainRoom2":
                    tempItemHolder = GameObject.FindGameObjectsWithTag("Item");

                    GamePlayManager.GPM.itemsLeftInMainRoom2Type = new string[tempItemHolder.Length];
                    GamePlayManager.GPM.itemsLeftInMainRoom2Location = new Vector2[tempItemHolder.Length];

                    foreach (GameObject item in tempItemHolder)
                    {


                        nameOfItem = item.gameObject.name;

                        tempArray = nameOfItem.Split(char.Parse("("));

                        GamePlayManager.GPM.itemsLeftInMainRoom2Type[i] = tempArray[0];
                        GamePlayManager.GPM.itemsLeftInMainRoom2Location[i] = item.gameObject.transform.position;

                        i++;
                    }
                    break;
            }

            if (SceneManager.GetActiveScene().buildIndex == 5)
            {
                SceneManager.LoadScene(13);
            }
            else
            {
                SceneManager.LoadScene(5);
            }
        }
    }

}
