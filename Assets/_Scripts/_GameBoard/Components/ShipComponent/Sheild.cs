using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheild : TransferableComponent
{
    [SerializeField]
    private float sheildRegeneration;
    public override void OnMainPhaseStart()
    {
        base.OnMainPhaseStart();
        currentHealth += (sheildRegeneration * owner.stats[ComponentStat.SheildPower] * owner.stats[ComponentStat.AggregatePower]);
    }
    public override void CriticalDamage()
    {
        //just do nothing
    }
}
