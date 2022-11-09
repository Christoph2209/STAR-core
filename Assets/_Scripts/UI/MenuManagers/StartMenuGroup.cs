using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuGroup : MonoBehaviour
{
    public void PlayGame(string scene)
   {
        scene = "Assets/Scenes/SampleScene.unity";
        SceneManager.LoadScene(scene);
   }

    public void ReportBug()
    {
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdYi0FtZtnpq76GEI5_deuLjHJ9PPHNVg-168bor4B2zt0SMQ/viewform?usp=sf_link");
    }

   public void QuitGame()
   {
      Application.Quit();
      Debug.Log("Game is exiting");
   }

    public void Settings(string scene)
    {
        scene = "Assets/Scenes/SettingScene.unity";
        SceneManager.LoadScene(scene);
    }

}