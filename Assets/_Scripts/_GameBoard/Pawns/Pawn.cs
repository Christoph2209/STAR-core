using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using UnityEngine.Events;
using UnityEditor;

public abstract class Pawn : MonoBehaviour
{

    public SaveObject so;

    [SerializeField]
    private UniverseSimulation universeSimulation;
    [SerializeField]
    private FactionCommander faction;
    [SerializeField]
    private GameObject componentMenu;
    [SerializeField]
    private GameObject foreignComponentMenu;
    [SerializeField]
    private Transform componentContainer;
    [SerializeField]
    private Transform foreignComponentContainer;
    [SerializeField]
    private GameObject statsMenu;
    [SerializeField]
    private TMP_Text statsText;
    [SerializeField]

    public Dictionary<ComponentStat, float> stats = new();


    #region Copy and lock inpsector value hack
    [SerializeField]
    private List<GameObject> setPawnComponents;//variable exposed in the inspector
    private List<GameObject> PawnComponentsReference => pawnComponents; // this holds a reference to the pawnComponents list that can not be written to
    private void CopyInspectorPawnValues()//call in awake
    {
        pawnComponents = new();
        foreach (GameObject pawnComponent in setPawnComponents)
        {
            
            if (pawnComponent != null)
            {
                InstantiatePawnComponent(pawnComponent);
            }
            else
            {
                pawnComponents.Add(null);
            }
        }
        setPawnComponents = PawnComponentsReference;
    }


    #endregion

    [SerializeField]// why does this make it work? I had a feeling it would work and it did, I don't have time to look into right now unfortunatley
    [HideInInspector]
    private List<GameObject> pawnComponents;//this is the real list that should be referenced by the code and shown at runtime
    private Dictionary<ComponentPriority, List<PawnComponent>> pawnComponentPriorityLists;
   
    private Action Move;
    private Action Attack;

    public UnityEvent OnStatsUpdate = new();
    public UnityEvent OnFactionUpdate= new();

    private void Start()// we want to close these menus after they awake
    {
        CloseComponentMenu();
        CloseStatMenu();
    }


    public virtual void EstablishPawn(string name, UniverseSimulation universeSimulation, FactionCommander faction)
    {
        this.name = name;
        this.universeSimulation = universeSimulation;
        this.faction = faction;
        universeSimulation.universeChronology.MainPhaseStart.AddListener(() => OnMainPhaseStart());
        universeSimulation.universeChronology.MainPhaseEnd.AddListener(() => OnMainPhaseEnd());
        universeSimulation.universeChronology.CombatPhaseStart.AddListener(() => OnCombatPhaseStart());
        universeSimulation.universeChronology.CombatPhaseEnd.AddListener(() => OnCombatPhaseEnd());

        CopyInspectorPawnValues();

        OnPhaseTransition();

    }




    public void DefaultAction(FactionCommander faction)
    {
        switch (universeSimulation.universeChronology.currentPhase)
        {
            case TurnPhase.TraderPhase:
                if (faction == this.faction)
                {
                    Debug.Log("Friendly default main action!");
                }
                else
                {
                    Debug.Log("Foreign default main action!");
                }
                break;
            case TurnPhase.RaiderPhase:
                if (faction == this.faction)
                {
                    Debug.Log("Friendly default combat action!");
                }
                else
                {
                    Debug.Log("Foreign default combat action!");
                }
                break;
            default:
                Debug.Log("No default action for phase " + universeSimulation.universeChronology.currentPhase);
                break;
        }

    }

