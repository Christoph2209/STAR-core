using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;



public class SettingsGroup : MonoBehaviour
{
    private int x, y;
    private float v;
    public Text worldText;
    public AudioSource musi;
    public AudioSource sefx;


    public void close(string scene)
    {
        scene = "Assets/Scenes/Menus/Main/StartMenu.unity";
        SceneManager.LoadScene(scene);
    }


    public void setMusicVolume(float f)
    {
        musi.volume = f;
    }

    public void setSFXVolume(float f)
    {
        sefx.volume = f;
    }

    public void worldX(string z)
    {
        x = int.Parse(z);
        UniverseGeneration.universeLength = x;
        worldText.text = ("Your current World Values are (" + UniverseGeneration.universeLength + ", " + UniverseGeneration.universeWidth + ")");
        Debug.LogError("Value changed to " + x);
    }

    public void worldY(string z)
    {
        y = int.Parse(z);
        UniverseGeneration.universeWidth = y;
        worldText.text = ("Your current World Values are (" + UniverseGeneration.universeLength + ", " + UniverseGeneration.universeWidth + ")");
        Debug.LogError("Value changed to " + y);
    }
}
