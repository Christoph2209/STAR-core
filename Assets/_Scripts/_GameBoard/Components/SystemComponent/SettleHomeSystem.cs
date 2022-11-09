using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettleHomeSystem : PawnComponent
{
    [SerializeField]
    private List<GameObject> HomeSystemComponents;
    public override void EstablishPawnComponent(Pawn owner, UniverseSimulation universeSimulation)
    {
        base.EstablishPawnComponent(owner, universeSimulation);
    }
    public void Settle(FactionCommander faction)
    {

        List<Pawn> pawnsOwned = universeSimulation.GetAllFactionPawns(faction);

        if (pawnsOwned.Count>0)
        {
            Debug.Log("You have already selected a home system!");
            
        }
        else
        {
            owner.SetFaction(faction);
            Debug.Log("Selecting home system: " + owner +" "+owner.GetFaction());
            
            foreach(GameObject component in HomeSystemComponents)
            {
                owner.InstantiatePawnComponent(component);
            }
            gameObject.SetActive(false);
        }
        
    }

    // Start is called before the first frame update
    public void Settle()
    {
        FactionCommander actingFaction = universeSimulation.playerFactionCommander.GetActingFaction();
        Settle(actingFaction);
        activeTurnPhase = TurnPhase.None;
        owner.CloseComponentMenu();
    }
    protected override void OnFactionUpdate()
    {
        base.OnFactionUpdate();
        if(owner.GetFaction()== null)
        {
            activeTurnPhase = TurnPhase.Main;
        }
        else
        {
            activeTurnPhase = TurnPhase.None;
        }
    }
}
