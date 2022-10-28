using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoHold : PawnComponent
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
        prioritys.TryAdd(ComponentPriority.SellOrder, -resources);//Less resources, higher priority.
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

    public static bool TryRemoveResources(List<Pawn> pawns,ComponentResource componentResource, int value)
    {
        if(GetTotalResources(pawns,componentResource)< value && value < 0)
        {
            Debug.Log("Invalid value");
            return false;
        }


        int overflow = value;
        foreach (Pawn pawn in pawns)
        {
            List<PawnComponent> cargoList = pawn.GetComponentPriorityList(ComponentPriority.SellOrder);
            for (int i = 0; i < cargoList.Count; i++)
            {
                if (cargoList[i] is CargoHold cargoHold && cargoHold.resourceType == componentResource)
                {
                    overflow -= cargoHold.resources;
                    if (overflow <= 0)
                    {
                        cargoHold.resources = -overflow;
                        break;
                    }
                    else
                    {
                        cargoHold.resources = 0;
                    }

                }
            }
        }
        return true;
        
    }


    public static int AddResources(Pawn pawn, ComponentResource componentResource, int value)
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

            }
        }

        return excess;
    }
}
