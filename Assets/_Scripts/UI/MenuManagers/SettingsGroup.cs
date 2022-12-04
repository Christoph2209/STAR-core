using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;



public class SettingsGroup : MonoBehaviour
{
    private int x, y;
    public Text worldText;
    public AudioSource musi, sefx;
    public Slider mu, sf;
    public SaveObject so;

    void Start()
    {
        //sefx = SoundManager.Instance._SFXSource;
        //musi = SoundManager.Instance._MusicSource;
        
        so = SaveManager.Load();


        Debug.Log(sf);
        //sf.value = so.volume_sfx;
        //mu.value = so.volume_mu;
        x = so.limitX;
        y = so.limitY;
        
        //worldText.text = ("Your current World Values are (" + UniverseGeneration.universeLength + ", " + UniverseGeneration.universeWidth + ")");
        Debug.Log(so.limitX);
    }


    public void close(string scene)
    {
        SaveManager.Save(so);
        so.setWorld(so.limitX, so.limitY, so.volume_mu, so.volume_sfx);
        scene = "Assets/Scenes/Menus/StartMenu.unity";
        SceneManager.LoadScene(scene);
    }


    public void setMusicVolume(float f)
    {
        f = mu.value;
        Debug.Log("Music Volume is " + f);
        musi.volume = f;
        so.volume_mu = f;
    }

    public void setSFXVolume(float f)
    {
        f = sf.value;
        Debug.Log("SFX Volume is " + f);
        sefx.volume = f;
        so.volume_sfx = f;
    }

    public void worldX(string z)
    {
        x = int.Parse(z);
        UniverseGeneration.universeLength = x;
        worldText.text = ("Your current World Values are (" + UniverseGeneration.universeLength + ", " + UniverseGeneration.universeWidth + ")");
        Debug.Log("Value changed to " + x);
        so.limitX = x;
    }

    public void worldY(string z)
    {
        y = int.Parse(z);
        UniverseGeneration.universeWidth = y;
        worldText.text = ("Your current World Values are (" + UniverseGeneration.universeLength + ", " + UniverseGeneration.universeWidth + ")");
        Debug.Log("Value changed to " + y);
        so.limitY = y;
    }
}
