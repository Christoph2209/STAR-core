using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettleSystem : PawnComponent
{
    public float settleRange=2;

    public void Settle(FactionCommander faction)
    {
        bool isFriendlyPawnInRange = false;
        bool isEnemyPawnInRange = false;
        foreach (Pawn pawnInRange in universeSimulation.GetAllPawnsInRange(owner.transform.position, settleRange))
        {
            if (pawnInRange.GetFaction() == faction)
            {
                Debug.Log("Pawn In Range!");
                isFriendlyPawnInRange = true;
            }
            else if(pawnInRange.GetFaction()!=null && pawnInRange != owner)
            {
                Debug.Log(pawnInRange);
                Debug.Log("Enemy Pawn In Range!");
                isEnemyPawnInRange = true;
            }
        }
        if (isFriendlyPawnInRange && !isEnemyPawnInRange)
        {
            Debug.Log(faction.name + " Is settleing the " + owner.name + " system.");
            owner.SetFaction(faction);
        }
        else
        {
            Debug.Log("Contested system, can't settle");
        }
        
    }

    // Start is called before the first frame update
    public void Settle()
    {
        FactionCommander actingFaction = universeSimulation.playerFactionCommander;
        Settle(actingFaction);
        
    }
}
