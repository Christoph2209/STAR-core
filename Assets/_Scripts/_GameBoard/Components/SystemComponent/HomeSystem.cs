using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This is the component is placed on the home system selected by the player. If it is destroyed the player looses the game. If a player controls the last home system they win.
/// </summary>
public class HomeSystem : PawnComponent 
{

    public override void EstablishPawnComponent(Pawn owner, UniverseSimulation universeSimulation)
    {
        base.EstablishPawnComponent(owner, universeSimulation);
        List<SystemInfo> trash = owner.GetPawnComponents<SystemInfo>();
        foreach(SystemInfo t in trash)
        {
            owner.RemovePawnComponent(t.gameObject);
        }
    }
}
