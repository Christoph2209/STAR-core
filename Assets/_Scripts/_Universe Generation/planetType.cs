using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planetType : MonoBehaviour
{
    
    public int planetTypeNum = 0;
    public Material[] types;

    void Start()
    {
        planetTypeNum = Random.Range(0, types.Length - 1);
        gameObject.GetComponent<Renderer>().material = types[planetTypeNum];
    }
}
