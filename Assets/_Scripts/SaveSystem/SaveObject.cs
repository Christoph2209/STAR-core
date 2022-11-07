[System.Serializable]
public class SaveObject
{
    public string name = "blank";
    public int locationX = 0;
    public int locationY = 0;
    public int locationZ = 0;
    public FactionCommander commander;

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
