using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenuGroup : MonoBehaviour
{
    [SerializeField]
    TMP_Text winText;
    public static string winner="";

    private void Awake()
    {
        if(winner == "")
        {
            winText.text = "HA. You are all losers.";
        }
        else
        {
            winText.text = winner.ToUpper() + " WINS!";
        }
    }


    public void ToStart(string scene)
    {
        scene = ("Assets/Scenes/Menus/Main/StartMenu.unity");
        SceneManager.LoadScene("StartMenu");
    }


   public void QuitGame()
   {
      Application.Quit();
      Debug.Log("Game is exiting");
   }
}
