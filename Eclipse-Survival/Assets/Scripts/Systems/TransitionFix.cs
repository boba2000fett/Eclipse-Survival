using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TransitionFix : MonoBehaviour
{
    static public void PauseMenuTransition(int nextScene)
    {
        if (nextScene == Constants.GAME_OVER_SCENE)
        {
            SceneManager.LoadScene(Constants.GAME_OVER_SCENE);
        }
        else if (nextScene == Constants.OPTION_SCENE)
        {
            SceneManager.LoadScene(Constants.OPTION_SCENE);
        }
    }

    static public void ExitGame()
    {
        Application.Quit();
    }
}
