using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This script goes on it's own game object to handle high level tutorial stuff
/// </summary>
public class TutorialLoop : MonoBehaviour
{
    [SerializeField]
    private UniverseSimulation universeSimulation;// universe simulation needs to be slotted into this field in the inspector
    private UniverseChronology universeChronology;
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

    }


   
    //called at the start of the phase, turn number can be gathered.
    public void OnMainPhaseStart()
    {
        Debug.Log("TUTORIAL: main phase, turn: " + universeChronology.GetTurnCount());
        switch (universeChronology.GetTurnCount())
        {
            case 1:
                Debug.Log("TUTORIAL: It's the first turn!");
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

}
