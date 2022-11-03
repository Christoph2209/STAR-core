using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TransferableComponent : PawnComponent,PlayerControlOverride
{

    public void TransferComponent(Pawn newOwner)
    {
        newOwner.TransferPawnComopnent(gameObject);
    }

    public void TransferComponent()
    {
        universeSimulation.playerFactionCommander.OverrideInput(this);

        
    }


    public virtual void OnMouseHighlight()
    {
        //throw new System.NotImplementedException();
    }

    public virtual void OnMouseMove(InputValue value)
    {
        //throw new System.NotImplementedException();
    }

    public virtual Vector3 OnMove(InputValue value)
    {
        return Vector3.zero;
        //throw new System.NotImplementedException();
    }

    public virtual void OnOpenMenu(InputValue value)
    {
        universeSimulation.playerFactionCommander.RestoreFactionInput();
        //throw new System.NotImplementedException();
    }

    public virtual void OnSelect(InputValue value)
    {
        PlayerFactionCommander input = universeSimulation.playerFactionCommander;
        Pawn targetPawn;
        if (input.isOverUI)
        {
            targetPawn = null;
        }
        else
        {
            targetPawn = input.closestPawnToCursor;
        }

        if(targetPawn == null)
        {
            Debug.Log("Nothing selected, try again");
        }
        else
        {
            Debug.Log(targetPawn + "Selected. Transfering comonent");
            TransferComponent(targetPawn);
            input.RestoreFactionInput();
        }
        
    }

}
