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


    private GameObject circleTarget;
    private GameObject circleHighlight;
    private GameObject circleRange;

    public void Attack()
    {
        target.DamagePawn(damage * owner.GetStats(ComponentStat.WeaponPower)* owner.GetStats(ComponentStat.AggregatePower));

        target = null;

        //AUDIO CALL
        AudioManager.Instance.PlayFireSFX();
    }


    public void SelectTarget()
    {
        universeSimulation.playerFactionCommander.OverrideInput(this);
        if(target!= null)
        {
            //if a target has already been selected highlight it
            circleTarget = DrawCircle.Create(target.transform, target.transform.position, Quaternion.Euler(90, 0, 0), 1.5f, 0.03f, Color.red);
        }
    }


   
    public override void OnMouseHighlight()
    {
        PlayerFactionCommander input = universeSimulation.playerFactionCommander;
        if (input.isOverUI)
        {
            return;
        }

        Pawn targetPawn = input.closestPawnToCursor;

        if (circleHighlight != null)
        {
            Destroy(circleHighlight);
        }

        if (targetPawn != null)
        {

            if (Vector3.Distance(targetPawn.transform.position, owner.transform.position) <= range)
            {
                Debug.Log("Drawing Circle");
                circleHighlight = DrawCircle.Create(targetPawn.transform, targetPawn.transform.position, Quaternion.Euler(90, 0, 0), 1.0f, 0.04f, Color.red);
            }
        }
    }

    public void OnMouseMove(InputValue value)
    {
        
    }

    public Vector3 OnMove(InputValue value)
    {
        return Vector3.zero;
    }

    public override void OnOpenMenu(InputValue value)
    {
        PlayerFactionCommander input = universeSimulation.playerFactionCommander;

        ExitAttackMenu(input);
    }

    public override void OnSelect(InputValue value)
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

            //AUDIO CALL
            AudioManager.Instance.PlayTargetSFX();
        }
        ExitAttackMenu(input);
    }

    private void ExitAttackMenu(PlayerFactionCommander input)
    {
        Destroy(circleTarget);
        Destroy(circleHighlight);
        input.RestoreFactionInput();
    }

    public override void OnCombatPhaseStart()
    {
        base.OnCombatPhaseStart();
        circleRange = DrawCircle.Create(owner.transform, owner.transform.position, Quaternion.Euler(90,0,0), range, 0.1f, Color.red);
    }
    public override void OnCombatPhaseEnd()
    {
        base.OnCombatPhaseEnd();
        Destroy(circleRange);
        Destroy(circleTarget);
        Destroy(circleHighlight);
    }


    protected override void AggressiveAction()
    {
        base.AggressiveAction();
        
        Pawn currentTarget = null;
        var UniverseInstance = new UniverseSimulation();

        List<Pawn> possibleTargets = UniverseInstance.GetAllPawnsInRange(owner.transform.position, range); // finds all pawns within range
        foreach (Pawn currentPawn in possibleTargets)
        {
            if(currentPawn.GetFaction() != owner.GetFaction()) // determines if target is an enemy
                {
                if (currentTarget == null || (currentPawn.GetTotalHealth() < currentTarget.GetTotalHealth())) // finds the enemy with lowest health
                    {
                    currentTarget = currentPawn;
                    }
                }
        }
        target = currentTarget;
        currentTarget = null;
        Attack();

    }


}