    public void OpenComponentMenu(FactionCommander faction)
    {
        if (faction == this.faction)
        {
            componentMenu.SetActive(true);
            Debug.Log("Opening"+ this +" Component Menu");
        }
        else
        {
            foreignComponentMenu.SetActive(true);
            Debug.Log("Opening "+ this +" Foreign Menu");
        }
        
    }
    public void CloseComponentMenu()
    {
        componentMenu.SetActive(false);
        foreignComponentMenu.SetActive(false);
        Debug.Log(this + " is closing menus");
    }
    public void OpenStatMenu(FactionCommander faction)
    {
        if (faction == this.faction)
        {
            statsMenu.SetActive(true);
            Debug.Log("Opening" + this + " Stat Menu");
        }
        else
        {

            Debug.Log("Opening " + this + " Foreign Stat Menu");
        }
    }
    public void CloseStatMenu()
    {
        statsMenu.SetActive(false);
    }



    //Pawn Componeent Creation/destruction/transfer
    public PawnComponent InstantiatePawnComponent(GameObject pawnComponentPrefab)
    {
        Debug.Assert(pawnComponentPrefab.TryGetComponent(typeof(PawnComponent), out _));


        GameObject newPawnComponent = Instantiate(pawnComponentPrefab,componentContainer);
        pawnComponents.Add(newPawnComponent);
        PawnComponent c = newPawnComponent.GetComponent<PawnComponent>();
        if (c.isForeign)
        {
            newPawnComponent.transform.SetParent( foreignComponentContainer);
        }
        else
        {
            newPawnComponent.transform.SetParent(componentContainer);
        }
        
        c.EstablishPawnComponent(this, universeSimulation);

        UpdatePrioritys();
        UpdateStats();

        return c;
    }

    public void TransferPawnComopnent(GameObject pawnComponentInstance)
    {
#if UNITY_EDITOR
        Debug.Assert(PrefabUtility.GetPrefabInstanceStatus(pawnComponentInstance) == PrefabInstanceStatus.NotAPrefab);
        Debug.Assert(pawnComponentInstance.TryGetComponent(typeof(PawnComponent), out _));
#endif
        PawnComponent c = pawnComponentInstance.GetComponent<PawnComponent>();

        //remove from previous
        Pawn previousOwner = c.owner;
        previousOwner.RemovePawnComponent(pawnComponentInstance);



        pawnComponents.Add(pawnComponentInstance);
        

        if (c.isForeign)
        {
            pawnComponentInstance.transform.SetParent(foreignComponentContainer);
        }
        else
        {
            pawnComponentInstance.transform.SetParent(componentContainer);
        }

        c.EstablishPawnComponent(this, universeSimulation);

        UpdatePrioritys();
        UpdateStats();
    }

    public void RemovePawnComponent(GameObject pawnComponent)
    {
        pawnComponents.Remove(pawnComponent);

        UpdatePrioritys();
        UpdateStats();

        pawnComponent.transform.SetParent(null,true);
    }
    public void DestroyPawnComponent(GameObject pawnComponent)
    {
        RemovePawnComponent(pawnComponent);
        Destroy(pawnComponent);
    }
    //---------------------------------------



    
    public List<T> GetPawnComponents<T>() where T : new()
    {
        List<T> pawnComponentsOfType = new();
        foreach (GameObject cgameObject in pawnComponents)
        {
            PawnComponent c = cgameObject.GetComponent<PawnComponent>();
         
            if (c is T componentOfType)
            {
                
                pawnComponentsOfType.Add(componentOfType);
            }
        }
        return pawnComponentsOfType;
    }
    public void UpdatePrioritys()
    {
        pawnComponentPriorityLists = new();
        foreach(GameObject pawnComponent  in pawnComponents)
        {
            PawnComponent script = pawnComponent.GetComponent<PawnComponent>();
            foreach(KeyValuePair<ComponentPriority,int> priority in script.Prioritys)
            {
                pawnComponentPriorityLists.TryAdd(priority.Key, new List<PawnComponent>());
                pawnComponentPriorityLists[priority.Key].Add(script);
            }
        }
        foreach(KeyValuePair<ComponentPriority, List<PawnComponent>> priorityList in pawnComponentPriorityLists)
        {
            ComponentPriority priorityName = priorityList.Key;
            pawnComponentPriorityLists[priorityName].Sort((priorityA, priorityB) => priorityB.Prioritys[priorityName].CompareTo(priorityA.Prioritys[priorityName]));
        }


        Debug.Log("______Draw Order______");
        for (int i = 0; i < pawnComponentPriorityLists[ComponentPriority.DrawOrder].Count; i++) {
            pawnComponentPriorityLists[ComponentPriority.DrawOrder][i].transform.SetSiblingIndex(i);
            Debug.Log(i + ".    " + pawnComponentPriorityLists[ComponentPriority.DrawOrder][i].name);
        }

    }
    
