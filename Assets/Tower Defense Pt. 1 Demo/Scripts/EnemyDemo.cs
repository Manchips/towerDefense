using System;
using UnityEngine;

public class EnemyDemo : MonoBehaviour
{
    // todo #1 set up properties
    //   health, speed, coin worth
    //   waypoints
    //   delegate event for outside code to subscribe and be notified of enemy death
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
    }

    //-----------------------------------------------------------------------------
    void Update()
    {
        // todo #3 Move towards the next waypoint
        Vector3 targetPosition = wayPointArray[targetWayPointIndex].position;
        Vector3 movementDir = (targetPosition - transform.position).normalized;

        Vector3 newPoistion = transform.position;
        newPoistion += movementDir * speed * Time.deltaTime;

        transform.position = newPoistion;
        // todo #4 Check if destination reaches or passed and change target

        if (Vector3.Dot(targetPosition - transform.position,transform.position) > 0)
        {
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
        
        //Clamping with mathf resulted in the enemy getting lost past waypoint 3 we skip 4-7 and try to go 8 but don't reach it and then get lost happens currently as well but this causes more issues 
        //---------------------------------------------------------------------------
        //Vector3 tempPos = transform.position;
        //tempPos.x = Mathf.Clamp(tempPos.x, -5.0f, 5.0f);//just random values that seemed to work okay in pathing 
        //transform.position = tempPos;
    }
}
