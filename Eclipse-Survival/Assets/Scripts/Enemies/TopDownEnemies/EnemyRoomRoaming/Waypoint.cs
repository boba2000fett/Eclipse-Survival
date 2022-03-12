using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Set in Inspector: Waypoint")]
    public Waypoint[] possibleTravelPoints;
    public Waypoint[] pathToExit;
    public Waypoint nextNodeSouthExit;
    public Waypoint nextNodeNorthExit;
    public Waypoint nextNodeWestExit;
    public Waypoint nextNodeEastExit;
    [Header("This will only be used in the scene where the home is.")]
    public Waypoint nextNodeHome;
    //public Waypoint nextNodeSouthExit;

    public bool isExitNode;
    public bool isHomeNode = false;
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

}
