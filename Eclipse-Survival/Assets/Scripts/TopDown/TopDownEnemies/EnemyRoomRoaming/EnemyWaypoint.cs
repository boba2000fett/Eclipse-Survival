using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyWaypoint : MonoBehaviour
{
    [Header("Set in Inspector: EnemyWaypoint")]
    public EnemyWaypoint[] possibleTravelPoints;
    public EnemyWaypoint[] pathToExit;
    public EnemyWaypoint nextNodeSouthExit;
    public EnemyWaypoint nextNodeNorthExit;
    public EnemyWaypoint nextNodeWestExit;
    public EnemyWaypoint nextNodeEastExit;
    public EnemyWaypoint nextNodeStairsExit;
    public EnemyWaypoint nextNodeBathroomExit;
    [Header("This will only be used in the scene where the home is.")]
    public EnemyWaypoint nextNodeHome;
    //public Waypoint nextNodeSouthExit;

    public bool isExitNode;
    public bool isHomeNode = false;

    [Header("Set Dynamically: EnemyWaypoint")]
    public bool completedWaypoint = false;
    /*
    When arriving at node 1, the grandmother would then travel to either 2 or 4 (it would pick randomly)
    _______________
    |1   2       6|
    | XXX  4   7  |
    | XXX  9   0  |
    |4   3       5|
    =========10=====

    *****Node 1 Example*****
Node 1:
possibleTravelPoints: Node 2 and Node 4
pathToExit: Node 4, Node 3, Node 10
 */

    public void Awake()
    {
        FindPotentialWaypoints();
    }

    public void FindPotentialWaypoints()
    {
        if (isHomeNode)
            return;

        Camera.main.GetComponent<CameraFollow>().enabled = false;
        GameObject.Find("MapBounds").gameObject.GetComponent<BoxCollider2D>().enabled = false;

        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent
        GameObject[] waypointsInScene = GameObject.FindGameObjectsWithTag("EnemyWaypoint");

        LinkedList<EnemyWaypoint> availableNodes = new LinkedList<EnemyWaypoint>();

        this.GetComponent<BoxCollider2D>().enabled = false;

        foreach (GameObject waypoint in waypointsInScene)
        {
            if (waypoint == this.gameObject)
                continue;

            Ray ray = new Ray(transform.position, waypoint.transform.position);

            RaycastHit2D hit = Physics2D.Raycast
                (transform.position,
                waypoint.transform.position - transform.position);


            if (hit.collider.gameObject.tag == "EnemyWaypoint")
            {
                if (hit.collider.gameObject.GetComponent<EnemyWaypoint>().isExitNode ||
                    hit.collider.gameObject.GetComponent<EnemyWaypoint>().isHomeNode ||
                    availableNodes.Find(hit.collider.gameObject.GetComponent<EnemyWaypoint>()) != null)
                {
                    continue;
                }

                availableNodes.AddLast(hit.collider.gameObject.GetComponent<EnemyWaypoint>());

            }
        }

        this.GetComponent<BoxCollider2D>().enabled = true;

        possibleTravelPoints = availableNodes.ToArray();
        //Main Camera/MapBounds
        GameObject.Find("MapBounds").gameObject.GetComponent<BoxCollider2D>().enabled = true;
        Camera.main.GetComponent<CameraFollow>().enabled = true;

        completedWaypoint = true;
    }

        
}
