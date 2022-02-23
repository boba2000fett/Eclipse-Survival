using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static GameObject x;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (x == null)
        {
            x = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
