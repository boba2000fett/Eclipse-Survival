using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;


public class SceneTransitionScriptBasic : MonoBehaviour
{
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
            //Determine the current active scene index value in the build is equal to defined constant value and
            //then switch the scene to the appropiate next scene
            if (currentScene.buildIndex == Constants.PLAYABLE_TOP_DOWN_VIEW)
            {
                SceneManager.LoadScene(Constants.PLAYABLE_WALL_VIEW);
            }
            else
            {
                SceneManager.LoadScene(Constants.PLAYABLE_TOP_DOWN_VIEW);
            }     
        }

        //TODO
        /*
         * Implement other update in methods to help identify the current scene being shown to help segment the different scene transition operations
         * Have the scene transitions function and methods be used and called for in their own methods
         * Add a way for a player to return to a previous scene without values being reset or set incorrectly
         * Implement the scene manager either within an appropiate scene script or add the scene manager to the overall game manager
         */
    }
}
