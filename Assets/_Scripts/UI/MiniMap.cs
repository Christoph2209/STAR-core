using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField]
    private GameObject markerPrefab;

    public void UpdateMiniMap(List<Pawn> pawns)
    {

    }
    public void GetExtentsX(List<Pawn> pawns ,out float min , out float max)
    {
        min = pawns[0].transform.position.x;
        max = pawns[0].transform.position.x;
        foreach ( Pawn pawn in pawns)
        {
            float pawnZ = pawn.transform.position.z;
            if (pawnZ > max)
            {
                max = pawnZ;
            }
            if (pawnZ < min)
            {
                min = pawnZ;
            }
        }
    }
    public void GetExtentsZ(List<Pawn> pawns, out float min, out float max)
    {
        min = pawns[0].transform.position.z;
        max = pawns[0].transform.position.z;
        foreach (Pawn pawn in pawns) {
            float pawnZ = pawn.transform.position.z;
            if(pawnZ >max)
            {
                max = pawnZ;
            }
            if (pawnZ < min)
            {
                min = pawnZ;
            }
        }
    }
}
