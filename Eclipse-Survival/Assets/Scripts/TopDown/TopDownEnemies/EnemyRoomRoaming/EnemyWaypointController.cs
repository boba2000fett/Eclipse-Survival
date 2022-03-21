using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyWaypointController : MonoBehaviour
{
    public EnemyWaypoint[] waypointList;


    public void Start()
    {
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("TrentBedroom1");
        }
    }
   
}

/* 

 */
