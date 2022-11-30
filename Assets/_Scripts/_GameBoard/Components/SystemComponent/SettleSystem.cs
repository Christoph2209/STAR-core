using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettleSystem : PawnComponent
{
    [SerializeField]
    private List<GameObject> newSytemComponents;


    public float settleRange=2;

    private GameObject circle;
    public void OnEnable()
    {
        if (universeSimulation == null|| universeSimulation.playerFactionCommander.GetActingFaction() == null)
        {
            return;
        }
        if (IsSettle(universeSimulation.playerFactionCommander.GetActingFaction()))
        {
            circle = DrawCircle.Create(owner.transform, owner.transform.position, Quaternion.Euler(90, 0, 0), settleRange, 0.1f, Color.green);
        }
        else
        {
            circle = DrawCircle.Create(owner.transform, owner.transform.position, Quaternion.Euler(90, 0, 0), settleRange, 0.1f, Color.yellow);
        }
    }
    public void OnDisable()
    {
        Destroy(circle);
    }
    public override void EstablishPawnComponent(Pawn owner, UniverseSimulation universeSimulation)
    {
        base.EstablishPawnComponent(owner, universeSimulation);
        if (owner.GetFaction() == null)
        {
            activeTurnPhase = TurnPhase.TraderPhase;
        }
        else
        {
            activeTurnPhase = TurnPhase.None;
        }

    }
    public void Settle(FactionCommander faction)
    {
        bool isSeblable = IsSettle(faction);



        if (isSeblable)
        {
            Debug.Log(faction.name + " Is settleing the " + owner.name + " system.");
            owner.SetFaction(faction);

            foreach (GameObject component in newSytemComponents)
            {
                owner.InstantiatePawnComponent(component);
            }
            gameObject.SetActive(false);


        }
        else
        {
            Debug.Log("Contested system, can't settle");
        }

    }

    private bool IsSettle(FactionCommander faction)
    {

        bool isFriendlyPawnInRange = false;
        bool isEnemyPawnInRange = false;
        foreach (Pawn pawnInRange in universeSimulation.GetAllPawnsInRange(owner.transform.position, settleRange))
        {
            if (pawnInRange.transform.TryGetComponent<Planet>(out _))
            {
                continue;
            }
            if (pawnInRange.GetFaction() == faction)
            {
                Debug.Log("Pawn In Range!");
                isFriendlyPawnInRange = true;
            }
            else if (pawnInRange.GetFaction() != null && pawnInRange != owner)
            {
                Debug.Log(pawnInRange);
                Debug.Log("Enemy Pawn In Range!");
                isEnemyPawnInRange = true;
            }
        }
        bool isSeblable = isFriendlyPawnInRange && !isEnemyPawnInRange;
        return isSeblable;
    }

    // Start is called before the first frame update
    public void Settle()
    {
        FactionCommander actingFaction = universeSimulation.playerFactionCommander.GetActingFaction();
        Settle(actingFaction);
        owner.CloseComponentMenu();
    }
    protected override void OnFactionUpdate()
    {
        base.OnFactionUpdate();
        if(owner.GetFaction()== null)
        {
            activeTurnPhase = TurnPhase.TraderPhase;
        }
        else
        {
            activeTurnPhase = TurnPhase.None;
        }
    }
}
