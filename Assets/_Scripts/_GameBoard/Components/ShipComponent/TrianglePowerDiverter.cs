using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrianglePowerDiverter : PawnComponent
{

    public override void EstablishPawnComponent(Pawn owner, UniverseSimulation universeSimulation)
    {
        base.EstablishPawnComponent(owner, universeSimulation);
        PowerCell powerCell = GetComponentInChildren<PowerCell>();
        powerCell.OnSliderValueUpdate = (x) => OnSliderValueUpdate(x);
        powerCell.ResetPowerCell();
    }
    public void OnSliderValueUpdate(Dictionary<ComponentStat, float> updatedValues)
    {
        owner.stats.TryAdd(ComponentStat.AggregatePower, 0);
        foreach(KeyValuePair<ComponentStat,float> updatedValue in updatedValues)
        {
            
            stats.TryAdd(updatedValue.Key, 0);
            stats[updatedValue.Key] = updatedValue.Value*owner.stats[ComponentStat.AggregatePower];

        }
        owner.UpdateStats();

    }
}
