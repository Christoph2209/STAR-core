using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class Thrusters : PawnComponent, PlayerControlOverride
{
    Vector3 moveOffset = Vector3.forward;
    public float thrusterMovement=5;
    private void MovePattern()
    {
        Debug.Log("Thrusters Moving Pawn");
        moveOffset =  Vector3.ClampMagnitude(moveOffset, thrusterMovement * owner.stats[ComponentStat.ThrusterPower]);
        owner.transform.position += moveOffset;
    }
    public void SelectMoveTarget()
    {
        universeSimulation.playerFactionCommander.OverrideInput(this);
        //universeSimulation.playerFactionCommander.MoveCameraToLocation(universeSimulation.transform.position-owner.transform.position);// I need to redo the camera movement

    }

    public Vector3 OnMove(InputValue value)
    {
        return Vector3.zero;
    }

    public void OnMouseMove(InputValue value)
    {
        //throw new NotImplementedException();
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
            

            Debug.Log(owner + "Moving to location " + moveTarget );
            owner.SetMovePattern(() => MovePattern());
            input.RestoreFactionInput();
        }
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
