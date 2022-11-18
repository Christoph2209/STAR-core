using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveObject
{    
    //Variable names
    public int limitX = 2;
    public int limitY = 2;
    public int worldSeed = 0;
    public float volume_mu = 1.0F;
    public float volume_sfx = 1.0F;
    public List<GameObject> pawns;
    public List<Vector3> locate;
    public List<FactionCommander> faction;
    public List<string> names;
    
    public int getSize(List<GameObject> p)
    {
        return p.Count;
    }

   // public FactionCommander commander;

    public void setWorld(int worldX, int worldY, float vm, float vsfx)
    {
        limitX = worldX;
        limitY = worldY;
        volume_mu = vm;
        volume_sfx = vsfx;
    }

}
