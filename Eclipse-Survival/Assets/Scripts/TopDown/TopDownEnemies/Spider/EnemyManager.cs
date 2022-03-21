using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    static public EnemyManager enemyManager;

    [Header("Set in Inspector: WolfSpiderManager")]
    public WolfSpiderRoaming wolfSpider1Prefab;
    public WolfSpiderRoaming wolfSpider2Prefab;

    [Header("Set Dynamically: WolfSpiderManager")]
    public bool exitedMenuScene = false;
    public bool awakeFunctionalityCompleted = false;
    public WolfSpiderRoaming wolfSpider1Instance;
    public WolfSpiderRoaming wolfSpider2Instance;
    public Grandmother grandmotherInstance;
    public Cat catInstance;



    public void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (enemyManager == null)
        {
            //Set the GPM instance
            enemyManager = this;
        }
        else if (enemyManager != this)
        {
            //If the reference has already been set and
            //is not the right instance reference, Destroy the GameObject
            Destroy(gameObject);
        }

        //Do not Destroy this gameobject when a new scene is loaded
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!exitedMenuScene)
        {
            return;
        }
        if (exitedMenuScene)
        {
            if (wolfSpider1Instance == null)
            {
                GameObject go = GameObject.Instantiate(wolfSpider1Prefab.gameObject);
                wolfSpider1Instance = go.GetComponent<WolfSpiderRoaming>();                
            }
            else if (wolfSpider2Instance == null)
            {
                GameObject go = GameObject.Instantiate(wolfSpider2Prefab.gameObject);
                wolfSpider2Instance = go.GetComponent<WolfSpiderRoaming>();
            }
        }
    }


    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {

        if (SceneManager.GetActiveScene().name == Constants.STARTING_PLAY_SCENE_NAME && !exitedMenuScene)
        {
            exitedMenuScene = true;

            catInstance = GameObject.Find("Cat").GetComponent<Cat>();
            grandmotherInstance = GameObject.Find("Grandmother").GetComponent<Grandmother>();

            return;
        }
        else if (SceneManager.GetActiveScene().name != Constants.GAME_OVER_SCENE_NAME)
        {
            exitedMenuScene = false;
            Destroy(wolfSpider1Instance.gameObject);
            Destroy(wolfSpider2Instance.gameObject);
            Destroy(grandmotherInstance);
            Destroy(catInstance);
        }
    }
}
