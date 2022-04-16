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

    [Header("Set Dynamically: Cat")]
    public Facing previousFacing;
    public Vector2 avoidPosition;
    public Vector3 hoppingPoint;
    public bool isPouncing = false;
    public PounceState pounceState = PounceState.NotPouncing;
    public float pouncePrepareTime;
    public float pounceStopTime;
    
    

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
        AudioManagement.Instance.PlayCatSFX(isAlerted, isInRoom);
                       
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
                TurnOffIsAlerted();
                return;
            }

            if (attackCooldown)
            {
                attackCooldownTime += Time.deltaTime;   
                if (attackCooldownTime > attackCooldownTimeInterval)
                {
                    attackCooldown = false;
                    attackCooldownTime = 0;
                }
            }

            Vector2 distanceFromTarget = target.transform.position - transform.position;
            //Debug.Log(distanceFromTarget.magnitude);


            if (distanceFromTarget.magnitude <= attackRange && !attackCooldown)
            {
                hoppingPoint = target.transform.position;
                isPouncing = true;
                pouncePrepareTime = 0f;
                pounceStopTime = 0f;
                pounceState = PounceState.Prepare;
            }
            else
            {
                //previousPosition = transform.position;

                previousDistanceToXander = distanceToXander;

                //Tracking the previousFacing direction every .4 seconds-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
                stuckTimeCheck += Time.deltaTime;

                if (stuckTimeCheck >= stuckTimeCheckInterval)
                {
                    previousFacing = direction;
                    previousPosition = transform.position;
                    stuckTimeCheck = 0;
                    speed = (((Vector2)transform.position - previousPosition).magnitude / Time.deltaTime);
                }


                transform.position = Vector2.MoveTowards(transform.position, target.gameObject.transform.position, runSpeed * Time.deltaTime);

                distanceToXander = Vector2.Distance(target.transform.position, transform.position);
            }


            //if (distanceFromTarget.magnitude > attackRange)
            //{
            //    previousPosition = transform.position;

            //    previousDistanceToXander = distanceToXander;

            //    transform.position = Vector2.MoveTowards(transform.position, target.gameObject.transform.position, runSpeed * Time.deltaTime);

            //    distanceToXander = Vector2.Distance(target.transform.position, transform.position);
            //}
            //else
            //{
            //    hoppingPoint = target.transform.position;
            //    isPouncing = true;
            //    pouncePrepareTime = 0f;
            //    pounceStopTime = 0f;
            //    pounceState = PounceState.Prepare;
            //}
        }
        else
        {
            Pounce();
        }
    }

   
    public void SwitchOffIsAlerted()
    {

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

            //Debug.LogWarning($"Distance to Hopping Point" + $"{Mathf.Abs(this.transform.position.magnitude - hoppingPoint.magnitude)}");

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

                attackCooldown = true;
                attackCooldownTime = 0;
            }
        }           
    }

    public void VelocityChecker()
    {
        stuckTimePauseTimer += Time.deltaTime;

        if (stuckTimePauseTimer >= stuckTimePauseTimerInterval)
        {
            if (previousFacing == direction &&
                (((Vector2)transform.position - previousPosition).magnitude / Time.deltaTime) < .5f) 
            {
                stuck = true;
                DecideUnstuckDirection();
                stuckTimePauseTimer = 0;
            }
        }

        #region Old Things

        //previousVelocity = velocity;

        //velocity = ((Vector2)transform.position - previousPosition) / Time.deltaTime;

        ////velocityTowardsXander = distanceToXander - previousDistanceToXander;
        //acceleration = (velocity - previousVelocity);

        ////stuckTimeCheck += Time.deltaTime;
        //stuckTimePauseTimer += Time.deltaTime;
        //stuckTimeCheck += Time.deltaTime;

        ////if ((Mathf.Abs(previousVelocity.x) - Mathf.Abs(velocity.x) > 1f ||
        ////    Mathf.Abs(previousVelocity.y) - Mathf.Abs(velocity.y) > 1f) &&
        ////    Mathf.Abs(previousDistanceToXander - distanceToXander) > .01)


        ////Debug.LogError($"distanceToXander - previousDistanceToXander / Time.deltaTime = " + 
        ////    $"{Mathf.Abs((distanceToXander) - (previousDistanceToXander) ) / Time.deltaTime}");
        ////Debug.LogWarning($"Mathf.Abs(distanceToXander - previousDistanceToXander)" +
        ////    $"{Mathf.Abs(distanceToXander - previousDistanceToXander)}");

        //if (velocity.magnitude > .5f)
        //{
        //    stuckTimeCheck = 0;
        //    return;
        //}
        ////else if (stuckTimeCheck > stuckTimeCheckInterval)
        ////{
        ////    stuck = true;
        ////    stuckTimeCheck = 0;
        ////    DecideUnstuckDirection();
        ////}
        ////.05

        //if (stuckTimePauseTimer >= stuckTimePauseTimerInterval)
        //{
        //    stuckTimePauseTimer = 0;

        //    //this.GetComponent<BoxCollider2D>().enabled = false;
        //    //this.GetComponent<CircleCollider2D>().enabled = false;
        //    //GameObject.Find("MapBounds").gameObject.GetComponent<BoxCollider2D>().enabled = false;

        //    Ray ray = new Ray(transform.position, target.transform.position);

        //    RaycastHit2D hit = Physics2D.Raycast
        //        (transform.position,
        //        target.transform.position - transform.position,
        //        LayerMask.GetMask("Default"));

        //    if (hit.collider.gameObject.tag == "Xander")
        //    {
        //        stuckTimeCheck = 0;
        //    }
        //    //this.GetComponent<BoxCollider2D>().enabled = true;
        //    //this.GetComponent<CircleCollider2D>().enabled = true;
        //    //GameObject.Find("MapBounds").gameObject.GetComponent<BoxCollider2D>().enabled = true;
        //}

        //if (stuckTimeCheck > stuckTimeCheckInterval)
        //{
        //    stuck = true;
        //    stuckTimeCheck = 0;

        //    stuckTimePauseTimer = 0;

        //    DecideUnstuckDirection();
        //}
        #endregion
    }

    /// <summary>
    /// This method uses a more intelligent method for the enemy to get unstuck from a projectile.
    /// </summary>
    public void DecideUnstuckDirection()
    {
        Vector2 distanceToXander = target.transform.position - transform.position;
        RaycastHit2D hit;
        Vector2 travel;
        if (Mathf.Abs(distanceToXander.x) > Mathf.Abs(distanceToXander.y))//We have to go Left or Right to go to Xander
        {
            if (Mathf.Abs(distanceToXander.x) == distanceToXander.x)//Xander is to the Right
            {                
                Debug.Log("Xander is to the Right");
               //If Xander is above
                if (Mathf.Abs(distanceToXander.y) == distanceToXander.y)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        travel = new Vector2(transform.position.x, transform.position.y + i);
                        hit = Physics2D.Raycast(travel, Vector2.right, 10f, LayerMask.GetMask("Default"));

                        if (hit.distance > 1f)
                        {
                            avoidPosition = travel;
                            return;
                        }
                    }

                    stuck = false; //This is if nothing is found. 
                }
                else 
                {
                    for (int i = 0; i < 7; i++)
                    {
                        travel = new Vector2(transform.position.x, transform.position.y - i);
                        hit = Physics2D.Raycast(travel, Vector2.right, 10f, LayerMask.GetMask("Default"));

                        if (hit.distance > 1f)
                        {
                            avoidPosition = travel;
                            return;
                        }
                    }

                    stuck = false; //This is if nothing is found.
                }
                
            }
            else
            {
                Debug.Log("Xander is to the Left");
                //If Xander is above
                if (Mathf.Abs(distanceToXander.y) == distanceToXander.y)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        travel = new Vector2(transform.position.x, transform.position.y + i);
                        hit = Physics2D.Raycast(travel, Vector2.left, 10f, LayerMask.GetMask("Default"));

                        if (hit.distance > 1f)
                        {
                            avoidPosition = travel;
                            return;
                        }
                    }

                    stuck = false; //This is if nothing is found. 
                }
                else
                {
                    for (int i = 0; i < 7; i++)
                    {
                        travel = new Vector2(transform.position.x, transform.position.y - i);
                        hit = Physics2D.Raycast(travel, Vector2.left, 10f, LayerMask.GetMask("Default"));

                        if (hit.distance > 1f)
                        {
                            avoidPosition = travel;
                            return;
                        }
                    }

                    stuck = false; //This is if nothing is found.
                }
            }

        }
        else
        {
            if (Mathf.Abs(distanceToXander.y) == distanceToXander.y)//Xander is above Cat
            {
                Debug.Log("Xander is Above");
                //If Xander is above
                if (Mathf.Abs(distanceToXander.x) == distanceToXander.x)//If Xander is above and to the right
                {
                    for (int i = 0; i < 7; i++)
                    {
                        travel = new Vector2(transform.position.x + i, transform.position.y);
                        hit = Physics2D.Raycast(travel, Vector2.up, 10f, LayerMask.GetMask("Default"));

                        if (hit.distance > 1f)
                        {
                            avoidPosition = travel;
                            return;
                        }
                    }

                    stuck = false; //This is if nothing is found. 
                }
                else //If Xander is above and to the left
                {
                    for (int i = 0; i < 7; i++)
                    {
                        travel = new Vector2(transform.position.x - i, transform.position.y);
                        hit = Physics2D.Raycast(travel, Vector2.up, 10f, LayerMask.GetMask("Default"));

                        if (hit.distance > 1f)
                        {
                            avoidPosition = travel;
                            return;
                        }
                    }

                    stuck = false; //This is if nothing is found.
                }

            }
            else//Xander is below cat
            {
                Debug.Log("Xander is Below");
                //If Xander is below
                if (Mathf.Abs(distanceToXander.x) == distanceToXander.x)//If Xander is below and to the right
                {
                    for (int i = 0; i < 7; i++)
                    {
                        travel = new Vector2(transform.position.x + i, transform.position.y);
                        hit = Physics2D.Raycast(travel, Vector2.down, 10f, LayerMask.GetMask("Default"));

                        if (hit.distance > 1f)
                        {
                            avoidPosition = travel;
                            return;
                        }
                    }

                    stuck = false; //This is if nothing is found. 
                }
                else //If Xander is below and to the left
                {
                    for (int i = 0; i < 7; i++)
                    {
                        travel = new Vector2(transform.position.x - i, transform.position.y);
                        hit = Physics2D.Raycast(travel, Vector2.down, 10f, LayerMask.GetMask("Default"));

                        if (hit.distance > 1f)
                        {
                            avoidPosition = travel;
                            return;
                        }
                    }

                    stuck = false; //This is if nothing is found.
                }
            }

        }
        #region Old Code
        ////unstuckDirection = Random.Range(1, 4);
        ////if (Mathf.Abs(acceleration.x) > Mathf.Abs(acceleration.y))//If stuck against surface either above or below enemy
        //if (direction == Facing.Right || direction == Facing.Left)
        //{
        //    //unstuckDirection = Random.Range(3, 4);

        //    Vector2 travelUp = new Vector2(transform.position.x, transform.position.y + 3);
        //    Vector2 travelDown = new Vector2(transform.position.x, transform.position.y - 3);

        //    Vector2 upDistance = travelUp - (Vector2)target.transform.position;
        //    Vector2 downDistance = travelDown - (Vector2)target.transform.position;

        //    RaycastHit2D upHit;
        //    RaycastHit2D downHit;
        //    if (direction == Facing.Right)
        //    {
        //        upHit = Physics2D.Raycast(travelUp, Vector2.right);
        //        downHit = Physics2D.Raycast(travelDown, Vector2.right);
        //    }
        //    else
        //    {
        //        upHit = Physics2D.Raycast(travelUp, Vector2.left);
        //        downHit = Physics2D.Raycast(travelDown, Vector2.left);
        //    }

        //    if (upHit.collider.gameObject.tag == "Xander")
        //    {
        //        unstuckDirection = 3;
        //    }
        //    else if (downHit.collider.gameObject.tag == "Xander")
        //    {
        //        unstuckDirection = 4;
        //    }
        //    else if (upHit.distance > downHit.distance)
        //    {
        //        unstuckDirection = 3;
        //    }
        //    else if (upHit.distance < downHit.distance)
        //    {
        //        unstuckDirection = 4;
        //    }
        //    else if (upDistance.magnitude <= downDistance.magnitude)
        //    {
        //        unstuckDirection = 3;
        //    }
        //    else
        //    {
        //        unstuckDirection = 4;
        //    }
        //}
        //else
        //{
        //    //unstuckDirection = Random.Range(1, 2);


        //    Vector2 travelRight = new Vector2(transform.position.x + 3, transform.position.y);
        //    Vector2 travelLeft = new Vector2(transform.position.x - 3, transform.position.y);

        //    Vector2 rightDistance = travelRight - (Vector2)target.transform.position;
        //    Vector2 leftDistance = travelLeft - (Vector2)target.transform.position;

        //    //1 left 2 right 3 up 4 down
        //    RaycastHit2D rightHit;
        //    RaycastHit2D leftHit;
        //    if (direction == Facing.Up)
        //    {
        //        rightHit = Physics2D.Raycast(travelRight, Vector2.up);
        //        leftHit = Physics2D.Raycast(travelLeft, Vector2.up);
        //    }
        //    else
        //    {
        //        rightHit = Physics2D.Raycast(travelRight, Vector2.down);
        //        leftHit = Physics2D.Raycast(travelLeft, Vector2.down);
        //    }

        //    if (rightHit.collider.gameObject.tag == "Xander")
        //    {
        //        unstuckDirection = 1;
        //    }
        //    else if (leftHit.collider.gameObject.tag == "Xander")
        //    {
        //        unstuckDirection = 2;
        //    }
        //    else if (rightHit.distance > leftHit.distance)
        //    {
        //        unstuckDirection = 1;
        //    }
        //    else if (rightHit.distance < leftHit.distance)
        //    {
        //        unstuckDirection = 2;
        //    }
        //    else if (rightDistance.magnitude <= leftDistance.magnitude)
        //    {
        //        unstuckDirection = 2;
        //    }
        //    else
        //    {
        //        unstuckDirection = 1;
        //    }
        //}
        #endregion
    }

    public void TravelInDirection()
    {
        //travelTime += Time.deltaTime;

        //if (travelTime > travelTimeInterval)
        //{
        //    stuck = false;
        //    travelTime = 0;
        //    isAlerted = true; //Maybe put this here?
        //    return;
        //}
        if ((Vector2)transform.position == avoidPosition)
        {
            stuck = false;
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, avoidPosition, runSpeed * Time.deltaTime);

        #region Old Code
        //switch (unstuckDirection)
        //{
        //    case 1: //(Go Left)
        //        transform.position = Vector2.MoveTowards(transform.position,
        //            new Vector2(transform.position.x - 5, transform.position.y), runSpeed * Time.deltaTime);
        //        break;
        //    case 2: //(Go Right)
        //        transform.position = Vector2.MoveTowards(transform.position,
        //            new Vector2(transform.position.x + 5, transform.position.y), runSpeed * Time.deltaTime);
        //        break;
        //    case 3: //(Go Up)
        //        transform.position = Vector2.MoveTowards(transform.position,
        //            new Vector2(transform.position.x, transform.position.y + 5), runSpeed * Time.deltaTime);
        //        break;
        //    case 4: //(Go Down)
        //        transform.position = Vector2.MoveTowards(transform.position, new
        //            Vector2(transform.position.x, transform.position.y - 5), runSpeed * Time.deltaTime);
        //        break;
        //}
        #endregion Old Code
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
#region New Planning
/*
Goal:
Update Method
{
if(stuck)
    TravelInDirection();

if(!isInRoom)
    stuck = false;
    isAlerted = false;

VelocityChecker();



}

VelocityChecker()
{
Every .4 seconds [isInRoom = true (Enemy is in the current room)],
-If previous direction = current direction (Facing.Down, etc.), and Mathf.abs(position - previousPosition) < 0
    stuck = true;
    DecideUnstuckDirection();
}

DecideUnstuckDirection()
{
-If Xander is Up
    Shoot Raycasts to the Right or Left, Pointing them up, and keep going until the collider does not hit the same object
    Set new target as position of Raycast that worked
-If Xander is Down
    Shoot Raycasts to the Right or Left, Pointing them Down, and keep going until the collider does not hit the same object
-If Xander is Left
    Shoot Raycasts to the Up or Down, Pointing them Left, and keep going until the collider does not hit the same object
-If Xander is Right
    Shoot Raycasts to the Up or Down, Pointing them Right, and keep going until the collider does not hit the same object
}

distanceToXander = target.position - transform.position;
if(Mathf.abs(distanceToXander.x) > Mathf.abs(distanceToXander.y))//We have to go Left or Right to go to Xander
{
    if(Mathf.abs(distanceToXander.x) == distanceToXander.x)
{
}

}
else
{

}


TravelInDirection()
{
if(at new position)
    stuck = false;
    return;

TravelTowards New Position

}

 */
#endregion
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
