using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CargoHold : TransferableComponent
{
    [SerializeField]
    private ComponentResource resourceType;
    [SerializeField]
    private int maxResources = 10;
    [SerializeField]
    private int resources = 10;


    [SerializeField]
    TMP_Text rare;
    [SerializeField]
    TMP_Text medium;
    [SerializeField]
    TMP_Text well;



    public override void EstablishPawnComponent(Pawn owner, UniverseSimulation universeSimulation)
    {
        base.EstablishPawnComponent(owner, universeSimulation);
        if (prioritys.TryAdd(ComponentPriority.SellOrder, -resources))
        {//Less resources, higher priority.
            Debug.Log("Added priority");
        }

        UpdateResourceText();

    }

    private void UpdateResourceText()
    {
        rare.transform.parent.gameObject.SetActive(false);
        medium.transform.parent.gameObject.SetActive(false);
        well.transform.parent.gameObject.SetActive(false);

        switch (resourceType)
        {
            case ComponentResource.Rare:
                rare.text = resources + "/" + maxResources;
                rare.transform.parent.gameObject.SetActive(true);
                prioritys[ComponentPriority.DrawOrder] = -5;
                break;
            case ComponentResource.Medium:
                medium.text = resources + "/" + maxResources;
                medium.transform.parent.gameObject.SetActive(true);
                prioritys[ComponentPriority.DrawOrder] = -4;
                break;
            case ComponentResource.WellDone:
                well.text = resources +"/"+maxResources;
                well.transform.parent.gameObject.SetActive(true);
                prioritys[ComponentPriority.DrawOrder] = -3;
                break;
            default:
                break;
        }
    }

    public static int GetTotalResources(List<Pawn> pawns, ComponentResource componentResource)
    {
        int totalResources = 0;

        foreach (Pawn pawn in pawns)
        {
            List<PawnComponent> cargoList = pawn.GetComponentPriorityList(ComponentPriority.SellOrder);
            for (int i = 0; i < cargoList.Count; i++)
            {
                if (cargoList[i] is CargoHold cargoHold && cargoHold.resourceType == componentResource)
                {
                    totalResources += cargoHold.resources;
                }
            }
        }
            return totalResources;
    }

    public static bool TryRemoveResources(List<Pawn> pawns, List<Cost> resources)
    {
        foreach (Cost resource in resources)//verify the resources are there
        {
            if (GetTotalResources(pawns, resource.type) < resource.value || resource.value< 0)
            {
                AudioManager.Instance.PlayErrorSFX();

                Debug.Log("Failed To Purchase");
                return false;
            }
        }



        foreach (Cost resource in resources)// remove all of the resources
        {

            int overflow = resource.value;
            foreach (Pawn pawn in pawns)
            {
                List<PawnComponent> cargoList = pawn.GetComponentPriorityList(ComponentPriority.SellOrder);
                for (int i = 0; i < cargoList.Count; i++)
                {
                    if (cargoList[i] is CargoHold cargoHold && cargoHold.resourceType == resource.type)
                    {
                        overflow -= cargoHold.resources;
                        if (overflow <= 0)
                        {
                            SetResources(-overflow, cargoHold);
                            break;
                        }
                        else
                        {
                            SetResources(0, cargoHold);
                        }
                        cargoHold.UpdateResourceText();
                    }

                }
            }
            

        }
        return true;
        
    }

    private static void SetResources(int value, CargoHold cargoHold)
    {
        
        cargoHold.resources = value;
        cargoHold.prioritys[ComponentPriority.SellOrder] = -cargoHold.resources;
        cargoHold.UpdateResourceText();
        if(value == 0)
        {
            cargoHold.owner.RemovePawnComponent(cargoHold.gameObject);
        }
    }

    public static int AddResources(Pawn pawn, GameObject cargoHoldComponent, ComponentResource componentResource, int value)
    {
        if (value < 0)
        {
            Debug.Log("Invalid value");
            return value;
        }
        int excess = value;
        List<PawnComponent> cargoList = pawn.GetComponentPriorityList(ComponentPriority.SellOrder);
        for (int i = 0; i < cargoList.Count; i++)
        {
            if (cargoList[i] is CargoHold cargoHold && cargoHold.resourceType == componentResource)
            {
                
                SetResources(cargoHold.resources + excess, cargoHold);//add excess resources to resources
                excess = Mathf.Max(0, cargoHold.resources - cargoHold.maxResources);// set excess equal to how much resources overflowed. If the value is less than or equal to 0 clam the excess to 0
                SetResources(Mathf.Clamp(cargoHold.resources, 0, cargoHold.maxResources), cargoHold);
                
            }
        }
        if (excess > 0)
        {
            while (excess > 0)
            {
                CargoHold newCargoHold = pawn.InstantiatePawnComponent(cargoHoldComponent) as CargoHold;
                newCargoHold.resourceType = componentResource;
                if (newCargoHold.maxResources > excess)
                {
                    SetResources(excess, newCargoHold);
                    excess = 0;
                }
                else
                {
                    SetResources(newCargoHold.maxResources,newCargoHold);
                    excess -= newCargoHold.resources;
                }
                
            }
        }
        return excess;
    }


}
