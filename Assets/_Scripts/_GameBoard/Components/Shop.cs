using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : PawnComponent
{

    private List<GameObject> Inventory;

    private List<Cost> GetComponentCost(GameObject componentPrefab)
    {
        return componentPrefab.GetComponent<PawnComponent>().price;
    }
    private GameObject GetComponentIcon(GameObject componentPrefab)
    {
        return componentPrefab.GetComponent<PawnComponent>().icon;
    }
    //This method is used to purchase an item. The resources can be pulled from any pawn. The target recieves the pawn component
    private bool TryPurchaseItem(List<Pawn> customers, Pawn target, GameObject pawnComponent)
    {
        List<Cost> resourceCost = GetComponentCost(pawnComponent);

        if(CargoHold.TryRemoveResources(customers, resourceCost))
        {
            target.AddPawnComponent(pawnComponent);
            return true;
        }
        else
        {
            return false;
        }

    }


}
