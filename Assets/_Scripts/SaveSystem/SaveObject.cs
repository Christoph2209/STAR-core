using UnityEngine;

[System.Serializable]
public class SaveObject
{
    //Variable names
 /*   
    public GameObject prefab;
    public string name = "blank";
    public int locationX = 0;
    public int locationY = 0;
    public int locationZ = 0;*/
    public int limitX = 2;
    public int limitY = 2;
    public int worldSeed = 0;
    public float volume_mu = 1.0F;
    public float volume_sfx = 1.0F;
   // public FactionCommander commander;
    //Statics
//    public static int seed;

  //  public static int worldX;
    //public static int worldY;

    public void setWorld(int worldX, int worldY, float vm, float vsfx)
    {
        limitX = worldX;
        limitY = worldY;
        volume_mu = vm;
        volume_sfx = vsfx;
    }

    public int getWorldX()
    {
        return limitX;
    }

    public int getWorldY()
    {
        return limitY;
    }
    public void setSeed(int seed)
    {
        worldSeed = seed;
    }
/*
    public void setName(string name2)
    {
        name = name2;
    }

    public void setX(int a)
    {
        locationX = a;
    }

    public void setY(int a)
    {
        locationY = a;
    }

    public void setZ(int a)
    {
        locationZ = a;
    }

   */
}
