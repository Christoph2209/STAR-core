using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingMovement : PawnComponent
{
    public double rangeFactor = 1.0;
    LineRenderer lr;
    GameObject line;

    private void Awake()
        {
        line = new GameObject("line");
        line.transform.SetParent(transform);
        
        lr = line.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        lr.startColor = new Color32(0x4D, 0xBC, 0x68, 0xFF);
        lr.endColor = Color.black;
        lr.startWidth = 0.0f;
        lr.endWidth = 0.1f;
        line.SetActive(false);
    }
    /*
     public void SelectMoveTarget()
        {
            StartCoroutine(SelectPosition());
        }

    IEnumerator SelectPosition()
        {
    
            line.SetActive(true);
            lr.SetPosition(0, target.transform.position);
            lr.SetPosition(1, ship.transform.position);
            
        

            do
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane hPlane = new(Vector3.up, Vector3.zero);
                hPlane.Raycast(ray, out float distance);
                Vector3 targetPosition = ray.GetPoint(distance);
                target.transform.position = ship.SetMoveTarget(targetPosition);
                lr.SetPosition(0, target.transform.position);
                yield return null;

            } while (!Input.GetMouseButtonUp(0));
    }
    Vector3 CheckRange(Vector3 target)
    {
        target.y = 0;
        MoveTarget = Vector3.MoveTowards(transform.position, target, stats);
        return MoveTarget;
    }
    
*/
}
// Movement part of Ship
/*
        IEnumerator Move()
        {
            Vector3 start = transform.position;

            float warpTime = 0.3f;
            float warpTimer = 0f;
            while (warpTimer <= warpTime)
            {

                transform.position = Vector3.Lerp(start, MoveTarget, warpTimer / warpTime);
                warpTimer += Time.deltaTime;
                yield return null;
            }
            transform.position = MoveTarget;
            yield return null;
            MoveTarget = transform.position;
        }
*/

// ShipMainMenu
/*
    public class ShipMainMenu : MonoBehaviour, IPawn
    {
        public Ship ship;
        public GameObject target;
        public GameObject line;
        LineRenderer lr;

        private void Awake()
        {
            
            
            lr = line.AddComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            lr.startColor = new Color32(0x4D, 0xBC, 0x68, 0xFF);
            lr.endColor = Color.black;
            lr.startWidth = 0.0f;
            lr.endWidth = 0.1f;
            line.SetActive(false);
            target.SetActive(false);
        }
        public void SelectMoveTarget()
        {
            StartCoroutine(SelectPosition());
        }
        IEnumerator SelectPosition()
        {
    
            target.SetActive(true);
            line.SetActive(true);
            lr.SetPosition(0, target.transform.position);
            lr.SetPosition(1, ship.transform.position);
            
        

            do
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane hPlane = new(Vector3.up, Vector3.zero);
                hPlane.Raycast(ray, out float distance);
                Vector3 targetPosition = ray.GetPoint(distance);
                target.transform.position = ship.SetMoveTarget(targetPosition);
                lr.SetPosition(0, target.transform.position);
                yield return null;

            } while (!Input.GetMouseButtonUp(0));
            
        }



        private void Start()
        {
            GameStatus.Instance.MainPhaseStart.AddListener(() => OnMainPhaseStart());
            GameStatus.Instance.MainPhaseEnd.AddListener(() => OnMainPhaseEnd());
            GameStatus.Instance.CombatPhaseStart.AddListener(() => OnCombatPhaseStart());
            GameStatus.Instance.CombatPhaseEnd.AddListener(() => OnCombatPhaseEnd());
        }

        public void OnCombatPhaseEnd()
        {
            
        }

        public void OnCombatPhaseStart()
        {

        }

        public void OnMainPhaseEnd()
        {
            Debug.Log("destorying this thing $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
            target.SetActive(false);
            line.SetActive(false);
        }

        public void OnMainPhaseStart()
        {

        }

    }
}
*/