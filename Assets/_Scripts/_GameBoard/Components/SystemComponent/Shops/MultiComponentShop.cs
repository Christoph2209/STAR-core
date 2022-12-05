using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MultiComponentShop : PawnComponent
{
    [SerializeField]
    private bool destroyOnBuild= false;
    [SerializeField]
    private List<GameObject> inventory;
    [SerializeField]
    private List<Cost> resourceCost;

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

        foreach (Cost c in resourceCost)
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
            if (destroyOnBuild)
            {
                owner.RemovePawnComponent(gameObject);
            }
        }
        else
        {
            Debug.Log("Purchase Failure!");
        }
    }



}
