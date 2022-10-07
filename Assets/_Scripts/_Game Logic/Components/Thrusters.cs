using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Thrusters : PawnComponent
{
    Vector3 moveTarget = Vector3.forward;

    private void MovePattern()
    {
        Debug.Log("Thrusters Moving Pawn");
        owner.transform.position += moveTarget;
    }
    public void SelectMoveTarget()
    {
        owner.SetMovePattern(() => MovePattern());
    }

}
