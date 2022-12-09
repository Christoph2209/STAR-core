using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFactionCommander : FactionCommander
{
    //TODO AI and stuff
    AIBehavior behavior = AIBehavior.Passive;

    
    public override void OnMainPhaseStart()// main phase activities go here
    {
        StartCoroutine(MainPhase());
    }

    public override void OnMainPhaseEnd()
    {

    }



    public override void OnCombatPhaseStart()// combat phase activities go here
    {
        StartCoroutine(CombatPhase());
    }
    public override void OnCombatPhaseEnd()
    {
        
    }


    IEnumerator MainPhase()
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log("NPC taking main phase actions!");
        foreach (Pawn p in universeSimulation.GetAllFactionPawns(this))
        {
            foreach (PawnComponent pc in p.GetPawnComponents())
            {
                if (pc.activeTurnPhase == TurnPhase.TraderPhase)
                {
                    pc.TakeAction(AIBehavior.Aggressive);
                }
            }
        }
        yield return null;
        base.CompletePhase();
    }
    IEnumerator CombatPhase()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("NPC taking combat phase actions!");
        Debug.Log("NPC taking main phase actions!");
        foreach (Pawn p in universeSimulation.GetAllFactionPawns(this))
        {
            foreach (PawnComponent pc in p.GetPawnComponents())
            {
                if (pc.activeTurnPhase == TurnPhase.RaiderPhase)
                {
                    pc.TakeAction(AIBehavior.Aggressive);
                }
            }
        }
        yield return null;
        base.CompletePhase();
    }
}
