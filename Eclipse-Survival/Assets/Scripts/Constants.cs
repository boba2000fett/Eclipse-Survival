using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{

    //Constant Values for the Day/Night light cycle script
    public const float MAX_LIGHT_INTENSITY = 0.6f;
    public const float MIN_LIGHT_INTENSITY = 0.05f;
    public const float LIGHT_DECREASE_DURATION = 450f; // in seconds
    public const float LIGHT_INCREASE_DURATION = 450f; // in seconds
    public const float DAY_NIGHT_CUTOFF = 0.3f; // light intensity at which IsDaytime is toggled

    //Constant Int Value for the Scene Index Value
    public const int SPLASH_SCENE = 0;
    public const int STUDIO_SCENE = 1;
    public const int MAIN_MENU_SCENE = 2;
    public const int OPTION_SCENE = 3;
    public const int CREDIT_SCENE = 4;
    //public const int PLAY_SCENE_TOP_DOWN_VIEW = 5;
    //public const int PLAY_SCENE_WALL_VIEW = 6;
    public const int PAUSE_SCENE = 7;
    public const int GAME_OVER_SCENE = 8;
    

    //Test Constant Scene for transition between two scenes
    //public const int PLAYABLE_TOP_DOWN_VIEW = 0;
    //public const int PLAYABLE_WALL_VIEW = 1;

    //Constant Values for the game object variables

}
