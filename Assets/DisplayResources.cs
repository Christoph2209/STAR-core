using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisplayResources : MonoBehaviour
{
    [SerializeField]
    PlayerFactionCommander pfc;
    [SerializeField]
    TMP_Text resource1;
    [SerializeField]
    TMP_Text resource2;
    [SerializeField]
    TMP_Text resource3;


    void Update()
    {
        List<Pawn> allActingPawns = pfc.universeSimulation.GetAllFactionPawns(pfc.GetActingFaction());
        resource1.text = "" + CargoHold.GetTotalResources(allActingPawns, ComponentResource.Rare);
        resource2.text = "" + CargoHold.GetTotalResources(allActingPawns, ComponentResource.Medium);
        resource3.text = "" + CargoHold.GetTotalResources(allActingPawns, ComponentResource.WellDone);
    }
}
