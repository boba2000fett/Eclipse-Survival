using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    [Header("Set Dynamically: Cat")]
    public Vector3 hoppingPoint;
    public bool isPouncing = false;
    public PounceState pounceState = PounceState.NotPouncing;
    public float pouncePrepareTime;
    public float pounceStopTime;

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

    public override void AlertMoveTowards()
    {
        if (!isPouncing)
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
                //Attack Xander when you are within range
                /* Lunge at Xander (Cat, Rat) */
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
            transform.position = Vector2.MoveTowards(transform.position, hoppingPoint, runSpeed * Time.deltaTime);
            //Switch to jumping position animation

            if ((this.transform.position.magnitude - hoppingPoint.magnitude) <= .2)
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
            }
        }           
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Xander":
                //Here we will want to damage Xander
                return;
            case "Grandmother":
                //If Cat Collides with Grandmother, it will chose a different path.
                if (!currentWaypointDestination.isExitNode)
                {
                    DetermineDestination();
                }
                return;
        }
    }
   
}

#region Planning
/*
 * Planning: 
 * What I am thinking for the AI
 * The cat will run at Xander. If within range, the Cat will lunge. The cat will use a 
 * simular child Game Object like the Grandmother does for the frying pan, except the cat will lunge at that position. 
 * So, the Cat will change to a jump stance, and then change to a jumping stance towards that position. Then, it will land (same animation
 * as the jump stance), and wait for a few seconds. The ordeal should take in total around 2 seconds. 
 * 
 * Variables
 * Vector2 - Xander's Last Position - Hopping Point (This will be where the Cat hops too (Don't really need a child game object))
 * enum Pounce State {Prepare, Jumping, Landed, NotPouncing}
 * 
 * If (within Range)
 *  state = Pounce: (or just have isPouncing)
 * 
 * Update Method
 * Pounce();
 * 
 * 
 * void Pounce()
 *  if (isPouncing)
 *      if(PounceState.Prepare)
 *          Switch to stance position animation
 *          pouncePrepareTime += Time.deltaTime;
 *      else if(PounceState.Jumping)
 *          Move towards pouncePosition
 *          Switch to jumping position animation    
 *      else if (Reached pouncePosition)
 *          Switch to stance position animation
 *          pounceStopTime += Time.deltaTime;    
 *
 *      if(pouncePrepareTime >= pouncePrepateTimeInverval)
 *          pounceState = PounceState.Jump
 *      if(pounceStopTime  >= pounceStopTimeInverval)
 *          isPouncing = false;
 *  
 * 
 */
#endregion
