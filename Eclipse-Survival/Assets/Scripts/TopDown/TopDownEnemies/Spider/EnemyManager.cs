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
    public bool spawnedVariables = false;
    public bool awakeFunctionalityCompleted = false;
    public WolfSpiderRoaming wolfSpider1Instance;
    public WolfSpiderRoaming wolfSpider2Instance;
    public Grandmother grandmotherInstance;
    public Cat catInstance;



    public void Awake()
    {
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

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (SceneManager.GetActiveScene().name == "DownstairsTopLeftKitchen" && !exitedMenuScene)
        //{
        //    ActivateObjects();
        //}
        //else if (SceneManager.GetActiveScene().name == Constants.GAME_OVER_SCENE_NAME)
        //{
        //    exitedMenuScene = false;
        //    if (wolfSpider1Instance != null)
        //    {
        //        Destroy(wolfSpider1Instance.gameObject);
        //    }
        //    if (wolfSpider2Instance.gameObject != null)
        //    {
        //        Destroy(wolfSpider2Instance.gameObject);
        //    }
        //    if (grandmotherInstance.gameObject != null)
        //    {
        //        Destroy(grandmotherInstance.gameObject);
        //    }
        //    if (catInstance.gameObject != null)
        //    {
        //        Destroy(catInstance.gameObject);
        //    }
        //}
    }



    public void ActivateObjects()
    {
        exitedMenuScene = true;

        catInstance = GameObject.Find("Cat").GetComponent<Cat>();
        grandmotherInstance = GameObject.Find("Grandmother").GetComponent<Grandmother>();

        if (wolfSpider1Instance != null || wolfSpider2Instance != null)
        {
            return;
        }

        //GameObject go = GameObject.Instantiate(wolfSpider1Prefab.gameObject);
        wolfSpider1Instance = GameObject.Instantiate(wolfSpider1Prefab.gameObject).GetComponent<WolfSpiderRoaming>();

        //wolfSpider1Instance = GameObject.Instantiate(wolfSpider1Prefab);
        wolfSpider2Instance = GameObject.Instantiate(wolfSpider2Prefab);

        //GameObject go2 = GameObject.Instantiate(wolfSpider2Prefab.gameObject);
        //wolfSpider2Instance = go2.GetComponent<WolfSpiderRoaming>();
    }


    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {


        if (SceneManager.GetActiveScene().name == "DownstairsTopLeftKitchen" && !exitedMenuScene)
        {
            ActivateObjects();
        }
        else if (SceneManager.GetActiveScene().name == Constants.GAME_OVER_SCENE_NAME)
        {
            exitedMenuScene = false;
            Destroy(wolfSpider1Instance.gameObject);
            Destroy(wolfSpider2Instance.gameObject);
            Destroy(grandmotherInstance.gameObject);
            Destroy(catInstance.gameObject);
        }

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
