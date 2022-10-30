using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : PawnComponent
{
    [SerializeField]
    private GameObject inventory;
    private float interactionRange = 3 ;
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
    public void PurchaseItem()
    {
        //purchase 
        List<Pawn> pawnsInRange = universeSimulation.GetAllPawnsInRange(owner.GetFaction(), owner.transform.position, interactionRange);
        if (TryPurchaseItem(pawnsInRange , owner , inventory ))
        {
            Debug.Log("Purchase successful!");
        }
        else
        {
            Debug.Log("Purchase failure!");
        }
    }
    
    


}
