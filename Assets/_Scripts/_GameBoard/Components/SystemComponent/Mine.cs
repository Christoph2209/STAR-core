using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : PawnComponent
{
    [SerializeField]
    private List<Cost> resourceGeneration;
    [SerializeField]
    private GameObject cargoHold;
    public override void OnMainPhaseStart()
    {
        foreach (Cost resource in resourceGeneration)
        {
            CargoHold.AddResources(owner, cargoHold, resource.type, resource.value);
        }
    }
}
