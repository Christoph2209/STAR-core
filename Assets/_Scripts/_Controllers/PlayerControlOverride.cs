using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface PlayerControlOverride 
{
    public Vector3 OnMove(InputValue value);
    public void OnMouseMove(InputValue value);
    public void OnSelect(InputValue value);
    public void OnMouseHighlight();
    public void OnOpenMenu(InputValue value);
}
