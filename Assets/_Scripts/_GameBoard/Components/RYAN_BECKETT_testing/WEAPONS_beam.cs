using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WEAPONS_beam : PawnComponent, PlayerControlOverride
{
    [SerializeField]
    private float damage;

    [SerializeField]
    private GameObject targetingUI;
    private Pawn target;

    public void Attack()
    {
        int RollValue;
        RollValue = Random.Range(1, 20);

        if (RollValue >= 10)
        {
            target.DamagePawn(damage * owner.GetStats(ComponentStat.WeaponPower) * owner.GetStats(ComponentStat.AggregatePower));
        }
    }

    public void SelectTarget()
    {

        universeSimulation.playerFactionCommander.OverrideInput(this);

    }

    public void OnMouseHighlight()
    {

    }

    public void OnMouseMove(InputValue value)
    {

    }

    public Vector3 OnMove(InputValue value)
    {
        throw new System.NotImplementedException();
    }

    public void OnOpenMenu(InputValue value)
    {
        throw new System.NotImplementedException();
    }

    public void OnSelect(InputValue value)
    {
        PlayerFactionCommander playerFactionCommander = universeSimulation.playerFactionCommander;
        playerFactionCommander.RestoreFactionInput();
    }
}
