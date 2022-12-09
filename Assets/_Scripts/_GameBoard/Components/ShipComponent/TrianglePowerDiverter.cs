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
            stats[updatedValue.Key] = updatedValue.Value;

        }
        owner.UpdateStats();

    }

    protected override void AggressiveAction()
    {
        base.AggressiveAction();
        bool enemyNear = false;
        float range = 3; // PLACEHOLDER VALUE

        List<Pawn> possibleTargets = universeSimulation.GetAllPawnsInRange(owner.transform.position, range); // finds all pawns within range
        foreach (Pawn currentPawn in possibleTargets)
        {
            if(currentPawn.GetFaction() != owner.GetFaction()){enemyNear = true;}  // determines if an enemy is in range
        }

        if(enemyNear) // divert power to weapons (either 100% or 50/50 with shields)
        {
            stats[ComponentStat.SheildPower]=0.0f;
            stats[ComponentStat.ThrusterPower]=0.0f;
            stats[ComponentStat.WeaponPower]=1.0f;
        }
        else // divert power to thrusters 100%
        {
            stats[ComponentStat.SheildPower]=0.0f;
            stats[ComponentStat.ThrusterPower]=1.0f;
            stats[ComponentStat.WeaponPower]=0.0f;
        }
        owner.UpdateStats();
        enemyNear = false;
    }

    
}
