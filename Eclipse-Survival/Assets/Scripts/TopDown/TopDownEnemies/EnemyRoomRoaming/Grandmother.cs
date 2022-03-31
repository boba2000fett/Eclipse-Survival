using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grandmother : EnemyRoomRoaming
{
    public enum StrikeState { Strike, Hold, PullBack, NotStriking }


    static public Grandmother grandmaSingleton;

    [Header("Set in Inspector: Grandmother")]
    public float hittingDuration = 0.5f;
    public int fryingPanStrength;
    [Tooltip("This is the time interval it takes the Grandmother to initially strike")]
    public float strikeTimeInterval = .25f;
    [Tooltip("This is the time interval that the Grandmother holds after striking (with Frying Pan in place)")]
    public float holdTimeInterval = .5f;
    [Tooltip("This is the time interval it takes the Grandmother to pull back the Frying Pan after hitting")]
    public float pullBackTimeInterval = .25f;

    [Header("Set Dynamically: Grandmother")]
    public GameObject fryingPan;
    public bool isHitting = false;
    public float hittingTime = 0f;
    public float strikeTime = 0;
    public float holdTime = 0;
    public float pullBackTime = 0;
    public StrikeState strikeState = StrikeState.NotStriking;

    private Vector2 leftFacingPositionFryingPan = new Vector2(-0.26f, -0.171f);
    private Vector2 rightFacingPositionFryingPan = new Vector2(0.26f, -0.171f);
    private Vector2 upFacingPositionFryingPan = new Vector2(0, 0.317f);
    private Vector2 downFacingPositionFryingPan = new Vector2(0, -.25f);
    private Vector2 standardPositionFryingPan = new Vector2(0, 0);



    public override void Awake()
    {
        base.Awake();

        #region Grandma Singleton
        if (grandmaSingleton == null)
        {
            //Set the GPM instance
            grandmaSingleton = this;
        }
        else if (grandmaSingleton != this)
        {
            //If the reference has already been set and
            //is not the right instance reference, Destroy the GameObject
            Destroy(gameObject);
        }

        //Do not Destroy this gameobject when a new scene is loaded
        DontDestroyOnLoad(gameObject);
        #endregion

        #region Grandmother Awake
        fryingPan = this.gameObject.transform.GetChild(0).gameObject;
        fryingPan.transform.localPosition = standardPositionFryingPan;
        fryingPan.SetActive(false);
        Damage fryingPanDamage = fryingPan.GetComponent<Damage>();
        fryingPanDamage.strength = fryingPanStrength;

        isHitting = false;
        hittingTime = 0f;
        #endregion
    }

    public override void Update()
    {
        if (atHome && SceneManager.GetActiveScene().name == "UpstairsTopLeftBedroom")
        {
            //Debug.LogWarning("Grandmother is Sleep");
            GameObject.FindObjectOfType<Bed>().GrandmaInBed();
        }
        else if(!atHome && SceneManager.GetActiveScene().name == "UpstairsTopLeftBedroom")
        {
            GameObject.FindObjectOfType<Bed>().RegularBed();
        }

        if (!isHitting)
        {
            base.Update();
        }
        else
        {
            StrikeFryingPan();
        }

        //if (target == null)
        //{
        //    isHitting = false;
        //    hittingTime = 0f;
        //    fryingPan.SetActive(false);
        //}

        //if (isHitting)
        //{
        //    UpdateFryingPanPosition();
        //    CheckFryingPan();
        //}
    }

    public override void AlertMoveTowards()
    {
        //if (!isHitting)
        //{

            //Possibly add in more conditions to make the object leave alert phase, like checking if the target left the room
            if (target == null || (alertTime > alertTimeDuration))
            {
                TurnOffIsAlerted();
                return;
            }

            Vector2 distanceFromTarget = target.gameObject.transform.position - transform.position;

            Debug.Log(distanceFromTarget.magnitude);

            if (distanceFromTarget.magnitude > attackRange)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.gameObject.transform.position, runSpeed * Time.deltaTime);
            }
            else
            {
                if (!isHitting)
                {
                    AudioManagement.Instance.PlayFryingPanSFX();
                }
                isHitting = true;
                strikeState = StrikeState.Strike;
            }
        //}
        //else
        //{
        //    StrikeFryingPan();

        //}
    }

    #region Frying Pan Methods
    public void StrikeFryingPan()
    {
        //UpdateFryingPanPosition();
        //fryingPan.SetActive(true);
        //isHitting = true;
        var ts = TimeSpan.FromSeconds(strikeTimeInterval);
        Debug.Log($"strikeTimeInterval {string.Format("{0:00}:{1:00}", ts.TotalMinutes, ts.Seconds)}");

        SwitchAttackingAnimation(true);

        if (strikeState == StrikeState.Strike)
        {
            //Switch to Animation where Grandmother Strikes Pan down from over her head
            strikeTime += Time.deltaTime;

            if (strikeTime >= strikeTimeInterval)
            {
                strikeState = StrikeState.Hold;
                strikeTime = 0f;
            }
        }
        else if (strikeState == StrikeState.Hold)
        {
            //Play Animation of Grandmother Holding Frying Pan in Place
            //Activate Frying Pan***
            UpdateFryingPanPosition();
            fryingPan.SetActive(true);

            holdTime += Time.deltaTime;
            if (holdTime >= holdTimeInterval)
            {
                strikeState = StrikeState.PullBack;
                holdTime = 0f;
                fryingPan.SetActive(false);
            }
        }
        else if (strikeState == StrikeState.PullBack)
        {
            //Switch to animation of Grandmother Pulling Back Pan
            pullBackTime += Time.deltaTime;

            if (pullBackTime >= pullBackTimeInterval)
            {
                isHitting = false;
                strikeState = StrikeState.NotStriking;
                pullBackTime = 0f;

                //Possibly Check if Target is Null here, and then if it is, make isAlerted False
                SwitchAttackingAnimation(false);
            }
        }
    }

    public void SwitchAttackingAnimation(bool setBool)
    {
        if (setBool)
        {
            switch (direction)
            {
                case Facing.Down:
                    anim.SetBool("isAttackingDown", true);
                    break;
                case Facing.Right:
                    anim.SetBool("isAttackingRight", true);
                    break;
                case Facing.Left:
                    anim.SetBool("isAttackingLeft", true);
                    break;
                case Facing.Up:
                    anim.SetBool("isAttackingUp", true);
                    break;
            }
        }
        else
        {
            switch (direction)
            {
                case Facing.Down:
                    anim.SetBool("isAttackingDown", false);
                    break;
                case Facing.Right:
                    anim.SetBool("isAttackingRight", false);
                    break;
                case Facing.Left:
                    anim.SetBool("isAttackingLeft", false);
                    break;
                case Facing.Up:
                    anim.SetBool("isAttackingUp", false);
                    break;
            }
        }
    }

    public void UpdateFryingPanPosition()
    {
        if (this.direction == Facing.Down)
        {
            fryingPan.GetComponent<SpriteRenderer>().enabled = true;
            fryingPan.transform.rotation = Quaternion.Euler(0, 0, 180);
            fryingPan.transform.localPosition = downFacingPositionFryingPan;
        }
        else if (this.direction == Facing.Up)
        {
            fryingPan.GetComponent<SpriteRenderer>().enabled = false;
            fryingPan.transform.rotation = Quaternion.Euler(0, 0, 0);
            fryingPan.transform.localPosition = upFacingPositionFryingPan;
        }
        else if (this.direction == Facing.Right)
        {
            fryingPan.GetComponent<SpriteRenderer>().enabled = true;
            fryingPan.transform.rotation = Quaternion.Euler(0, 0, -90);
            fryingPan.transform.localPosition = rightFacingPositionFryingPan;
        }
        else if (this.direction == Facing.Left)
        {
            fryingPan.GetComponent<SpriteRenderer>().enabled = true;
            fryingPan.transform.rotation = Quaternion.Euler(0, 0, 90);
            fryingPan.transform.localPosition = leftFacingPositionFryingPan;
        }
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
    #endregion
}
/*
 Grandmother:
To make her have a STRIKE - HOLD - PULL BACK type of attack, I will implement a simular system as the PounceSystem for the Cat
StrikeState
STRIKE - The Grandmother Stops in Place, and the Animation of her striking with the Frying Pan plays
HOLD - The Grandmother fully strikes the Frying Pan, with her Holding it in Place
PULL BACK - The Grandmother retracts the Frying Pan, pulling it back
NotStriking - Default State, make this when the grandma is not striking anything

bool isHitting;
//Set all 3 of these times to zero
float strikeTime
float holdTime
float pullBackTime

if(State = Strike)
    strikeTime += Time.deltatime
    if(strikeTime >= strikeTimeInterval)
        strikeTime = 0;
        State = State.HOLD

else if(State = HOLD)
    Activate Frying Pan
    holdTime += Time.deltaTime;
    if(holdTime >= holdTimeInterval)
        State = State.PULLBACK
        holdTime = 0;

else if(State = PULL BACK)
    pullBackTime += Time.deltaTime
    if(pullBackTime >= pullBacktimeInterval)
        isHitting = false;
        pullBackTime = 0;
        State = State.Striking

 */
