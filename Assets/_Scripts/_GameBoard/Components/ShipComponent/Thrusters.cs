using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class Thrusters : PawnComponent, PlayerControlOverride
{
    Vector3 moveOffset = Vector3.forward;
    public float thrusterMovement=5;


    private GameObject circleRange;
    private GameObject circleTarget;

    private float moveTime = 0.2f;
    
    private void MovePattern()
    {
        Debug.Log("Thrusters Moving Pawn");
        moveOffset = Vector3.ClampMagnitude(moveOffset, MaxMoveDistance());
        StaticCoroutine.Start(MoveProcess(moveOffset));
        //owner.transform.position += moveOffset;
    }
    IEnumerator MoveProcess(Vector3 moveOffset)
    {
        AudioManager.Instance.PlayMoveActionSFX();

        float t = 0;
        Vector3 currentVelocity = Vector3.zero;
        Vector3 currentOffset = Vector3.zero;
        Vector3 previousOffset = Vector3.zero;
        while (t <= moveTime)
        {
            t += Time.deltaTime;

            currentOffset = Vector3.Lerp(Vector3.zero, moveOffset, t / moveTime);
            owner.transform.position += currentOffset - previousOffset;
            previousOffset = currentOffset;
            yield return null;
        }

    }
    private float MaxMoveDistance()
    {
        return thrusterMovement * owner.stats[ComponentStat.ThrusterPower] * owner.stats[ComponentStat.AggregatePower];
    }

    public void SelectMoveTarget()
    {
        circleRange = DrawCircle.Create(owner.transform, owner.transform.position, Quaternion.Euler(90, 0, 0), MaxMoveDistance(), 0.1f, Color.green);
        universeSimulation.playerFactionCommander.OverrideInput(this);
        //universeSimulation.playerFactionCommander.MoveCameraToLocation(universeSimulation.transform.position-owner.transform.position);// I need to redo the camera movement

        //AUDIO CALL
        AudioManager.Instance.PlayMoveTargetSFX();
    }

    public Vector3 OnMove(InputValue value)
    {
        return Vector3.zero;
    }

    public void OnMouseMove(InputValue value)//terrible code that could be optimized but probabaly won't need to be. Just draws a circle at the mouse location clamped to a valid position.
    {
        Destroy(circleTarget);
        circleTarget = DrawCircle.Create(owner.transform, owner.transform.position + Vector3.ClampMagnitude(universeSimulation.playerFactionCommander.MouseWorldPoint()-owner.transform.position, MaxMoveDistance()), Quaternion.Euler(90, 0, 0), 0.7f, 0.03f, Color.green);
    }

    public void OnSelect(InputValue value)
    {
        PlayerFactionCommander input = universeSimulation.playerFactionCommander;

        if (input.isOverUI)
        {
            //No move Target Selected. do nothing and wait for further input.
        }
        else
        {
            Vector3 moveTarget = universeSimulation.playerFactionCommander.MouseWorldPoint();

            moveOffset = moveTarget - owner.transform.position;


            Debug.Log(owner + "Moving to location " + moveTarget);
            owner.SetMovePattern(() => MovePattern());
            ExitMoveMenu(input);
        }
    }

    private void ExitMoveMenu(PlayerFactionCommander input)
    {
        Destroy(circleRange);
        Destroy(circleTarget);
        input.RestoreFactionInput();
    }

    public void OnMouseHighlight()
    {
        //throw new NotImplementedException();
    }

    public void OnOpenMenu(InputValue value)
    {
        //throw new NotImplementedException();
    }


    protected override void AggressiveAction() // Finds the closest enemy pawn and approaches it
    {
        base.AggressiveAction();

        float distance = float.PositiveInfinity;
        Pawn closestEnemy = null;
        Vector3 targetPosition = owner.transform.position;
        var UniverseInstance = new UniverseSimulation();

        foreach (Pawn pawn in UniverseInstance.GetAllPawns()) // edited from UniverseSimulation.GetClosestPawnToPosition() 
        {
            if (pawn.GetFaction() != owner.GetFaction()) // Determines if current pawn is enemy
            {
                if (closestEnemy == null || Vector3.Distance(targetPosition, closestEnemy.transform.position) > Vector3.Distance(targetPosition, pawn.transform.position))
                {
                    closestEnemy = pawn;
                    distance = Vector3.Distance(targetPosition, pawn.transform.position);
                }
            }
        }

        if (closestEnemy != null)
        {
            /// move to closest enemy
            Vector3 moveTarget = closestEnemy.transform.position;

            moveOffset = moveTarget - owner.transform.position;

            Debug.Log(owner + "Moving to location " + moveTarget);
            owner.SetMovePattern(() => MovePattern());
        }

    }


}
