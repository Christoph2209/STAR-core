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

   public void QuitGame()
   {
      Application.Quit();
      Debug.Log("Game is exiting");
   }
}