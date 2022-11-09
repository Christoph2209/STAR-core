using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiComponentShop : PawnComponent
{ 
    [SerializeField]
    private List<GameObject> inventory;
    [SerializeField]
    private List<Cost> resourceCost;

    private float interactionRange = 3;


    public void PurchaseItem()
    {
        //purchase 
        List<Pawn> friendlyPawnsInRange = universeSimulation.GetAllPawnsInRange(owner.GetFaction(), owner.transform.position, interactionRange);
        if (CargoHold.TryRemoveResources(friendlyPawnsInRange, resourceCost))
        {
            foreach (GameObject item in inventory)
            {
                owner.InstantiatePawnComponent(item);
            }
            Debug.Log("Purchase Succesful!");
        }
        else
        {
            Debug.Log("Purchase Failure!");
        }
    }



}
