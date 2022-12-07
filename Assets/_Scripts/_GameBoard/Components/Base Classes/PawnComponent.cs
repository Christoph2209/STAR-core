using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

using TMPro;

/// <summary>
/// MonoBehavior methods should not used by objects inheriting from this class.
/// MonoBehavior is present to utilize unity's built in dependency injection through serialized fields.
/// </summary>
public abstract class PawnComponent : MonoBehaviour
{

    public List<Cost> price;

    [SerializeField]
    private float maxHealth;
    protected float currentHealth;
    [SerializeField]
    private TMP_Text healthText;
    
    public Pawn owner;
    
    public TurnPhase activeTurnPhase = TurnPhase.TraderPhase;
    public bool isForeign = false;

    protected UniverseSimulation universeSimulation;

    /// <summary>
    /// The same component should never have more than one of the same starting stat
    /// </summary>
    [SerializeField]
    private List<Priority> startingPrioritys;
    protected Dictionary<ComponentPriority, int> prioritys;
    public ReadOnlyDictionary<ComponentPriority,int> Prioritys { get => new ReadOnlyDictionary<ComponentPriority, int>(prioritys); }
    [SerializeField]
    private List<StartingStat> startingStats;
    protected Dictionary<ComponentStat, float> stats;
    public ReadOnlyDictionary<ComponentStat, float> Stats { get => new ReadOnlyDictionary<ComponentStat, float>(stats); }



    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }
    public virtual void EstablishPawnComponent(Pawn owner, UniverseSimulation universeSimulation)
    {
        if(activeTurnPhase== universeSimulation.universeChronology.currentPhase|| activeTurnPhase == TurnPhase.Every)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }


        if (prioritys == null)//if the prioritys have not been set we set them to there default
        {
            prioritys = new();
            foreach (Priority basePriority in startingPrioritys)
            {
                prioritys.Add(basePriority.Name, basePriority.Value);
            }
        }
        if (stats == null)//if the stats have nto been set, we set them to there default
        {
            stats = new();
            foreach (StartingStat baseStat in startingStats)
            {
                stats.Add(baseStat.Name, baseStat.Value);
            }
        }


        //Remove Listeners from previous owners
        if (this.owner != null)
        {
            this.owner.OnFactionUpdate.RemoveListener(() => OnFactionUpdate());
        }
        if (this.universeSimulation != null)
        {
            this.universeSimulation.universeChronology.MainPhaseStart.RemoveListener(() => OnMainPhaseStart());
            this.universeSimulation.universeChronology.MainPhaseEnd.RemoveListener(() => OnMainPhaseEnd());
            this.universeSimulation.universeChronology.CombatPhaseStart.RemoveListener(() => OnCombatPhaseStart());
            this.universeSimulation.universeChronology.CombatPhaseEnd.RemoveListener(() => OnCombatPhaseEnd());
        }

        //add listeners for new owner
        this.owner = owner;
        this.owner.OnFactionUpdate.AddListener(() => OnFactionUpdate());

        this.universeSimulation = universeSimulation;
        this.universeSimulation.universeChronology.MainPhaseStart.AddListener(() => OnMainPhaseStart());
        this.universeSimulation.universeChronology.MainPhaseEnd.AddListener(() => OnMainPhaseEnd());
        this.universeSimulation.universeChronology.CombatPhaseStart.AddListener(() => OnCombatPhaseStart());
        this.universeSimulation.universeChronology.CombatPhaseEnd.AddListener(() => OnCombatPhaseEnd());


        gameObject.GetComponent<RectTransform>().SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        
    }







    public virtual void OnMainPhaseStart()
    {
        if(activeTurnPhase == TurnPhase.TraderPhase|| activeTurnPhase == TurnPhase.Every)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public virtual void OnMainPhaseEnd()
    {
            gameObject.SetActive(false);
    }
    public virtual void OnCombatPhaseStart()
    {
        if (activeTurnPhase == TurnPhase.RaiderPhase || activeTurnPhase == TurnPhase.Every)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public virtual void OnCombatPhaseEnd()
    {
            gameObject.SetActive(false);
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

        UpdateHealthDisplay();
        return overflow;
    }
    public virtual void CriticalDamage()
    {
        owner.DestroyPawnComponent(gameObject);
    }

    private void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth / maxHealth * 100 + "%";
        }
    }

    public void RepairComponent()
    {
        currentHealth = maxHealth;
        UpdateHealthDisplay();
    }


    protected virtual void DeffensiveAction() { }
    protected virtual void AggressiveAction() { }
    protected virtual void PassiveAction() { }
    protected virtual void ExpansionistAction() { }

    public float GetHealth()
    {
        return currentHealth;
    }


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


[System.Serializable]
public struct Cost
{
    public ComponentResource type;
    public int value;
}
public enum AIBehavior { Deffensive, Aggressive, Passive, Expansionist}

