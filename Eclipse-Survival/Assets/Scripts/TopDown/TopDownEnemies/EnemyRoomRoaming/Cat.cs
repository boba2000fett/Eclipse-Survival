using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cat : EnemyRoomRoaming
{
    static public Cat catSingleton;

    public enum PounceState
    {
        Prepare,
        Jumping,
        Landed,
        NotPouncing
    }

    [Header("Set In Inspector: Cat")]
    public float pouncePrepateTimeInverval;
    public float pounceStopTimeInverval;
    public int damage;
    public float pounceSpeed;
    public float pounceCooldownTimeInterval = 1f;

    [Header("Set Dynamically: Cat")]
    public Vector3 hoppingPoint;
    public bool isPouncing = false;
    public PounceState pounceState = PounceState.NotPouncing;
    public float pouncePrepareTime;
    public float pounceStopTime;
    public float pounceCooldownTime = 0;

    

    //public Vector2 previousDistanceToXander;
    //public Vector2 distanceToXander;
    //public Vector2 velocityTowardsXander;

    public override void Awake()
    {
        base.Awake();

        #region Cat Singleton 
        if (catSingleton == null)
        {
            //Set the GPM instance
            catSingleton = this;
        }
        else if (catSingleton != this)
        {
            //If the reference has already been set and
            //is not the right instance reference, Destroy the GameObject
            Destroy(gameObject);
        }

        //Do not Destroy this gameobject when a new scene is loaded
        DontDestroyOnLoad(gameObject);
        #endregion
    }

    public override void Update()
    {
        base.Update();
        if (atHome && SceneManager.GetActiveScene().name == "DownstairsBottomRightLivingRoom")
        {
            //Debug.LogWarning("Grandmother is Sleep");
            GameObject.FindObjectOfType<Bed>().GrandmaInBed();
        }
        else if (!atHome && SceneManager.GetActiveScene().name == "DownstairsBottomRightLivingRoom")
        {
            GameObject.FindObjectOfType<Bed>().RegularBed();
        }
    }

    public override void AlertMoveTowards()
    {       
        if (!isPouncing)
        {

            if (stuck)
            {
                TravelInDirection();
                return;
            }
            else
            {
                VelocityChecker();
                if (stuck)
                    return;
            }

            if (target == null || (alertTime > alertTimeDuration))
            {
                isAlerted = false;
                return;
            }

            Vector2 distanceFromTarget = target.transform.position - transform.position;
            Debug.Log(distanceFromTarget.magnitude);

            if (distanceFromTarget.magnitude > attackRange)
            {
                previousPosition = transform.position;
                
                previousDistanceToXander = distanceToXander;
                
                transform.position = Vector2.MoveTowards(transform.position, target.gameObject.transform.position, runSpeed * Time.deltaTime);

                distanceToXander = Vector2.Distance(target.transform.position, transform.position);
            }
            else
            {
                hoppingPoint = target.transform.position;
                isPouncing = true;
                pouncePrepareTime = 0f;
                pounceStopTime = 0f;
                pounceState = PounceState.Prepare;
            }
        }
        else
        {
            Pounce();
        }
    }

   

    private void Pounce()
    {
        SwitchAttackingAnimation(true);

        if (pounceState == PounceState.Prepare)
        {
            //Switch to stance position animation
            pouncePrepareTime += Time.deltaTime;

            if (pouncePrepareTime >= pouncePrepateTimeInverval)
            {
                pounceState = PounceState.Jumping;
                pouncePrepareTime = 0f;
            }               
        }
        else if (pounceState == PounceState.Jumping)
        {
            transform.position = Vector2.MoveTowards(transform.position, hoppingPoint, pounceSpeed * Time.deltaTime);
            //Switch to jumping position animation

            Debug.LogWarning($"Distance to Hopping Point" +
                $"{Mathf.Abs(this.transform.position.magnitude - hoppingPoint.magnitude)}");

            if (Mathf.Abs(this.transform.position.magnitude - hoppingPoint.magnitude) <= .1)
            {                
                pounceState = PounceState.Landed;                
            }
        }
        else if (pounceState == PounceState.Landed)
        {
            //Switch to stance position animation
            pounceStopTime += Time.deltaTime;

            if (pounceStopTime >= pounceStopTimeInverval)
            {
                isPouncing = false;
                pounceState = PounceState.NotPouncing;
                pounceStopTime = 0f;
                pouncePrepareTime = 0f;

                SwitchAttackingAnimation(false);

            }
        }           
    }


    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        switch (collision.gameObject.tag)
        {
            case "Xander":
                //Reset Time float variables, and bool variables (isPouncing, stuck, etc.)
                return;
            case "Grandmother":
                //If Cat Collides with Grandmother, it will chose a different path.
                if (!currentWaypointDestination.isExitNode && !isAlerted)
                {
                    DetermineDestination();
                }
                return;
        }
    }
   
}

#region Planning
/*
 Planning: Implementing Cat Improved Movement System 
(Will need to implement this into Cat, Grandmother, and WolfSpiderTopDown scripts)


Goals:
-Record Velocity that the Cat is moving when Alerted
-If cat is not changing verticle or horizontal velocity enough over a period of time, then switch off isAlerted,
    travel along the axis that was not stuck, and travel in that direction for a few seconds (random whether right or 
    left, or up or down), and then begin targeting Xander again

Variables I will need
-Vector2 velocity DYNAMIC
-Vector2 previousPosition DYNAMIC
-float stuckTimeCheck DYNAMIC
-float stuckTimeCheckInterval STATIC
-float travelTime DYNAMIC
-float travelTimeInterval STATIC
-int unstuckDirection DYNAMIC



Current AlertMoveTowards() Method in script
{
if(stuck)
{
TravelInDirection();
}
else
{
VelocityChecker();
}


If Not Pouncing
    Check if you need to set isAlerted = false;

    Get Distance from Target

    if(Not within attacking range)
        previousPosition = transform.position
previousDistanceToXander = transform.position - target.transform.position;
        Move towards Target	
distanceToXander = transform.position - target.transform.position;
    else
	    Begin hopping towards enemy
else
	Pounce()
}


public bool VelocityChecker()
{
    velocity = transform.position - previous.position

    velocityTowardsXander = distanceToXander - previousDistanceToXander

    stuckTimeCheck += Time.deltaTime;

    if(velocity.magnitude > .5f)
    {
        stuckTimeCheck = 0;
        return;
    }
    else if(stuckTimeCheck > stuckTimeInterval)
    {
        stuck = true;
        DecideUnstuckDirection();
    }
}

public void DecideUnstuckDirection()
{
    unstuckDirection = Random.Range(1,4);
}

public void TravelinDirection()
{
travelTime += Time.deltaTime;

if(traveltime > travelTimeInterval)
{
    stuck = false;
    travelTime = 0;
    isAlerted = true; //Maybe put this here?
    return;
}

case(unstuckDirection)
{
    case 1: (Go Left)
        Vector2.TravelTowards(transform.position, new Vector2(transform.position.x - 5, transform.position.y), runSpeed * Time.deltaTime)
        break;
    case 2: (Go Right)
        Vector2.TravelTowards(transform.position, new Vector2(transform.position.x + 5, transform.position.y), runSpeed * Time.deltaTime)
        break;
    case 3: (Go Up)
        Vector2.TravelTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 5), runSpeed * Time.deltaTime)
        break;
    case 4: (Go Down)
        Vector2.TravelTowards(transform.position, new Vector2(transform.position.x, transform.position.y - 5), runSpeed * Time.deltaTime)
        break;
}



}

 
 */
#endregion
