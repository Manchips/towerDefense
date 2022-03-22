using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDemo : MonoBehaviour
{
    // todo #1 set up properties
    //   health, speed, coin worth
    //   waypoints
    //   delegate event for outside code to subscribe and be notified of enemy death
    public delegate void EnemyDied(int coins);
    
    public static event EnemyDied addCoins;
    
    public int health = 3;
    public float speed = 3f;
    public int coins = 3;

    public Transform[] wayPointArray;

    public int targetWayPointIndex;
    // NOTE! This code should work for any speed value (large or small)

    //-----------------------------------------------------------------------------
    void Start()
    {
        // todo #2
        //   Place our enemy at the starting waypoint
        transform.position = wayPointArray[0].position;
        targetWayPointIndex = 1;
        StartCoroutine(UpdatePickingRaycast());
    }
    
    IEnumerator UpdatePickingRaycast()
    {
        while (true)
        {
            Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition); //grabs mouse position and uses that for the ray 
            if(Physics.Raycast(mouse,out RaycastHit hitInfo))
            {
                float l = 3;
                Debug.DrawLine(hitInfo.point + Vector3.left * l, hitInfo.point + Vector3.right * l, Color.magenta); //We probably don't need this since its a 2d game
                Debug.DrawLine(hitInfo.point + Vector3.up * l, hitInfo.point + Vector3.down * l, Color.magenta);
                if (Input.GetMouseButton(0))
                {
                    if (hitInfo.collider.gameObject == gameObject)
                    {
                        health--;
                        if (health == 0)
                        {
                            Destroy(gameObject);
                            addCoins?.Invoke(coins);
                        }
                    }
                }
            }
            yield return null; //just gives up 1 frame for the infinite loop 
        }
    }
    
    //-----------------------------------------------------------------------------
    void Update()
    {
        //--Raycasting stuff -------------------------------------------------------
        
        float castDistance = GetComponent<Collider>().bounds.extents.y + 0.1f;
        Ray ray = new Ray(transform.position, Vector3.down); //we want the raycast to go down we don't care if mario jumps on top 

        
        if (Physics.Raycast(ray, out RaycastHit hitInfo, castDistance))
        {
            Debug.DrawRay(transform.position,Vector3.down,Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position,Vector3.down,Color.blue);
        }
        
        // todo #3 Move towards the next waypoint
        Vector3 targetPosition = wayPointArray[targetWayPointIndex].position;
        Vector3 movementDir = (targetPosition - transform.position).normalized;

        Vector3 newPoistion = transform.position;
        newPoistion += movementDir * speed * Time.deltaTime;

        transform.position = newPoistion;
        // todo #4 Check if destination reaches or passed and change target
        Vector3 beforeMovement = (wayPointArray[targetWayPointIndex].position - transform.position).normalized;
        Vector3 afterMovement = (beforeMovement - movementDir).normalized;
        
        if (Vector3.Dot(beforeMovement,afterMovement) > 0)
        {
            transform.position = wayPointArray[targetWayPointIndex].position;
            Debug.Log("On target");
            TargetNextWaypoint();
        }
    }

    //-----------------------------------------------------------------------------
    private void TargetNextWaypoint()
    {
        targetWayPointIndex++;
        transform.LookAt(wayPointArray[targetWayPointIndex]);
        Debug.Log("Targeting " +  wayPointArray[targetWayPointIndex]);
        
    }
}
