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
        int high = PlayerPrefs.GetInt("HighScore");
        if (r > high)
        {
            high = r;
            PlayerPrefs.SetInt("HighScore", r);
        }
        scoreText.text = "Score: "+r+" Hours\nHigh Score: "+high+" Hours";
        Time.timeScale = 0f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
