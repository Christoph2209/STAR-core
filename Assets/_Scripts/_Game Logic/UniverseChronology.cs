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



        while (true)
        {
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
            

        }
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

public enum TurnPhase { TransitionToMain, Main, TransitionToCombat, Combat };