using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grandmother : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
            /*
            Strike Xander with Frying Pan   
             */
        }
    }


}
