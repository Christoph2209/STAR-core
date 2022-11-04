using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoHold : TransferableComponent
{
    [SerializeField]
    private ComponentResource resourceType;
    [SerializeField]
    private int maxResources = 10;
    [SerializeField]
    private int resources = 10;


    public override void EstablishPawnComponent(Pawn owner, UniverseSimulation universeSimulation)
    {
        base.EstablishPawnComponent(owner, universeSimulation);
        if (prioritys.TryAdd(ComponentPriority.SellOrder, -resources))
        {//Less resources, higher priority.
            Debug.Log("Added priority");
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
                            cargoHold.resources = -overflow;
                            cargoHold.prioritys[ComponentPriority.SellOrder] = -cargoHold.resources;
                            break;
                        }
                        else
                        {
                            cargoHold.resources = 0;
                            cargoHold.prioritys[ComponentPriority.SellOrder] = -cargoHold.resources;
                        }

                    }
                }
            }

        }
        return true;
        
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
                cargoHold.resources += excess;//add excess resources to resources

                excess = Mathf.Max(0, cargoHold.resources - cargoHold.maxResources);// set excess equal to how much resources overflowed. If the value is less than or equal to 0 clam the excess to 0
                cargoHold.resources = Mathf.Clamp(cargoHold.resources, 0, cargoHold.maxResources);
                cargoHold.prioritys[ComponentPriority.SellOrder] = -cargoHold.resources;

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
                    newCargoHold.resources = excess;
                    newCargoHold.prioritys[ComponentPriority.SellOrder] = -newCargoHold.resources;
                    excess = 0;
                }
                else
                {
                    newCargoHold.resources = newCargoHold.maxResources;
                    newCargoHold.prioritys[ComponentPriority.SellOrder] = -newCargoHold.resources;
                    excess -= newCargoHold.resources;
                }
            }
        }
        return excess;
    }


}
