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
