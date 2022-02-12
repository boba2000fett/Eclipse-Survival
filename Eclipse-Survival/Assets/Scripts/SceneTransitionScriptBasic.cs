using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;


public class SceneTransitionScriptBasic : MonoBehaviour
{
    [Header("Set In Inspecotr")]
    public float delay;

    [Header("Set Dynamically")]
    Scene currentScene;
    
    void Start()
    {
        //Indetify the current scene that is being shown
        currentScene = SceneManager.GetActiveScene();
    }

    void Update()
    {     
        
        
        //Check for the input key of X for switching between the two playable scenes
        if (Input.GetKey(KeyCode.X))
        {
            AutoTransitionScene(3);    
        }

        //TODO
        /*
         * Implement other update in methods to help identify the current scene being shown to help segment the different scene transition operations
         * Have the scene transitions function and methods be used and called for in their own methods
         * Add a way for a player to return to a previous scene without values being reset or set incorrectly
         * Implement the scene manager either within an appropiate scene script or add the scene manager to the overall game manager
         */
    }

    public void AutoTransitionScene(int nextScene)
    {
        switch (currentScene.buildIndex)
        {
            case Constants.SPLASH_SCENE:
                SceneManager.LoadScene(Constants.STUDIO_SCENE);
                break;
            case Constants.STUDIO_SCENE:
                SceneManager.LoadScene(Constants.MAIN_MENU_SCENE);
                break;
            case Constants.MAIN_MENU_SCENE:
                if (nextScene == Constants.SPLASH_SCENE)
                {
                    SceneManager.LoadScene(Constants.SPLASH_SCENE);
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
