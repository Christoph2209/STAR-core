using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using System;


public class PlayerFactionCommander : FactionCommander 
{
    [SerializeField]
    TMP_Text PhaseInfo;
    
    float selectionDistance = 1.5f;
    Vector3 moveDirection = Vector3.zero;
    float camSpeed = 10f;

    public bool isOverUI { get; private set; }
    public Pawn closestPawnToCursor { get; private set; }

    GameObject[] sounds;
    private void Update()
    {

        PhaseInfo.text = actingFaction.factionName + ": " + universeSimulation.universeChronology.currentPhase.ToString() + ", Ready:" + universeSimulation.universeChronology.IsFactionReady(actingFaction);


        //TODO Display Pawn Stats
       
        universeSimulation.transform.position -= camSpeed * Time.deltaTime * moveDirection;
        isOverUI = EventSystem.current.IsPointerOverGameObject();//works as intended, ignore warning
        closestPawnToCursor = universeSimulation.GetClosestPawnInRange(MouseWorldPoint(), selectionDistance, out _);
        ProcessMouseHighlight();


    }


    Coroutine moveToLocation;
    private void MoveToLocation(Vector3 targetLocation)
    {
        if(moveToLocation!= null)
        {
            StopCoroutine(moveToLocation);
        }
        moveToLocation = StartCoroutine(CMoveToLocation(targetLocation));
    }
    IEnumerator CMoveToLocation(Vector3 targetLocation)
    {
        float maxDistance = 0.1f;
        float speed = camSpeed;
        while (( targetLocation-universeSimulation.transform.position ).magnitude > maxDistance)
        {
            yield return null;
            Vector3 maxMove = targetLocation - universeSimulation.transform.position;
            Vector3 move = speed * Time.deltaTime * maxMove.normalized;
            move = Vector3.ClampMagnitude(move, maxMove.magnitude);
            speed += Time.deltaTime * 200;
            universeSimulation.transform.position += move;
        }
        
    }


    
    public void OnMove(InputValue value)
    {
        if (playerControlOverride != null)
        {
            moveDirection = playerControlOverride.OnMove(value);
            return;
        }
        moveDirection = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
        Debug.Log(moveDirection);
    }



    Vector2 mouseScreenPoint = new(0.5f, 0.5f);
    
    public void OnMouseMove(InputValue value)
    {
        mouseScreenPoint = value.Get<Vector2>();
        if (playerControlOverride != null)
        {
            playerControlOverride.OnMouseMove(value);
            return;
        }
    }


  
    public Vector3 MouseWorldPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPoint);
        Plane hPlane = new(Vector3.up, Vector3.zero);
        hPlane.Raycast(ray, out float distance);
        Vector3 mouseWorldPoint = ray.GetPoint(distance);
        return mouseWorldPoint;
    }
    public Vector3 ScreenCenterWorldPoint()
    {
  
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(0.5f* Camera.main.pixelWidth,0.5f*Camera.main.pixelHeight));
        Plane hPlane = new(Vector3.up, Vector3.zero);
        hPlane.Raycast(ray, out float distance);
        Vector3 screenCenterWorldPoint = ray.GetPoint(distance);
        return screenCenterWorldPoint;
    }





    Vector3 startSelectPoint;
    bool wasOverUI;
    
    public void OnSelect(InputValue value)
    {
        if (playerControlOverride != null)
        {
            playerControlOverride.OnSelect(value);
            return;
        }

        float minDragDistance=0.5f;
        Vector3 currentSelectPoint = MouseWorldPoint();
      
        if (value.isPressed)
        {
            startSelectPoint = currentSelectPoint;
            wasOverUI = isOverUI;
        }
        else if (Vector3.Distance(startSelectPoint, currentSelectPoint) <= minDragDistance)//select the single closest Pawn
        {
            if (!wasOverUI)
            {
                Vector3 moveOffset;//select location to move to
                if (closestPawnToCursor != null)
                {
                    moveOffset = closestPawnToCursor.transform.position - ScreenCenterWorldPoint();
                    closestPawnToCursor.DefaultAction(actingFaction);
                }
                else
                {
                    moveOffset = startSelectPoint - ScreenCenterWorldPoint();
                }
                //move selected location to center of screen
                Vector3 targetLocation = universeSimulation.transform.position - moveOffset;
                MoveToLocation(targetLocation);
                //if the selcted target is a pawn activate the deafault action

            }
        }
        else if(!wasOverUI)
        {
            //TODO DragToSelect
            Debug.Log("Implement rectangular select");
        }
    }

    Pawn lastStatMenuOpened;
    
    private void ProcessMouseHighlight()
    {
        if (playerControlOverride != null)
        {
            playerControlOverride.OnMouseHighlight();
            return;
        }

        if (lastStatMenuOpened != closestPawnToCursor)
        {
            if (lastStatMenuOpened != null)
            {
                lastStatMenuOpened.CloseStatMenu();
                lastStatMenuOpened = null;
            }
            if (closestPawnToCursor != null)
            {
                closestPawnToCursor.OpenStatMenu(actingFaction);
                lastStatMenuOpened = closestPawnToCursor;
            }
        }
    }

    Pawn lastComponentMenuOpened;
    
    public void OnOpenMenu(InputValue value)
    {
        if (playerControlOverride!=null)
        {
            playerControlOverride.OnOpenMenu(value);
            return;
        }

        Pawn targetPawn;
        if (isOverUI||closestPawnToCursor == lastComponentMenuOpened)
        {
            targetPawn = null;
        }
        else
        {
            targetPawn = closestPawnToCursor;
        }
        //---
        if (lastComponentMenuOpened != null )
        {
            lastComponentMenuOpened.CloseComponentMenu();
            lastComponentMenuOpened = null;
        }
        //---
        if (targetPawn != null)
        {
            targetPawn.OpenComponentMenu(actingFaction);
            lastComponentMenuOpened = targetPawn;
        }
        else
        {
            Debug.Log("No menu to open/open command menu?");
            lastComponentMenuOpened = null;
        }

        
    }


    public void OnTest(InputValue value)
    {
        List<Pawn> AllPawns = universeSimulation.GetAllPawns();
        foreach(Pawn pawn in AllPawns)
        {
            pawn.CloseStatMenu();
            pawn.CloseComponentMenu();
        }
        int index = universeSimulation.factionsInPlay.IndexOf(actingFaction);
        index++;
        index %= universeSimulation.factionsInPlay.Count;
        actingFaction = universeSimulation.factionsInPlay[index];

        Debug.Log(actingFaction);
    }
    public void OnExit(InputValue value)
    {
        Application.Quit();
        Debug.Log("Exiting Game");
    }

    PlayerControlOverride playerControlOverride;
    bool isOverride = false;
    public void OverrideInput(PlayerControlOverride playerControlOverride)
    {
        this.playerControlOverride = playerControlOverride;

        isOverride = true;
        lastComponentMenuOpened.CloseComponentMenu();
        if (lastStatMenuOpened != null)
        {
            lastStatMenuOpened.CloseStatMenu();
            lastStatMenuOpened = null;// by setting last stat menu opened to null it will reset itself once control resumes
        }
    }

    public void RestoreFactionInput()
    {
        playerControlOverride = null;
        isOverride = false;
        if (lastComponentMenuOpened != null)
        {
            lastComponentMenuOpened.OpenComponentMenu(actingFaction);
        }
    }

}