using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    [Header("Set in Inspector: Bed (Set in Prefab Inspector)")]
    public GameObject regularBed;
    public GameObject grandmotherBed;
    public GameObject catBed;

    public void Update()
    {
        
    }
    public void GrandmaInBed()
    {
        regularBed.SetActive(false);
        grandmotherBed.SetActive(true);
    }
    public void RegularBed()
    {
        this.regularBed.SetActive(true);
        this.grandmotherBed.SetActive(false);
    }
}
