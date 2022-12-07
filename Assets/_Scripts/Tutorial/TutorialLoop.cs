using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// This script goes on it's own game object to handle high level tutorial stuff
/// </summary>

public class TutorialLoop : MonoBehaviour
{
    [SerializeField]
    private UniverseSimulation universeSimulation;// universe simulation needs to be slotted into this field in the inspector
    private UniverseChronology universeChronology;

    [SerializeField]
    private GameObject HomePlanetUI1;
    [SerializeField]
    private GameObject HomePlanetUI2;
    [SerializeField]
    private GameObject ControlsUI;
    [SerializeField]
    private GameObject ReadyPanelUI;
    [SerializeField]
    private GameObject ResourcesUI;
    [SerializeField]
    private GameObject ComponentsUI;
    [SerializeField]
    private GameObject ShipUI;
    [SerializeField]
    private GameObject TransferUI;
    [SerializeField]
    private GameObject PowerTriangle;
    [SerializeField]
    private GameObject Movement;
    [SerializeField]
    private GameObject TraderConclusion;
    [SerializeField]
    private GameObject RaiderUI;
    [SerializeField]
    private GameObject RaiderUI2;
    [SerializeField]
    private GameObject Damage;
    [SerializeField]
    private GameObject Final;

    private void Start()
    {
        universeChronology = universeSimulation.universeChronology;
        universeChronology.OnSetup.AddListener(() => OnSetup());
        universeChronology.MainPhaseStart.AddListener(() => OnMainPhaseStart());
        universeChronology.CombatPhaseStart.AddListener(() => OnCombatPhaseStart());
    }

    //pick your home system
    public void OnSetup()
    {
        Debug.Log("TUTORIAL: setup phase, turn: " + universeChronology.GetTurnCount());
        HomePlanetUI1.SetActive(true);
    }

    //called at the start of the phase, turn number can be gathered.
    public void OnMainPhaseStart()
    {
        Debug.Log("TUTORIAL: main phase, turn: " + universeChronology.GetTurnCount());
        switch (universeChronology.GetTurnCount())
        {
            case 1:
                Debug.Log("TUTORIAL: It's the first turn!");
                ResourcesUI.SetActive(true);
                break;
            case 2:
                Debug.Log("TUTORIAL: It's the second turn!");
                if(RaiderUI.activeSelf)
                {
                    RaiderUI.SetActive(false);
                }
                TransferUI.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void OnCombatPhaseStart()
    {
        Debug.Log("TUTORIAL: combat phase, turn: " + universeChronology.GetTurnCount());
        switch (universeChronology.GetTurnCount())
        {
            case 1:
                Debug.Log("TUTORIAL: It's the first turn!");
                RaiderUI.SetActive(true);
                break;
            case 2:
                Debug.Log("TUTORIAL: It's the second turn!");
                Damage.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void DeactivateHomePanel()
    {
        if (HomePlanetUI1.activeSelf)
        {
            HomePlanetUI1.SetActive(false);

            HomePlanetUI2.SetActive(true);
        }
    }

    public void DeactivateHomePanel2()
    {
        if (HomePlanetUI2.activeSelf)
        {
            HomePlanetUI2.SetActive(false);

            ControlsUI.SetActive(true);
            ReadyPanelUI.SetActive(true);
        }
    }

    public void DeactivateControls()
    {
        if (ControlsUI.activeSelf && ReadyPanelUI.activeSelf)
        {
            ControlsUI.SetActive(false);
            ReadyPanelUI.SetActive(false);
        }

        else
        {
            return;
        }
    }

    public void DeactivateResourcesUI()
    {
        if (ResourcesUI.activeSelf)
        {
            ResourcesUI.SetActive(false);

            ComponentsUI.SetActive(true);
        }
    }

    public void DeactivateComponentsUI()
    {
        if (ComponentsUI.activeSelf)
        {
            ComponentsUI.SetActive(false);

            ShipUI.SetActive(true);
        }
    }

    public void DeactivateShipsUI()
    {
        if (ShipUI.activeSelf)
        {
            ShipUI.SetActive(false);
        }
    }

    public void DeactivateTransferUI()
    {
        if (TransferUI.activeSelf)
        {
            TransferUI.SetActive(false);
        }
        PowerTriangle.SetActive(true);
    }
    public void DeactivateTriangleUI()
    {
        if (PowerTriangle.activeSelf)
        {
            PowerTriangle.SetActive(false);

            Movement.SetActive(true);
        }
    }

    public void DeactivateMovementUI()
    {
        if (Movement.activeSelf)
        {
            Movement.SetActive(false);

            TraderConclusion.SetActive(true);
        }
    }
    public void DeactivateTraderUI()
    {
        if (TraderConclusion.activeSelf)
        {
            TraderConclusion.SetActive(false);
        }
    }

    public void DeactivateDamageUI()
    {
        if (Damage.activeSelf)
        {
            Damage.SetActive(false);
        }

        Final.SetActive(true);
    }

    public void EndTutorial ()
    {
        if (Final.activeSelf)
        {
            Final.SetActive(false);
        }
    }
}
