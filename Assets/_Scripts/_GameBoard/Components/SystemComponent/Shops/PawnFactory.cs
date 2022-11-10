using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnFactory : PawnComponent
{
    [SerializeField]
    private List<GameObject> FactionShips;
    [SerializeField]
    private List<Cost> resourceCost;

    private float interactionRange = 3;


    //This method is used to purchase an item. The resources can be pulled from any pawn. The target recieves the pawn component
    private bool TryPurchasePawn(List<Pawn> customers, GameObject pawnPrefab)
    {
        if (CargoHold.TryRemoveResources(customers, resourceCost))
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
        }
        else
        {
            Debug.Log("Purchase failure!");
        }
    }


}
