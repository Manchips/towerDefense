using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    public Transform target;

    public HealthBar healthBar;
    
    public int health = 3; 
    public int coins = 3;
    
    public delegate void EnemyDied(int coins);
    
    public static event EnemyDied addCoins;
    
    // todo #1 create and get a reference to the NavMeshAgent 

    private NavMeshAgent agent; 
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //todo #3 - place enemy at closest navmesh point (create GetNavmeshPosition)
        
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
                if (Input.GetMouseButtonDown(0))
                {
                    if (hitInfo.collider.gameObject == gameObject)
                    {
                        health--;
                        healthBar.SetHealth(health);
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

    // Update is called once per frame
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
        
        //Vector3 meshPosition = GetNavMeshPosition(target.position);
        agent.SetDestination(target.position);

            //Ray pickRay = Camera.main.ScreenPointToRay()
        
    }

    Vector3 GetNavMeshPosition(Vector3 samplePosition)
    {
        //todo #2 - place enemy at closest waypoint 
        NavMesh.SamplePosition(samplePosition, out NavMeshHit hitinfo, 100f, -1);
        return hitinfo.position;
    }
}
