using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour
{
    [Header("Set In Inspector: Room")]
    public string sceneName; //This will be the name of the Scene that this "Room" is in
    public int roomID; //This will be the build index of the Scene that this "Room" is in

    private void Start()
    {
        //roomID = SceneManager.GetActiveScene().buildIndex;
        //sceneName = SceneManager.GetActiveScene().name;
    }
}
