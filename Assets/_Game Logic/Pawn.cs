using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    [SerializeField]
    UniverseSimulation universeSimulation;
    [SerializeField]
    FactionCommander faction;

    
    public GameObject componentMenu;
    public Transform componentContainer;
    public GameObject statsMenu;


    #region Copy and lock inpsector value hack
    [SerializeField]
    private List<GameObject> setPawnComponents;//variable exposed in the inspector
    private List<GameObject> pawnComponentsReference => pawnComponents; // this holds a reference to the pawnComponents list that can not be written to
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
        setPawnComponents = pawnComponentsReference;
    }


    #endregion
    [SerializeField]// why does this make it work? I had a feeling it would work and it did, I don't have time to look into right now unfortunatley
    [HideInInspector]
    private List<GameObject> pawnComponents;//this is the real list that should be referenced by the code and shown at runtime


    private void Awake()
    {
        CopyInspectorPawnValues();
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

    public void OpenMenu(FactionCommander faction)
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
    public void CloseMenu()
    {
        componentMenu.SetActive(false);
        Debug.Log(this + " is closing menus");
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






    public virtual void OnMainPhaseStart()
    {
        foreach (GameObject pawnComponent in pawnComponents)
        {
            pawnComponent.GetComponent<PawnComponent>().OnMainPhaseStart();
        }
    }
    public virtual void OnMainPhaseEnd()
    {
        foreach (GameObject pawnComponent in pawnComponents)
        {
            pawnComponent.GetComponent<PawnComponent>().OnMainPhaseEnd();
        }
    }
    public virtual void OnCombatPhaseStart()
    {
        foreach (GameObject pawnComponent in pawnComponents)
        {
            pawnComponent.GetComponent<PawnComponent>().OnCombatPhaseStart();
        }
    }
    public virtual void OnCombatPhaseEnd()
    {
        foreach (GameObject pawnComponent in pawnComponents)
        {
            pawnComponent.GetComponent<PawnComponent>().OnCombatPhaseEnd();
        }
    }


}

