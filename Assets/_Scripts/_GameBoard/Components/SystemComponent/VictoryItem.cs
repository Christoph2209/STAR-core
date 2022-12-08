using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// If the player builds this item they win the game
/// </summary>
public class VictoryItem : PawnComponent
{

protected override void AggressiveAction()
{
    // OPTIONAL: Set priority = 1
    PurchaseItem();
}

}
