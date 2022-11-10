using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour
{
    public SaveObject so;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveManager.Save(so);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            so = SaveManager.Load();
        }
    }

    public void Save()
    {
        SaveManager.Save(so);
        Debug.LogError("Saved");
    }
}
