using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
public class UniverseSimulation : MonoBehaviour
{
    public GameObject PlayerFactionCommander;
    public GameObject NPCFactionCommander;
    public GameObject Ship;
    public GameObject SolarSystem;

    [SerializeField]
    private List<Pawn> pawns = new();
    public List<FactionCommander> factionsInPlay = new();
    public UniverseChronology universeChronology;
    
    public void GeneratePawn(GameObject pawnPrefab, FactionCommander Owner, string pawnName) 
    {
        GameObject pawnGameObject = Instantiate(pawnPrefab, transform);
        Pawn newPawn = pawnGameObject.GetComponent<Pawn>();
        newPawn.EstablishPawn(pawnName ,this, Owner);
        pawns.Add(newPawn);
    }

    public bool EstablishFaction(string name, GameObject factionCommanderPrefab)
    {
        bool isNewFaction = true;
        foreach(FactionCommander factionCommander in factionsInPlay)
        {
            if(factionCommander.name == name)
            {
                isNewFaction = false;
            }
        }
         
        if (isNewFaction)
        {
            GameObject faction = Instantiate(factionCommanderPrefab,transform);
            faction.GetComponent<FactionCommander>().EstablishFaction(name, this);
            factionsInPlay.Add(faction.GetComponent<FactionCommander>());
            return true;
        }
        else 
        {
            return false;
        }
    }

    public void EstablishUniverseChronolgoy()
    {
        if (universeChronology != null)
        {
            Debug.LogError("Creating new chronology for this universe while one already exists! Multiple timelines are not supported!");
        }
        GameObject chronology = new("UniverseChronology");
        universeChronology = chronology.AddComponent<UniverseChronology>();
        chronology.GetComponent<UniverseChronology>().EstablishUniverseChronology(this);
        
    }
    
    



    public Pawn GetClosestPawnToPosition(Vector3 targetPosition, out float distance)
    {
        distance = float.PositiveInfinity;
        Pawn closest = null;
        foreach(Pawn pawn in pawns)
        {
            if(closest==null || Vector3.Distance(targetPosition, closest.transform.position) > Vector3.Distance(targetPosition, pawn.transform.position))
            {
                closest = pawn;
                distance = Vector3.Distance(targetPosition, pawn.transform.position);
            }
        }
        return closest;
    }
    public Pawn GetClosestPawnInRange(Vector3 targetPosition, float range, out float distance)
    {
        Pawn closest = GetClosestPawnToPosition(targetPosition, out distance);
        if (distance <= range)
        {
            return closest;
        }
        else
        {
            return null;
        }
    }

    public List<Pawn> GetAllPawnsInRange(Vector3 targetPosition, float range)
    {
        List<Pawn> pawnInRange = new();
        foreach (Pawn pawn in pawns)
        {
            if (range > Vector3.Distance(targetPosition, pawn.transform.position))
            {
                pawnInRange.Add(pawn);
            }
        }
        return pawnInRange;
    }

    public List<Pawn> GetAllFactionPawns(FactionCommander faction)
    {
        List<Pawn> pawnInRange = new();
        foreach (Pawn pawn in pawns)
        {
            if (pawn.GetFaction() == faction)
            {
                pawnInRange.Add(pawn);
            }
        }
        return pawnInRange;
    }










    
    void Start()
    {

        //initialize all of the factions and turnphase tracking
        EstablishUniverseChronolgoy();
        //EstablishFaction("PLAYER FACTION",PlayerFactionCommander);
       // EstablishFaction("OTHER FACTION", NPCFactionCommander);
        GeneratePawn(Ship,factionsInPlay.First(), "TEST PAWN");
        
    }


}
