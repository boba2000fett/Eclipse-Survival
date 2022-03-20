using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Text scoreText;

    void Start()
    {
        AudioManagement.Instance.SwitchBackgroundMusic(BackgroundMusicType.Menu);
        int r = GamePlayManager.GPM.CurrentHoursSurvived;
        scoreText.text = "Score: "+r+" Hours\nHigh Score: "+r+" Hours";
    }

}
