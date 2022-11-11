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

    private void MovePattern()
    {
        Debug.Log("Thrusters Moving Pawn");
        moveOffset = Vector3.ClampMagnitude(moveOffset, MaxMoveDistance());
        owner.transform.position += moveOffset;
    }

    private float MaxMoveDistance()
    {
        return thrusterMovement * owner.stats[ComponentStat.ThrusterPower];
    }

    public void SelectMoveTarget()
    {
        circleRange = DrawCircle.Create(owner.transform, owner.transform.position, Quaternion.Euler(90, 0, 0), MaxMoveDistance(), 0.1f, Color.green);
        universeSimulation.playerFactionCommander.OverrideInput(this);
        //universeSimulation.playerFactionCommander.MoveCameraToLocation(universeSimulation.transform.position-owner.transform.position);// I need to redo the camera movement

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
}
