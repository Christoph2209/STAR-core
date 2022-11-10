using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : PawnComponent
{

    [SerializeField]
    private GameObject cargoHoldPrefab;
    public override void OnMainPhaseStart()
    {
        base.OnMainPhaseStart();

        CargoHold.AddResources(owner, cargoHoldPrefab, ComponentResource.Rare, (int) owner.stats[ComponentStat.RareResource]);
        CargoHold.AddResources(owner, cargoHoldPrefab, ComponentResource.Medium, (int)owner.stats[ComponentStat.MediumResource]);
        CargoHold.AddResources(owner, cargoHoldPrefab, ComponentResource.WellDone, (int)owner.stats[ComponentStat.WellResource]);

    }
}
