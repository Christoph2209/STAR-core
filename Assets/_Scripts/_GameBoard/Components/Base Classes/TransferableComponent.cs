using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TransferableComponent : PawnComponent,PlayerControlOverride
{
    private float transferRange = 2.5f;
    private GameObject circleHighlight;
    private GameObject circleRange;

    public void TransferComponent(Pawn newOwner)
    {
        newOwner.TransferPawnComopnent(gameObject);

        //AUDIO CALL
        AudioManager.Instance.PlayTransferSFX();
    }

    public void TransferComponent()
    {
        universeSimulation.playerFactionCommander.OverrideInput(this);
        circleRange = DrawCircle.Create(owner.transform, owner.transform.position, Quaternion.Euler(90, 0, 0), transferRange - 0.25f, 0.06f, Color.white);
    }


    public virtual void OnMouseHighlight()
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

            if (Vector3.Distance(targetPawn.transform.position, owner.transform.position) <= transferRange)
            {
                Debug.Log("Drawing Circle");
                circleHighlight = DrawCircle.Create(targetPawn.transform, targetPawn.transform.position, Quaternion.Euler(90, 0, 0), 1.0f, 0.03f, Color.white);
            }
        }
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
            
        }
        OnTransferMenuExit(input);
        
    }
    private void OnTransferMenuExit(PlayerFactionCommander input)
    {
        input.RestoreFactionInput();
        Destroy(circleHighlight);
        Destroy(circleRange);
    }

}
