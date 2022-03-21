using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDClockUpdate : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Image HUDClock;
    public Sprite[] clockFaces;
    public Text hoursSurvivedText;

    [Header("Set Dynamically")]
    private int hour;

    // Start is called before the first frame update
    void Start()
    {
        hour = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hour != DayNightCycle.DNC.Hour) // if display is out of sync with the Day/Night Cycle
        {
            hour = DayNightCycle.DNC.Hour;

            if (DayNightCycle.DNC.NeedToUpdateClockUI)
            {
                GamePlayManager.GPM.CurrentHoursSurvived++;
                DayNightCycle.DNC.NeedToUpdateClockUI = false;
            }

            hoursSurvivedText.text = $"Hours Survived: {GamePlayManager.GPM.CurrentHoursSurvived}";
            UpdateHUDClock();
        }
    }

    private void UpdateHUDClock()
    {
        switch (hour)
        {
            case 0:
                HUDClock.sprite = clockFaces[0];
                break;
            case 1:
                HUDClock.sprite = clockFaces[1];
                break;
            case 2:
                HUDClock.sprite = clockFaces[2];
                break;
            case 3:
                HUDClock.sprite = clockFaces[3];
                break;
            case 4:
                HUDClock.sprite = clockFaces[4];
                break;
            case 5:
                HUDClock.sprite = clockFaces[5];
                break;
            case 6:
                HUDClock.sprite = clockFaces[6];
                break;
            case 7:
                HUDClock.sprite = clockFaces[7];
                break;
            case 8:
                HUDClock.sprite = clockFaces[8];
                break;
            case 9:
                HUDClock.sprite = clockFaces[9];
                break;
            case 10:
                HUDClock.sprite = clockFaces[10];
                break;
            case 11:
                HUDClock.sprite = clockFaces[11];
                break;
            case 12:
                HUDClock.sprite = clockFaces[0];
                break;
            case 13:
                HUDClock.sprite = clockFaces[1];
                break;
            case 14:
                HUDClock.sprite = clockFaces[2];
                break;
            case 15:
                HUDClock.sprite = clockFaces[3];
                break;
            case 16:
                HUDClock.sprite = clockFaces[4];
                break;
            case 17:
                HUDClock.sprite = clockFaces[5];
                break;
            case 18:
                HUDClock.sprite = clockFaces[6];
                break;
            case 19:
                HUDClock.sprite = clockFaces[7];
                break;
            case 20:
                HUDClock.sprite = clockFaces[8];
                break;
            case 21:
                HUDClock.sprite = clockFaces[9];
                break;
            case 22:
                HUDClock.sprite = clockFaces[10];
                break;
            case 23:
                HUDClock.sprite = clockFaces[11];
                break;
        }
    }
}
