using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    GameObject[] possibleTravelPoints;
    GameObject[] pathToExit;
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
