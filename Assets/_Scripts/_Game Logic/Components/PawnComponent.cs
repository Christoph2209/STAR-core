using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;


/// <summary>
/// MonoBehavior methods should not used by objects inheriting from this class.
/// MonoBehavior is present to utilize unity's built in dependency injection through serialized fields.
/// </summary>
public abstract class PawnComponent : MonoBehaviour
{
    public Pawn owner;
    
    public TurnPhase activeTurnPhase = TurnPhase.Main;

    public bool isDefaultAction;
    public int defaultActionPriority;

    UniverseSimulation universeSimulation;

    /// <summary>
    /// The same component should never have more than one of the same starting stat
    /// </summary>
    [SerializeField]
    private List<startingStat> startingStats;
    protected Dictionary<string, float> stats = new();
    public ReadOnlyDictionary<string, float> Stats { get => new ReadOnlyDictionary<string, float>(stats); }




    public virtual void EstablishPawnComponent(Pawn owner, UniverseSimulation universeSimulation)
    {
        if(activeTurnPhase!= universeSimulation.universeChronology.currentPhase)
        {
            gameObject.SetActive(false);
        }
        foreach (startingStat baseStat in startingStats)
        {
            stats.Add(baseStat.StatName, baseStat.value);
        }
        this.owner = owner;
        Debug.Log("Owner:" + owner);

        this.universeSimulation = universeSimulation;
        this.universeSimulation.universeChronology.MainPhaseStart.AddListener(() => OnMainPhaseStart());
        this.universeSimulation.universeChronology.MainPhaseEnd.AddListener(() => OnMainPhaseEnd());
        this.universeSimulation.universeChronology.CombatPhaseStart.AddListener(() => OnCombatPhaseStart());
        this.universeSimulation.universeChronology.CombatPhaseEnd.AddListener(() => OnCombatPhaseEnd());
    }





    private void OnDestroy()
    {
        owner.UpdateStats();
    }




    public void OnMainPhaseStart()
    {
        if(activeTurnPhase == TurnPhase.Main)
        {
            gameObject.SetActive(true);
        }
    }
    public void OnMainPhaseEnd()
    {
        if(activeTurnPhase == TurnPhase.Main)
        {
            gameObject.SetActive(false);
        }
    }
    public void OnCombatPhaseStart()
    {
        if (activeTurnPhase == TurnPhase.Combat)
        {
            gameObject.SetActive(true);
        }
    }
    public void OnCombatPhaseEnd()
    {
        if (activeTurnPhase == TurnPhase.Combat)
        {
            gameObject.SetActive(false);
        }
    }



    public void TakeAction(AIBehavior behavior)
    {
        switch (behavior)
        {
            case AIBehavior.Deffensive:
                DeffensiveAction();
                break;
            case AIBehavior.Aggressive:
                AggressiveAction();
                break;
            case AIBehavior.Passive:
                PassiveAction();
                break;
            case AIBehavior.Expansionist:
                ExpansionistAction();
                break;
        }
    }

    protected virtual void DeffensiveAction() { }
    protected virtual void AggressiveAction() { }
    protected virtual void PassiveAction() { }
    protected virtual void ExpansionistAction() { }




    [System.Serializable]
    private struct startingStat
    {
        public string StatName;
        public float value;
    }
}

public enum AIBehavior { Deffensive, Aggressive, Passive, Expansionist}

