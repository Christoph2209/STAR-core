using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[CreateAssetMenu(menuName = "PawnComponent/DefaultTest")]
public class PawnComponent : MonoBehaviour
{
    public Pawn owner;

    public string namename;

    public bool isDefaultAction;
    public int defaultActionPriority;

    public List<Stats> stats;


    private void addStat(string name, float value)
    {
        Stats newStat = new();
        newStat.StatName = name;
        newStat.value = value;
        stats.Add(newStat);
        owner.UpdateStats();
    }



    public void EstablishPawnComponent(Pawn owner)
    {
        this.owner = owner;
    }

    public void OnCombatPhaseEnd()
    {
        Debug.Log("Component has ended it's combat phase");
    }

    public void OnCombatPhaseStart()
    {
        Debug.Log("Component has started it's combat phase");
    }

    public void OnMainPhaseEnd()
    {
        Debug.Log("Component has ended it's main phase");
    }

    public void OnMainPhaseStart()
    {
        addStat("TestStat", 777);
        Debug.Log("Component has started it's main phase");
    }

}


[System.Serializable]
public struct Stats
{
    public string StatName;
    public float value;
}