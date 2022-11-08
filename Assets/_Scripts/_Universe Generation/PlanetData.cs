using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetData 
{
    public string name;
    public FactionCommander faction;
    public float[] position;

    public PlanetData(Pawn planet)
    {
        name = planet.name;
        position = new float[3];
        position[0] = planet.transform.position.x;
        position[1] = planet.transform.position.y;
        position[2] = planet.transform.position.z;

    }
}
