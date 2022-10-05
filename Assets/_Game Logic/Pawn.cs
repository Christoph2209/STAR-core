using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public abstract class Pawn : MonoBehaviour
{
    [SerializeField]
    UniverseSimulation universeSimulation;
    [SerializeField]
    FactionCommander faction;

    public delegate void MovePawn();
    public MovePawn movePawn;

    public void mvtest()
    {
        Debug.Log("No Move Action Taken");
    }

    public GameObject componentMenu;
    public Transform componentContainer;
    public GameObject statsMenu;
    public TMP_Text statsText;

    public Dictionary<string, float> stats = new();

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
                AddPawnComponent(pawnComponent);
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


    private void Awake()
    {
        CopyInspectorPawnValues();
    }
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
    }




    public void DefaultAction(FactionCommander faction)
    {
        switch (universeSimulation.universeChronology.currentPhase)
        {
            case TurnPhase.Main:
                if (faction == this.faction)
                {
                    Debug.Log("Friendly default main action!");
                }
                else
                {
                    Debug.Log("Foreign default main action!");
                }
                break;
            case TurnPhase.Combat:
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
            
            Debug.Log("Opening "+ this +" Foreign Menu");
        }
        
    }
    public void CloseComponentMenu()
    {
        componentMenu.SetActive(false);
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




    private void AddPawnComponent(GameObject pawnComponent)
    {
        Debug.Assert(pawnComponent.TryGetComponent(typeof(PawnComponent), out _));
        GameObject newPawnComponent = Instantiate(pawnComponent, componentContainer);
        pawnComponents.Add(newPawnComponent);
        newPawnComponent.GetComponent<PawnComponent>().EstablishPawnComponent(this);
    }

    private void RemovePawnComponent(GameObject pawnComponent)
    {
        pawnComponents.Remove(pawnComponent);
        Destroy(pawnComponent);
    }



    public void UpdateStats()
    {
        stats = new();
        foreach (GameObject pawnComponent in pawnComponents)
        {
            PawnComponent p = pawnComponent.GetComponent<PawnComponent>();
            foreach(Stats s in p.stats)
            {
                stats.TryAdd(s.StatName, 0);
                stats[s.StatName] += s.value;
            }
        }
        string statString = "";
        foreach(KeyValuePair<string, float> stat in stats)
        {
            statString += stat.Key + ": " + stat.Value + "\n";
        }
        statsText.text = statString;
        Debug.Log("UpdatedStats");
    }


   
    protected virtual void OnPhaseTransition()
    { 
        CloseComponentMenu();
    }


    public virtual void OnMainPhaseStart()
    {
        //movePawn => mvtest();
        OnPhaseTransition();
        foreach (GameObject pawnComponent in pawnComponents)
        {
            pawnComponent.GetComponent<PawnComponent>().OnMainPhaseStart();
        }
    }
    public virtual void OnMainPhaseEnd()
    {
        //movePawn();
        OnPhaseTransition();
        foreach (GameObject pawnComponent in pawnComponents)
        {
            pawnComponent.GetComponent<PawnComponent>().OnMainPhaseEnd();
        }
    }
    public virtual void OnCombatPhaseStart()
    {
        OnPhaseTransition();
        foreach (GameObject pawnComponent in pawnComponents)
        {
            pawnComponent.GetComponent<PawnComponent>().OnCombatPhaseStart();
        }
    }
    public virtual void OnCombatPhaseEnd()
    {
        OnPhaseTransition();
        foreach (GameObject pawnComponent in pawnComponents)
        {
            pawnComponent.GetComponent<PawnComponent>().OnCombatPhaseEnd();
        }
    }













    //Getters/setters

    public FactionCommander GetFaction()
    {
        return faction;
    }


}

