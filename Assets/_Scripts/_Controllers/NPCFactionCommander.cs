using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFactionCommander : FactionCommander
{
    //TODO AI and stuff
    AIBehavior behavior = AIBehavior.Passive;
    public override void OnMainPhaseStart()// main phase activities go here
    {
        Debug.Log("NPC taking main phase actions!");
    }
    public override void OnMainPhaseEnd()
    {

    }



    public override void OnCombatPhaseStart()// combat phase activities go here
    {
        Debug.Log("NPC taking combat phase actions!");
    }
    public override void OnCombatPhaseEnd()
    {

    }
}
