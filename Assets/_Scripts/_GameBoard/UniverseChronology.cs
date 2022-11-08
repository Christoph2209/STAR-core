using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UniverseChronology : MonoBehaviour
{
    UniverseSimulation universeSimulation;
    HashSet<FactionCommander> readiedFactions = new();
    public TurnPhase currentPhase { get; private set; }
    public UnityEvent MainPhaseStart = new();
    public UnityEvent MainPhaseEnd= new();
    public UnityEvent CombatPhaseStart = new();
    public UnityEvent CombatPhaseEnd = new();

    private FactionCommander winner;
    //Required for initialization. If this method doesn't get called it won't funtion properly
    public void EstablishUniverseChronology(UniverseSimulation universeSimulation)
    {
        if (this.universeSimulation != null)
        {
            Debug.LogError("The UniverseSimulation should never Change! This method should only be called durring initialization");
        }
        else
        {
            this.universeSimulation = universeSimulation;
            StartCoroutine(TurnPhase());
        }
    }


    IEnumerator TurnPhase()
    {
        float transitionTime=0.2f;
        int currentRound = 0;


        //Game Setup
        readiedFactions.Clear();
        currentPhase = global::TurnPhase.SelectHomeSystem;
        
        yield return new WaitUntil(() => readiedFactions.SetEquals( new HashSet<FactionCommander>(universeSimulation.factionsInPlay)));//set equals checks if the sets are equal, it does nto set them to equivilant values lol
        //Begin Game
        Debug.Log("Is It Working?");


        do
        {
            Debug.Log("NO ONE HAS ONE! On to the next round");
            currentRound++;

            //Trasition to main
            currentPhase = global::TurnPhase.TransitionToMain;
            yield return new WaitForSeconds(transitionTime);
            readiedFactions.Clear();
            //----

            //Main 
            currentPhase = global::TurnPhase.Main;
            MainPhaseStart.Invoke();
            yield return new WaitUntil(() => (readiedFactions.SetEquals(universeSimulation.factionsInPlay)));//set equals checks if the sets are equal, it does nto set them to equivilant values lol
            MainPhaseEnd.Invoke();
            //----

            //Transition to combat
            currentPhase = global::TurnPhase.TransitionToCombat;
            yield return new WaitForSeconds(transitionTime);
            readiedFactions.Clear();
            //----

            //Combat
            currentPhase = global::TurnPhase.Combat;
            CombatPhaseStart.Invoke();
            yield return new WaitUntil(() => (readiedFactions.SetEquals(universeSimulation.factionsInPlay)));//set equals checks if the sets are equal, it does nto set them to equivilant values lol
            CombatPhaseEnd.Invoke();
            //----

            Debug.Log("***Round Complete***");


        } while (!CheckVictoryCondition());

        currentPhase = global::TurnPhase.GameOver;
        if(winner == null)
        {
            Debug.Log("Everyone Looses");
        }
        else
        {
            Debug.Log(winner.factionName + " IS THE WINNER WOOOOOOOOOOOOOH.");
        }
    }

    private bool CheckVictoryCondition()
    {
        bool victoryAchieved = false;
        List<FactionCommander> homeSystems = new();
        foreach(Pawn pawn in universeSimulation.GetAllPawns())
        {
            //Trader Victory
            //Check For VictoryItem Win Condition
            if(pawn.GetPawnComponents<VictoryItem>().Count>0)
            {
                Debug.Log("GAME OVER: Victor item aquiered");
                winner = pawn.GetFaction();
                victoryAchieved = true;
            }

            //Raider Victory
            //Check to see if there are any home systems left
            if (pawn.GetPawnComponents<HomeSystem>().Count > 0)
            {
                homeSystems.Add(pawn.GetFaction());
            }
        }
        if(homeSystems.Count == 1)
        {
            Debug.Log("GAME OVER: Last Home System Standing");
            winner = homeSystems[0];
            victoryAchieved = true;
        }
        else if(homeSystems.Count == 0)
        {
            Debug.Log("GAME OVER: NO Player Wins, Mutual Destruction");
            winner = null;
            victoryAchieved = true;
        }

        return victoryAchieved;
    }




    public bool MarkFactionReady(FactionCommander factionCommander)
    {
        if (readiedFactions.Add(factionCommander))
        {
            Debug.Log(factionCommander + " has completed the " + currentPhase + " phase.");
            Debug.Log(readiedFactions.SetEquals(universeSimulation.factionsInPlay));
            Debug.Log(universeSimulation.factionsInPlay + "       " + readiedFactions);
            return true;
        }
        else
        {
            readiedFactions.Remove(factionCommander);
            Debug.Log(factionCommander + " is no longer ready to complete" + currentPhase + " phase!" + "\n" + "Canceling ready Status!");
            Debug.Log(readiedFactions.SetEquals(universeSimulation.factionsInPlay));
            return false;
        }
    }
    public bool IsFactionReady(FactionCommander factionCommander)
    {
        return readiedFactions.Contains(factionCommander);
    }
}

public enum TurnPhase { SelectHomeSystem, TransitionToMain, Main, TransitionToCombat, Combat, GameOver, None};