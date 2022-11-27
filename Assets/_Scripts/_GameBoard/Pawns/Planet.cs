using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : Pawn
{
    [SerializeField]
    GameObject desolatedPlanet;
    protected override void CriticalDamage()
    {
        base.CriticalDamage();
        universeSimulation.GeneratePawn(desolatedPlanet, null, "Desolated Planet", transform.position);
    }
}
