using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;

public class UniverseSimulation : MonoBehaviour
{
    public static int playerNum = 4;
    public SaveObject so;
    
    [SerializeField]
    public List<Pawn> pawns = new();
    public List<FactionCommander> factionsInPlay = new();
    public PlayerFactionCommander playerFactionCommander;
    [HideInInspector]
    public UniverseChronology universeChronology;


    //Move to a class that controls the state of the game
    [Header("Testing:")]
    public GameObject PlayerFactionCommander;
    public GameObject NPCFactionCommander;
    public GameObject Ship;
    public GameObject SolarSystem;
    //----------------------------------------------------



    public GameObject GeneratePawn(GameObject pawnPrefab, FactionCommander faction, string pawnName, Vector3 position) 
    {
        
        GameObject pawnGameObject = Instantiate(pawnPrefab, transform);
        Pawn newPawn = pawnGameObject.GetComponent<Pawn>();
        newPawn.transform.position = position;
        pawns.Add(newPawn);
        SaveManager.Save(so);
        newPawn.EstablishPawn(pawnName, this, faction);
        return pawnGameObject;
    }

    public GameObject LoadExistingPawn(GameObject pawnPrefab, GameObject paw, Vector3 locate, FactionCommander faction, string names)
    {
        paw = Instantiate(pawnPrefab, transform);
        Pawn newPawn = paw.GetComponent<Pawn>();
        newPawn.transform.position = locate;
        pawns.Add(newPawn);
        newPawn.EstablishPawn(names, this, faction);
        return paw;
    }

    public void DestroyPawn(Pawn pawn)
    {
        pawns.Remove(pawn);
        Destroy(pawn.gameObject);
    }

    public FactionCommander EstablishFaction(string name, GameObject factionCommanderPrefab)
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
            FactionCommander factionCommander = faction.GetComponent<FactionCommander>();
            factionsInPlay.Add(factionCommander);
            if (factionCommander.GetType().IsAssignableFrom(typeof(PlayerFactionCommander)))
            {
                Debug.LogWarning("FOUND THE PLAYER!!!");
                playerFactionCommander = (PlayerFactionCommander)factionCommander; 
            }
            return faction.GetComponent<FactionCommander>();
        }
        else 
        {
            return null;
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
    
    


    public List<Pawn> GetAllPawns()
    {
        return new(pawns);
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

    public List<Pawn> GetAllPawnsInRange(FactionCommander faction, Vector3 targetPosition, float range)
    {
        List<Pawn> pawnInRange = new();
        foreach (Pawn pawn in pawns)
        {
            if (range > Vector3.Distance(targetPosition, pawn.transform.position) && pawn.GetFaction() == faction)
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






    private void Awake()
    {
        EstablishUniverseChronolgoy();
    }


    void Start()
    {
        //Initialize all factions
        //EstablishFaction("PLAYER FACTION",PlayerFactionCommander);

        so = SaveManager.Load();

        EstablishFaction("Player 2", NPCFactionCommander);

        for (int i = 2; i < playerNum; i++)
        {
            EstablishFaction("Player "+ i+1, NPCFactionCommander);
        }
        //GameObject playerPawn = GeneratePawn(Ship,factionsInPlay.First(), "TEST PAWN", Vector3.zero);
        //GeneratePawn(Ship, factionsInPlay[1], "OTHER PAWN", Vector3.right*3);

        //Cost test1;
        //Cost test2;
        //test1.type = ComponentResource.Rare;
        //test1.value = 100;


        //CargoHold.AddResources(playerPawn.GetComponent<Pawn>(), ComponentResource.Rare, 20);
        //Debug.Log(CargoHold.TryRemoveResources(new List<Pawn> { playerPawn.GetComponent<Pawn>() }, new List<Cost> { test1 })+ "This should say false");
        //Debug.Log(CargoHold.GetTotalResources(new List<Pawn> { playerPawn.GetComponent<Pawn>() },ComponentResource.Rare)+ "Rare Resources available");
        //Debug.Log(CargoHold.GetTotalResources(new List<Pawn> { playerPawn.GetComponent<Pawn>() }, ComponentResource.Medium) + "Medium Resources available");
        //Debug.Log(CargoHold.GetTotalResources(new List<Pawn> { playerPawn.GetComponent<Pawn>() }, ComponentResource.WellDone) + "WellDone Resources available");


    }


}
