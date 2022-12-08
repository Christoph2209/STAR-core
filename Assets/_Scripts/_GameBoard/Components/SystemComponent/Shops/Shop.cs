using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Shop : PawnComponent
{
    [SerializeField]
    private bool destroyOnBuild = false;
    [SerializeField]
    private GameObject inventory;
    private float interactionRange = 3 ;
    [SerializeField]
    TMP_Text rare;
    [SerializeField]
    TMP_Text medium;
    [SerializeField]
    TMP_Text well;

    public override void EstablishPawnComponent(Pawn owner, UniverseSimulation universeSimulation)
    {
        base.EstablishPawnComponent(owner, universeSimulation);
        List<Cost> cost = GetComponentCost(inventory);
        rare.text = "0";
        medium.text = "0";
        well.text = "0";
        foreach (Cost c in cost)
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
    private List<Cost> GetComponentCost(GameObject componentPrefab)
    {
        return componentPrefab.GetComponent<PawnComponent>().price;
    }

    //This method is used to purchase an item. The resources can be pulled from any pawn. The target recieves the pawn component
    private bool TryPurchaseItem(List<Pawn> customers, Pawn target, GameObject pawnComponent)
    {
        List<Cost> resourceCost = GetComponentCost(pawnComponent);

        if(CargoHold.TryRemoveResources(customers, resourceCost))
        {
            target.InstantiatePawnComponent(pawnComponent);
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

            //AUDIO CALL
            AudioManager.Instance.PlayCraftSFX();

            if (destroyOnBuild)
            {
                owner.RemovePawnComponent(gameObject);
            }
        }
        else
        {
            Debug.Log("Purchase failure!");

            //AUDIO CALL
            AudioManager.Instance.PlayErrorSFX();
        }
    }
    
    /*
    protected override void AggressiveAction()
    {
        // item = victoryItem;
        //PurchaseItem();
        // item = ship;
        //PurchaseItem();
    }*/

}
