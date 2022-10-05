using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactionCommander : MonoBehaviour
{
    
    [SerializeField]
    protected UniverseSimulation universeSimulation;
    public string factionName = default;
    
    
    #region Initialization
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

        return true;
    }
    #endregion



    #region FactionActions
    public void CompletePhase()// All factions need to mark themselves as ready to complete any given phase.
    {
        switch (universeSimulation.universeChronology.currentPhase)
        {
            case TurnPhase.Main:
                universeSimulation.universeChronology.MarkFactionReady(this);
                break;
            case TurnPhase.Combat:
                universeSimulation.universeChronology.MarkFactionReady(this);
                break;
            default:
                Debug.Log("Attempting to complete invalid phase, No actions taken");
                break;
        }
    }

    #endregion

}
