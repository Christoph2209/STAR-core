using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : PawnComponent
{
    // Start is called before the first frame update
    void Start()
    {
        private float armorLevel = 2.0;
        private float armorHealth = 10 * armorLevel;

        owner.MaxHealth += armorHealth; 
    }


}
