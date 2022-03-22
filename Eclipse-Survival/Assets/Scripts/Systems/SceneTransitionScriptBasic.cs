using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;


public class SceneTransitionScriptBasic : MonoBehaviour
{
    [Header("Set In Inspector")]
    public float delayBetweenAutoTransition;

    [Header("Set Dynamically")]
    Scene currentScene;
    
    void Start()
    {
        //Indetify the current scene that is being shown
        currentScene = SceneManager.GetActiveScene();

        //Load in the audio manager

        //If the current scene is the splash or studio sceen activate a
        //delay time for progressing to the next scene
        if(currentScene.buildIndex == Constants.SPLASH_SCENE || 
            currentScene.buildIndex == Constants.STUDIO_SCENE)
        {
            
        }
    }

    void Update()
    {     
        //If the current scene is the splash or studio sceen activate 
        //The space button can be pressed to progress the transition faster
        if (currentScene.buildIndex == Constants.SPLASH_SCENE ||
            currentScene.buildIndex == Constants.STUDIO_SCENE)
        {
            //Check for the input key of Space for transitioning to the next scene
            if (Input.GetKey(KeyCode.Space))
            {
                    TransitionScene(0);
            }
        }
        
        //Check for the input key of X for switching between the two playable scenes
        if (Input.GetKey(KeyCode.Space))
        {
            TransitionScene(3);    
        }

        //TODO
        /*
         * Add a way for a player to return to a previous scene without values being reset or set incorrectly
         * Implement the scene manager either within an appropiate scene script or add the scene manager to the overall game manager
         */
    }

    //public void StartTransition()
    //{
    //    TransitionScene(0);
    //}

    public void TransitionScene(int nextScene)
    {
        //Switch Structure for determine the main menu scene transitions
        //The in Play scene transitions for the game will be implemented through triggers and scene names
        switch (currentScene.buildIndex)
        {
            case Constants.SPLASH_SCENE:
                SceneManager.LoadScene(Constants.STUDIO_SCENE);
                break;
            case Constants.STUDIO_SCENE:
                SceneManager.LoadScene(Constants.MAIN_MENU_SCENE);
                break;
            case Constants.MAIN_MENU_SCENE:
                if (nextScene == Constants.ALPHA_CAMERON_TEST_SCENE)
                {
                    GamePlayManager.GPM.NewGame();
                    SceneManager.LoadScene("DownstairsTopLeftKitchen");
                    AudioManagement.Instance.SwitchBackgroundMusic(BackgroundMusicType.Normal);
                }
                else if(nextScene == Constants.OPTION_SCENE)
                {
                    SceneManager.LoadScene(Constants.OPTION_SCENE);
                }
                else if (nextScene == Constants.CREDIT_SCENE)
                {
                    SceneManager.LoadScene(Constants.CREDIT_SCENE);
                }
                break;
            case Constants.OPTION_SCENE:
                SceneManager.LoadScene(Constants.MAIN_MENU_SCENE);
                break;
            case Constants.CREDIT_SCENE:
                SceneManager.LoadScene(Constants.MAIN_MENU_SCENE);
                break;
            case Constants.GAME_OVER_SCENE:
                SceneManager.LoadScene(Constants.MAIN_MENU_SCENE);
                break;
        }

        
        //else if (currentScene.buildIndex == Constants.PLAYABLE_TOP_DOWN_VIEW)
        //{
        //    SceneManager.LoadScene(Constants.PLAYABLE_WALL_VIEW);
        //}
        //else
        //{
        //    SceneManager.LoadScene(Constants.PLAYABLE_TOP_DOWN_VIEW);
        //    Physics2D.gravity = new Vector2(0, 0);
        //}
    }
}
