using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// This script goes on it's own game object to handle high level tutorial stuff
/// </summary>

public class TutorialLoop : MonoBehaviour
{
    [SerializeField]
    private UniverseSimulation universeSimulation;// universe simulation needs to be slotted into this field in the inspector
    private UniverseChronology universeChronology;

    public GameObject Panel;
    public TMP_Text TxtObj;

    private string Txt;
    private Vector3 Pos;

    private void Start()
    {
        universeChronology = universeSimulation.universeChronology;
        universeChronology.OnSetup.AddListener(() => OnSetup());
        universeChronology.MainPhaseStart.AddListener(() => OnMainPhaseStart());
        universeChronology.CombatPhaseStart.AddListener(() => OnCombatPhaseStart());
    }


    //pick your home system
    public void OnSetup()
    {
        Debug.Log("TUTORIAL: setup phase, turn: " + universeChronology.GetTurnCount());

        Pos = new Vector3(0, 0, 0);
        Txt = ("Welcome to Space Traders and Raiders. " +
        "It is far in the future and humanity is in a constant power struggle. " +
        "Stake your claim and conquer the universe by way of commerce…or force.");

        SetPanel(Pos, Txt, 0.5f, 0.7f);
    }


   
    //called at the start of the phase, turn number can be gathered.
    public void OnMainPhaseStart()
    {
        Debug.Log("TUTORIAL: main phase, turn: " + universeChronology.GetTurnCount());
        switch (universeChronology.GetTurnCount())
        {
            case 1:
                Debug.Log("TUTORIAL: It's the first turn!");

                Pos = new Vector3(200, 285, 0);
                Txt = ("“PlAYER 1… It’s time to claim your territory. " +
                "PLANETS and SHIPS that you own can be modified with items called COMPONENTS. " +
                "These COMPONENTS are created by converting RESOURCES that you must acquire across the universe. " +
                "Move around and open the Menu on the planet you wish to be your HOME SYSTEM with this in mind.”");

                SetPanel(Pos, Txt, 0.2f, 0.5f);
                break;
            case 2:
                Debug.Log("TUTORIAL: It's the second turn!");
                break;
            default:
                break;
        }
    }

    public void OnCombatPhaseStart()
    {
        Debug.Log("TUTORIAL: combat phase, turn: " + universeChronology.GetTurnCount());
    }

    private void SetPanel(Vector3 NewPos, string TextToPrint, float ScaleX, float ScaleY)
    {
        Panel.transform.localPosition = NewPos;
        Debug.Log(Panel.transform.position);

        Panel.gameObject.transform.localScale = new Vector3(ScaleX, ScaleY, 0);

        TxtObj.text = TextToPrint;
    }

}
