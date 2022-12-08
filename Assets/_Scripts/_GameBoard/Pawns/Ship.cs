using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Pawn
{


protected override void AggressiveAction()
{
    // OPTIONAL: Set priority = 2
    PurchaseItem();
}


}
