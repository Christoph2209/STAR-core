using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[CreateAssetMenu(menuName = "PawnComponent/DefaultTest")]
public class PawnComponent : MonoBehaviour
{
    Pawn owner;

    public string name; 

    public void EstablishPawnComponent(Pawn owner)
    {
        this.owner = owner;
    }

    public void OnCombatPhaseEnd()
    {

    }

    public void OnCombatPhaseStart()
    {

    }

    public void OnMainPhaseEnd()
    {

    }

    public void OnMainPhaseStart()
    {

    }

}
