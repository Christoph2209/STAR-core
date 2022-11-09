using UnityEngine;

[System.Serializable]
public class SaveObject
{
    //Variable names
    public GameObject prefab;
    public string name = "blank";
    public int locationX = 0;
    public int locationY = 0;
    public int locationZ = 0;
    public FactionCommander commander;
    
    public string seed;
    public int volume;
    public int worldX;
    public int worldY;


    public void setWorldX(int x)
    {
        worldX = x;
    }
    public void setWorldY(int y)
    {
        worldY = y;
    }

    public void setVol(int vol)
    {
        volume = vol;
    }

    public void setSeed(string see)
    {
        seed = see;
    }

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

   
}
