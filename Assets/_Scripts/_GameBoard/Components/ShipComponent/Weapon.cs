using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : TransferableComponent, PlayerControlOverride
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float range;
    [SerializeField]
    private Color rangeColor;
    [SerializeField]
    private GameObject targetingUI;
    private Pawn target;

    public void Attack()
    {
        target.DamagePawn(damage * owner.GetStats(ComponentStat.WeaponPower)* owner.GetStats(ComponentStat.AggregatePower));

        target = null;
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
        return Vector3.zero;
    }

    public void OnOpenMenu(InputValue value)
    {
        //throw new System.NotImplementedException();
    }

    public void OnSelect(InputValue value)
    {
        PlayerFactionCommander input = universeSimulation.playerFactionCommander;

        if (input.isOverUI)
        {
            target = null;
        }
        else
        {
            target = input.closestPawnToCursor;
        }

        if (target == null || Vector3.Distance(target.transform.position,owner.transform.position)>range)
        {
            Debug.Log("Nothing selected, try again");
        }
        else
        {
            Debug.Log(target + "Selected. Attacking Target");

            owner.SetAttackPattern(() => Attack());

            input.RestoreFactionInput();
        }
    }

    private GameObject circle;
    public override void OnCombatPhaseStart()
    {
        base.OnCombatPhaseStart();
        circle = DrawCircle.Create(owner.transform, owner.transform.position, Quaternion.Euler(90,0,0), range, 0.1f, Color.red);
    }
    public override void OnCombatPhaseEnd()
    {
        base.OnCombatPhaseEnd();
        Destroy(circle);
    }

}
