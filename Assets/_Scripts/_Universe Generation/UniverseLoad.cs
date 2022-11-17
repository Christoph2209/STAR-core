using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseLoad : MonoBehaviour
{
    //constants
    //sizes refer to the width of each of the squares
    private const int ZONE_SIZE = 4, SQUARE_SIZE = 3;
    //public variables
    public static int universeLength = 2, universeWidth = 2;
    public static int seed = 100;
    //private variables
    public GameObject[] zones;
    public float zoneProb = 0.1f;
    private int zonesGenerated;
    private int numberPawns;

    public SaveObject so;
    public GameObject planet;
    private UniverseSimulation universeSimulation;

    void Start()
    {
        universeSimulation = GetComponent<UniverseSimulation>();
        so = SaveManager.Load();
        LoadUniverse();
        numberPawns = so.pawns.Count;
    }

    // Update is called once per frame
    void LoadUniverse()
    {
        //load universe
        for (int i = 0; i < universeLength; i++)
        {
            for (int j = 0; j < universeWidth; j++)
            {
                for(int o = 0; o < numberPawns; o--)
                {
                    GameObject s = so.pawns.
                    universeSimulation.GeneratePawn();
                }
            }
        }

    }
}
