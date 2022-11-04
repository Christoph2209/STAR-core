using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactionCommander : MonoBehaviour
{
    
    [SerializeField]
    protected UniverseSimulation universeSimulation;
    public string factionName = default;
    protected FactionCommander actingFaction;

    #region Initialization
    private void Start()
    {
        actingFaction = this;
    }
    //Establish Faction is not to be called at any point except after the faction is created.
    //Only Universe Simulation should be able to call EstablishFaction when they are actively initializng that faction.
    public virtual bool EstablishFaction(string name, UniverseSimulation universeSimulation)
    {
        if(factionName== name)
        {
            Debug.LogError("FACTION ALREADY ESTABLISHED \n THIS SHOULD ONLY EVER BE CALLED ONCE");
            return false;
        }

        factionName = name;
        this.name = name + " Commander";
        this.universeSimulation = universeSimulation;


        universeSimulation.universeChronology.MainPhaseStart.AddListener(() => OnMainPhaseStart());
        universeSimulation.universeChronology.MainPhaseEnd.AddListener(() => OnMainPhaseEnd());
        universeSimulation.universeChronology.CombatPhaseStart.AddListener(() => OnCombatPhaseStart());
        universeSimulation.universeChronology.CombatPhaseEnd.AddListener(() => OnCombatPhaseEnd());


        return true;
    }
    #endregion


    public virtual void OnMainPhaseStart()
    {

    }
    public virtual void OnMainPhaseEnd()
    {

    }
    public virtual void OnCombatPhaseStart()
    {

    }
    public virtual void OnCombatPhaseEnd()
    {

    }





    #region FactionActions
    public void CompletePhase()// All factions need to mark themselves as ready to complete any given phase.
    {
        switch (universeSimulation.universeChronology.currentPhase)
        {
            case TurnPhase.SelectHomeSystem:
            case TurnPhase.Main:
            case TurnPhase.Combat:
                universeSimulation.universeChronology.MarkFactionReady(actingFaction);
                break;
            default:
                Debug.Log("Attempting to complete invalid phase, No actions taken");
                break;
        }
    }

    #endregion

    public FactionCommander GetActingFaction()
    {
        return actingFaction;
    }

}