    public void UpdateStats()
    {
        stats = new();
        foreach (GameObject pawnComponent in pawnComponents)
        {
            PawnComponent script = pawnComponent.GetComponent<PawnComponent>();
            foreach(KeyValuePair<ComponentStat,float> stat in script.Stats)
            {
                stats.TryAdd(stat.Key, 0); //if the stat is not present creat a new one with a starting value of 0
                stats[stat.Key] += stat.Value;
            }
        }

        //update stat UI
        string statString = "";
        foreach(KeyValuePair<ComponentStat, float> stat in stats)
        {
            statString += stat.Key + ": " + stat.Value + "\n";
        }
        statsText.text = statString;
        OnStatsUpdate.Invoke();
    }




    public float GetStats(ComponentStat stat)
    {
        if (stats.TryGetValue(stat, out float value))
        {
            return value;
        }
        else
        {
            stats.Add(stat, 0);
            return 0;
        }
    }


    public void DamagePawn(float damage)
    {
        float excess = damage;
        if (pawnComponentPriorityLists.ContainsKey(ComponentPriority.DamageOrder))
        {
            for (int i = 0; i < pawnComponentPriorityLists[ComponentPriority.DamageOrder].Count; i++)
            {
                excess = pawnComponentPriorityLists[ComponentPriority.DamageOrder][i].DamageComponent(excess);
            }
        }

        if (excess > 0)
        {
            Debug.Log("CRITICAL DAMAGE HAS BEEN SUSTAINED!!!!");
            universeSimulation.DestroyPawn(this);
        }
        Debug.Log("Pawn has been damaged! " + GetTotalHealth() + " Health left!");
    }
    public float GetTotalHealth()
    {
        float health =0;
        if (pawnComponentPriorityLists.ContainsKey(ComponentPriority.DamageOrder))
        {
            for (int i = 0; i < pawnComponentPriorityLists[ComponentPriority.DamageOrder].Count; i++)
            {
                health += pawnComponentPriorityLists[ComponentPriority.DamageOrder][i].GetHealth();
            }
        }
        else
        {
            Debug.Log("This Pawn has no health!");
            return 0;
        }
        return health;
    }


    protected virtual void OnPhaseTransition()
    {
        Move = () => { Debug.Log("No move action taken!"); };
        Attack = () => { Debug.Log("No attack action taken!"); };
        CloseComponentMenu();
    }


    protected virtual void OnMainPhaseStart()
    {
        OnPhaseTransition();
    }
    protected virtual void OnMainPhaseEnd()
    {
        
        Move();
        OnPhaseTransition();
    }
    protected virtual void OnCombatPhaseStart()
    {
        OnPhaseTransition();
    }
    protected virtual void OnCombatPhaseEnd()
    {
        Attack();
        OnPhaseTransition();

    }



    public void SetMovePattern(Action moveMethod)
    {
        Move = moveMethod;
    }
    public void SetAttackPattern(Action attackMethod)
    {
        Attack = attackMethod;
    }









    //Getters/setters

    public FactionCommander GetFaction()
    {
        return faction;
    }
    public void SetFaction(FactionCommander faction)
    {
        this.faction = faction;
        OnFactionUpdate.Invoke();
    }

    public List<PawnComponent> GetComponentPriorityList(ComponentPriority cp)
    {
        if (pawnComponentPriorityLists.ContainsKey(cp))
        {
            return pawnComponentPriorityLists[cp];
        }
        else
        {
            return new List<PawnComponent> { };
        }
    }

}

