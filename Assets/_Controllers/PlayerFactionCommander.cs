using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class PlayerFactionCommander : FactionCommander 
{
    float selectionDistance = 1.5f;
    Vector3 moveDirection = Vector3.zero;
    float camSpeed = 10f;
    FactionCommander actingFaction;
    private void Start()
    {
        actingFaction = this;
    }
    private void Update()
    {
        //TODO Display Pawn Stats
        universeSimulation.transform.position -= moveDirection*camSpeed*Time.deltaTime;
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
            Vector3 move = maxMove.normalized * Time.deltaTime * speed;
            move = Vector3.ClampMagnitude(move, maxMove.magnitude);
            speed += Time.deltaTime * 200;
            universeSimulation.transform.position += move;
        }
        
    }



    public void OnMove(InputValue value)
    {
        moveDirection = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
    }



    Vector2 mouseScreenPoint = new Vector2(0.5f, 0.5f);
    public void OnMouseMove(InputValue value)
    {
        mouseScreenPoint = value.Get<Vector2>();
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
    public void OnSelect(InputValue value)
    {
        float minDragDistance=0.5f;
        Vector3 currentSelectPoint = MouseWorldPoint();
        //DragToSelect
        if (value.isPressed)
        {
            startSelectPoint = currentSelectPoint;
        }
        else if(Vector3.Distance(startSelectPoint, currentSelectPoint)<=minDragDistance)//select the single closest Pawn
        {
            bool isOverUI = EventSystem.current.IsPointerOverGameObject();//works as intended, ignore warning
            if (!isOverUI)
            {

                Pawn closest;
                closest = universeSimulation.ClosestPawnInRange(startSelectPoint, selectionDistance, out float distance);


                Vector3 moveOffset;//select location to move to
                if (closest != null)
                {
                    moveOffset = closest.transform.position - ScreenCenterWorldPoint();
                    closest.DefaultAction(actingFaction);
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
        else
        {
            Debug.Log("Implement rectangular select");
        }
    }


    Pawn lastMenuOpened;
    public void OnOpenMenu(InputValue value)
    {
        bool isOverUI = EventSystem.current.IsPointerOverGameObject();//=works as intended, ignore warning
        if (!isOverUI)
        {
            if (lastMenuOpened != null)
            {
                lastMenuOpened.CloseMenu();
            }
            
            Pawn closest;
            closest = universeSimulation.ClosestPawnInRange(MouseWorldPoint(), selectionDistance, out float distance);
            if (closest != null)
            {
                closest.OpenMenu(actingFaction);
                lastMenuOpened = closest;
            }
            else
            {
                Debug.Log("No menu to open/open command menu?");
            }

        }
    }


    public void OnTest(InputValue value)
    {
        int index = universeSimulation.factionsInPlay.IndexOf(actingFaction);
        index++;
        index %= universeSimulation.factionsInPlay.Count;
        actingFaction = universeSimulation.factionsInPlay[index];

        Debug.Log(actingFaction);
    }






    



    public void ActivatePawnMenu(Pawn pawn)// right click 
    {
        Debug.Log("Activating the pawns action menu.");
    }
    public void ActivatePawnInfo(Pawn pawn)  //hover
    {
        Debug.Log("Activating the pawns stat menu.");
    }
    public void SelectShipPrimaryAction(Ship ship)//Left Click. Main: move, Combat: attack
    {
        Debug.Log("Activating the ships primary action");
    }
    public void SelectShipPrimaryAction(List<Ship> ship)//left click and drag
    {
        Debug.Log("Activating a bunch of ships primary action");
    }


}
