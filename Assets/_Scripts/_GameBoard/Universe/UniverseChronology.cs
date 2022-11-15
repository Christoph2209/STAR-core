using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UniverseChronology : MonoBehaviour
{
    UniverseSimulation universeSimulation;
    HashSet<FactionCommander> readiedFactions = new();
    public TurnPhase currentPhase { get; private set; }
    public UnityEvent OnSetup = new();
    public UnityEvent MainPhaseStart = new();
    public UnityEvent MainPhaseEnd= new();
    public UnityEvent CombatPhaseStart = new();
    public UnityEvent CombatPhaseEnd = new();

    private FactionCommander winner;
    private int turnCount = 0 ;
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
        


        //Game Setup
        readiedFactions.Clear();
        currentPhase = global::TurnPhase.SelectHomeSystem;
        OnSetup.Invoke();
        yield return new WaitUntil(() => readiedFactions.SetEquals( new HashSet<FactionCommander>(universeSimulation.factionsInPlay)));//set equals checks if the sets are equal, it does nto set them to equivilant values lol
        //Begin Game
        Debug.Log("Is It Working?");


        do
        {
            Debug.Log("NO ONE HAS ONE! On to the next round");
            turnCount++;

            //Trasition to main
            currentPhase = global::TurnPhase.TransitionToTrader;
            yield return new WaitForSeconds(transitionTime);
            readiedFactions.Clear();
            //----

            //Main 
            currentPhase = global::TurnPhase.TraderPhase;
            MainPhaseStart.Invoke();
            yield return new WaitUntil(() => (readiedFactions.SetEquals(universeSimulation.factionsInPlay)));//set equals checks if the sets are equal, it does nto set them to equivilant values lol
            MainPhaseEnd.Invoke();
            //----

            if (CheckVictoryCondition())
            {
                break;
            }

            //Transition to combat
            currentPhase = global::TurnPhase.TransitionToRaider;
            yield return new WaitForSeconds(transitionTime);
            readiedFactions.Clear();
            //----

            //Combat
            currentPhase = global::TurnPhase.RaiderPhase;
            CombatPhaseStart.Invoke();
            yield return new WaitUntil(() => (readiedFactions.SetEquals(universeSimulation.factionsInPlay)));//set equals checks if the sets are equal, it does nto set them to equivilant values lol
            CombatPhaseEnd.Invoke();
            //----

            Debug.Log("***Round Complete***");


        } while (!CheckVictoryCondition());

        currentPhase = global::TurnPhase.GameOver;
        
        
        if (winner == null)
        {
            GameOverMenuGroup.winner = "";
            Debug.Log("Everyone Looses");
        }
        else
        {
            GameOverMenuGroup.winner = winner.factionName;
            Debug.Log(winner.factionName + " IS THE WINNER WOOOOOOOOOOOOOH.");
        }
        SceneManager.LoadScene("GameOverMenu");
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
            Debug.Log(factionCommander  +  "  has already completeed their turn");
            return false;
        }
    }
    public bool IsFactionReady(FactionCommander factionCommander)
    {
        return readiedFactions.Contains(factionCommander);
    }
    public int GetTurnCount()
    {
        return turnCount;
    }
}

public enum TurnPhase { SelectHomeSystem, TransitionToTrader, TraderPhase, TransitionToRaider, RaiderPhase, GameOver, None, Every };