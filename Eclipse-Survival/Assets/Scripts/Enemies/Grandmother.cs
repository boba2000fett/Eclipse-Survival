using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grandmother : Enemy
{
    [Header("Set in Inspector: Grandmother")]
    public float hittingDuration = 0.5f;
    public float fryingPanStrength;    

    [Header("Set Dynamically: Grandmother")]
    public GameObject fryingPan;
    public bool isHitting;
    public float hittingTime = 0f;

    [HideInInspector]
    public Vector2 leftFacingPositionFryingPan = new Vector2(-15, 0);
    public Vector2 rightFacingPositionFryingPan = new Vector2(15, 0);
    public Vector2 upFacingPositionFryingPan = new Vector2(0, 18);
    public Vector2 downFacingPositionFryingPan = new Vector2(0, -18);
    public Vector2 standardPositionFryingPan = new Vector2(0, 0);


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void Awake()
    {
        base.Awake();
        fryingPan = this.gameObject.transform.GetChild(0).gameObject;
        fryingPan.transform.localPosition = standardPositionFryingPan;
        fryingPan.SetActive(false);        
        Damage fryingPanDamage = fryingPan.GetComponent<Damage>();
        fryingPanDamage.strength = fryingPanStrength;

        isHitting = false;
        hittingTime = 0f;
    }

    public override void Update()
    {
        base.Update();        

        if (isHitting)
        {
            UpdateFryingPanPosition();
            CheckFryingPan();
        }
    }

    public override void AlertMoveTowards()
    {
        //Possibly add in more conditions to make the object leave alert phase, like checking if the target left the room
        if (target == null || (alertTime > alertTimeDuration))
        {
            isAlerted = false;
        }

        Vector2 distanceFromTarget = target.gameObject.transform.position - transform.position;

        Debug.Log(distanceFromTarget.magnitude);

        if (distanceFromTarget.magnitude > attackRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.gameObject.transform.position, runSpeed * Time.deltaTime);
        }
        else 
        {
            StrikeFryingPan();            
        }        
    }

    public void StrikeFryingPan()
    {
        UpdateFryingPanPosition();
        fryingPan.SetActive(true);
        isHitting = true;
    }


    public void UpdateFryingPanPosition()
    {
        if (this.direction == Facing.Down)
            fryingPan.transform.localPosition = downFacingPositionFryingPan;
        else if (this.direction == Facing.Up)
            fryingPan.transform.localPosition = upFacingPositionFryingPan;
        else if (this.direction == Facing.Right)
            fryingPan.transform.localPosition = rightFacingPositionFryingPan;
        else if (this.direction == Facing.Left)
            fryingPan.transform.localPosition = leftFacingPositionFryingPan;
    }

    public void CheckFryingPan()
    {
        hittingTime += Time.deltaTime;

        if (hittingTime > hittingDuration)
        {
            isHitting = false;
            hittingTime = 0f;
            fryingPan.SetActive(false);
        }
    }

}
#region Planning
/*
Planning 
Options for this code with the Grandma and Cat
-POSSIBLY IMPLEMENT THIS IN THE BASE ENEMY CLASS
-JUST COPY IT TO THE CAT,
-OR MAKE CAT AND GRANDMOTHER INHERIT FROM A DIFFERENT CLASS CALLED ENEMYROAMING, WHICH WILL INHERIT FROM ENEMY.
    THEN, THE CAT AND GRANDMA CAN SHARE THAT SAME BEHAVIOR WITHOUT CLOGGING DOWN BASE ENEMY SCRIPT WITH THINGS THAT WOULD BE
    OVERRIDEN WITH WOLFSPIDER AND HOUSE RAT



Goal: Make Grandmother be able to switch between Scenes in a sequence. Then, the grandmother will go back
to her room for a few in-game hours.
Then, the grandmother will also be able to automatically switch to the GrandmotherWaypoints object in the scene.

Make grandma visit a random number of nodes in the room, and then she will leave
-Make Waypoints have a script that has a simple list of possible waypoints to travel to. 
-Make a Waypoint on the door, which means that the Grandmother will be set at that position.

Grandma requiredWaypoints = Random number between 0 and Number of waypoints in current room
    (Do get that, find the game object with the name of GrandmotherWaypoints, and get the number of children)

If Grandma reaches a waypoint
    requiredWaypoints++;
    if reached enough waypoints
        travel to one of the random available nodes
    else
        then make way to exit (Depending on Room you are in)

-If Grandmother must travel to exit, then they will use the list from their currentWaypoint called pathToExit. 
    This path will give the Grandmother the shortest path to the exit, this will need to be set manually in the inspector.    

Movement Pattern/Pathfinding Update
Update the Grandmothers Movement pattern to make it more randomized
When arriving at node 1, the grandmother would then travel to either 2 or 4 (it would pick randomly)
    _______12______
    |1   2       6|
    | XXX  4   7  |
   13 XXX         11      
    | XXX  9   0  |
    |4   3       5|
    =======10======

Waypoint Script:
GameObject[] possibleTravelPoints;
GameObject[] pathToExit;

*****Node 1 Example*****
       :Node 1:
possibleTravelPoints: Node 2 and Node 4
pathToSouthExit: Node 4, Node 3, Node 10
pathToNorthExit: Node 2, Node 12
pathToEastExit: Node 2, Node 7, Node 11
pathToWestExit: Node 13



Next problem: The grandmother must be able to know which exit to travel to. 

Grandmother sequence. 

 */
#endregion
