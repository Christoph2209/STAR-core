using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PawnFactory : PawnComponent
{
    [SerializeField]
    private List<GameObject> FactionShips;
    [SerializeField]
    private List<Cost> pawnCost;

    private float interactionRange = 3;

    [SerializeField]
    TMP_Text rare;
    [SerializeField]
    TMP_Text medium;
    [SerializeField]
    TMP_Text well;

    public override void EstablishPawnComponent(Pawn owner, UniverseSimulation universeSimulation)
    {
        base.EstablishPawnComponent(owner, universeSimulation);
        rare.text = "0";
        medium.text = "0";
        well.text = "0";
        foreach (Cost c in pawnCost)
        {
            switch (c.type)
            {
                case ComponentResource.Rare:
                    rare.text = c.value.ToString();
                    break;
                case ComponentResource.Medium:
                    medium.text = c.value.ToString();
                    break;
                case ComponentResource.WellDone:
                    well.text = c.value.ToString();
                    break;
                default:
                    break;
            }
        }
    }
    //This method is used to purchase an item. The resources can be pulled from any pawn. The target recieves the pawn component
    private bool TryPurchasePawn(List<Pawn> customers, GameObject pawnPrefab)
    {
        if (CargoHold.TryRemoveResources(customers, pawnCost))
        {
            universeSimulation.GeneratePawn(pawnPrefab, owner.GetFaction(), "ShipBuiltInFactory", owner.transform.position + new Vector3(0, 0, -1f));
            return true;
        }
        else
        {
            return false;
        }

    }
    private GameObject GetFactiongShip()
    {
        int index = universeSimulation.factionsInPlay.IndexOf(owner.GetFaction());
        return FactionShips[index];
    }
    public void PurchaseItem()
    {
        //purchase 
        List<Pawn> pawnsInRange = universeSimulation.GetAllPawnsInRange(owner.GetFaction(), owner.transform.position, interactionRange);
        if (TryPurchasePawn(pawnsInRange, GetFactiongShip()))
        {
            Debug.Log("Purchase successful!");

            //AUDIO CALL
            AudioManager.Instance.PlayCraftSFX();
        }
        else
        {
            Debug.Log("Purchase failure!");
        }
    }

    protected override void AggressiveAction()
    {
        base.AggressiveAction();
        PurchaseItem();
    }
}
