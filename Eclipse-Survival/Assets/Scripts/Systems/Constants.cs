using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    // Xander Constants
    public const float WALK_SPEED = 90f;
    public const float RUN_SPEED = 250f;
    public const int STARTING_HUNGER = 100;
    public const int STARTING_HEALTH = 10;
    public const float HUNGER_DECREMENT_INTERVAL = 5f;

    // Stamina
    public const float STARTING_STAMINA = 100f;
    public const float STAMINA_USE_INCREMENT = 0.5f;
    public const float STAMINA_RECHARGE_INCREMENT = 0.1f;
    public const float STAMINA_COOLDOWN_PERIOD = 3f;

    //Constant Values for the Day/Night light cycle script
    public const float MAX_LIGHT_INTENSITY = 0.9f;
    public const float MIN_LIGHT_INTENSITY = 0.05f;
    public const float LIGHT_DECREASE_DURATION = 45f; // in seconds
    public const float LIGHT_INCREASE_DURATION = 45f; // in seconds
    public const float DAY_NIGHT_CUTOFF = 0.3f; // light intensity at which IsDaytime is toggled

    //Constant Int Value for the Scene Index Value
    public const int SPLASH_SCENE = 0;
    public const int STUDIO_SCENE = 1;
    public const int MAIN_MENU_SCENE = 2;
    public const int OPTION_SCENE = 3;
    public const int CREDIT_SCENE = 4;
    public const int GAME_OVER_SCENE = 5;
    //public const int DownstairsTopLeftKitchen = 5;
    //public const int PLAY_SCENE_WALL_VIEW = 6;
    //public const int PAUSE_SCENE = 7;
    


    //Test Constant Scene for transition between two scenes
    //public const int PLAYABLE_TOP_DOWN_VIEW = 0;
    //public const int PLAYABLE_WALL_VIEW = 1;
    public const int ALPHA_CAMERON_TEST_SCENE = 5;

    //Constant Values for the game object variables
    public const float AI_CONNECTION_DISTANCE = 2.3f;



    //Constants For the max number of items that can spawn in the different rooms


}
