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
    [SerializeField]
    private float maxHealth;
    private float currentHealth;

    public Pawn owner;
    
    public TurnPhase activeTurnPhase = TurnPhase.Main;
    public bool isForeign = false;

    protected UniverseSimulation universeSimulation;

    /// <summary>
    /// The same component should never have more than one of the same starting stat
    /// </summary>
    [SerializeField]
    private List<Priority> startingPrioritys;
    protected Dictionary<ComponentPriority, int> prioritys = new();
    public ReadOnlyDictionary<ComponentPriority,int> Prioritys { get => new ReadOnlyDictionary<ComponentPriority, int>(prioritys); }
    [SerializeField]
    private List<StartingStat> startingStats;
    protected Dictionary<ComponentStat, float> stats = new();
    public ReadOnlyDictionary<ComponentStat, float> Stats { get => new ReadOnlyDictionary<ComponentStat, float>(stats); }




    public virtual void EstablishPawnComponent(Pawn owner, UniverseSimulation universeSimulation)
    {
        if(activeTurnPhase!= universeSimulation.universeChronology.currentPhase)
        {
            gameObject.SetActive(false);
        }


        foreach (Priority basePriority in startingPrioritys)
        {
            prioritys.Add(basePriority.Name, basePriority.Value);
        }
        foreach (StartingStat baseStat in startingStats)
        {
            stats.Add(baseStat.Name, baseStat.Value);
        }
        

        this.owner = owner;
        Debug.Log("Owner:" + owner);


        this.universeSimulation = universeSimulation;
        this.universeSimulation.universeChronology.MainPhaseStart.AddListener(() => OnMainPhaseStart());
        this.universeSimulation.universeChronology.MainPhaseEnd.AddListener(() => OnMainPhaseEnd());
        this.universeSimulation.universeChronology.CombatPhaseStart.AddListener(() => OnCombatPhaseStart());
        this.universeSimulation.universeChronology.CombatPhaseEnd.AddListener(() => OnCombatPhaseEnd());

        owner.OnFactionUpdate.AddListener(()=>OnFactionUpdate());
        RepairComponent();

    }





    private void OnDestroy()
    {
        owner.UpdateStats();
    }




    public virtual void OnMainPhaseStart()
    {
        if(activeTurnPhase == TurnPhase.Main)
        {
            gameObject.SetActive(true);
        }
    }
    public virtual void OnMainPhaseEnd()
    {
        if(activeTurnPhase == TurnPhase.Main)
        {
            gameObject.SetActive(false);
        }
    }
    public virtual void OnCombatPhaseStart()
    {
        if (activeTurnPhase == TurnPhase.Combat)
        {
            gameObject.SetActive(true);
        }
    }
    public virtual void OnCombatPhaseEnd()
    {
        if (activeTurnPhase == TurnPhase.Combat)
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnFactionUpdate() { }


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

    public float DamageComponent(float damage)
    {
        if (damage < 0)
        {
            Debug.LogError("Damage can not be a negative number!");
            return 0;
        }
        currentHealth -= damage;
        float overflow = Mathf.Abs(Mathf.Min(currentHealth, 0f));

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        return overflow;
    }
    public void RepairComponent()
    {
        currentHealth = maxHealth;
    }


    protected virtual void DeffensiveAction() { }
    protected virtual void AggressiveAction() { }
    protected virtual void PassiveAction() { }
    protected virtual void ExpansionistAction() { }




    [System.Serializable]
    private struct StartingStat
    {
        public ComponentStat Name;
        public float Value;
    }
    [System.Serializable]
    private struct Priority
    {
        public ComponentPriority Name;
        public int Value;
    }
}



public enum AIBehavior { Deffensive, Aggressive, Passive, Expansionist}

