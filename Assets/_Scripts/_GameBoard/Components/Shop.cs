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

    //private bool TryPurchaseItem(List<Pawn> customers, GameObject item)
    //{
    //    int cost = GetComponentCost(item);
    //    CargoHold.TryRemoveResources(customers,cost.)
    //}


}
