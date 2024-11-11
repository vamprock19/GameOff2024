using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolNavigation : MonoBehaviour
{
    enum EnemyState
    {
        NullState,      //0 - null
        Waiting,        //1 - transition between states
        Patrolling,     //2 - basic movement along route
        Navigating,     //3 - move to certain location
        Finding,        //4 - looking around for the player
        Spotting        //5 - currently seeing the player
    }
    
    public NavMeshAgent agent;
    [SerializeField] private PlayerLocomotionInput playerLocomotionInput;//ToDo remove
    private EnemyState currentState = EnemyState.Spotting;
    private GameObject player;

    [Header("Patrol Route")]
    [SerializeField] private Transform[] patrolPath;
    [SerializeField] private int patrolPointer = 0;
    private float defaultSpeed;


    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        defaultSpeed = agent.speed;

        //de-parent all "PatrolWaypoint" children (parented for organisation of patrols)
        if(agent.transform.Find("PatrolWaypointGroup") != null)
        {
            agent.transform.Find("PatrolWaypointGroup").parent = null;
        }
        //set patrol start path
        if(patrolPath.Length > 0)
        {
            if((patrolPointer >= patrolPath.Length) || (patrolPointer < 0))//fix patrol pointer errors
            {
                patrolPointer = 0;
            }
            agent.nextPosition = patrolPath[patrolPointer].position;//teleport to first waypoint
            if(currentState == EnemyState.Patrolling)
            {
                MoveToNextWaypoint();//start patrolling
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case EnemyState.Waiting:
                DoWaiting();
                break;
            case EnemyState.Patrolling:
                DoPatroling();
                break;
            case EnemyState.Navigating:
                DoNavigating();
                break;
            case EnemyState.Finding:
                DoFinding();
                break;
            case EnemyState.Spotting:
                DoSpotting();
                break;   
            default:
                break; 
        }

        //ToDo Remove following section (debug test for navigation)
        if (playerLocomotionInput.DebugInput > 0.1f)
        {
            //agent.SetDestination(playr.transform.position);
            NavigationAlert(player.transform.position);
        }
    }

    //State Functions
    private void DoWaiting()//Waiting State
    {
        //ToDo Modify this for looking around?
        agent.ResetPath();
        Invoke("MoveToNextWaypoint", 1);//return to patrol
        currentState = EnemyState.NullState;
    }

    private void MoveToNextWaypoint()
    {
        currentState = EnemyState.Patrolling;
        agent.SetDestination(patrolPath[patrolPointer].position);//go to next place
        patrolPointer = (patrolPointer + 1) % patrolPath.Length;
    }

    private void DoPatroling()//Patrolling State
    {
        if(((!agent.pathPending) && (agent.remainingDistance < 0.5f)) || (agent.pathStatus == NavMeshPathStatus.PathInvalid))//if patrol destination reached or failed
        {
            currentState = EnemyState.Waiting;
        }
    }

    private void DoNavigating()//Navigating State
    {
        //ToDo consider speed change
        if(((!agent.pathPending) && (agent.remainingDistance < 0.5f)) || (agent.pathStatus == NavMeshPathStatus.PathInvalid))//if patrol destination reached or failed
        {
            currentState = EnemyState.Finding;
        }
    }

    public void NavigationAlert(Vector3 searchLocation)//Tell enemy where to go to
    {
        currentState = EnemyState.Navigating;
        agent.SetDestination(searchLocation);//go to navigate location
    }

    private void DoFinding()//Finding State
    {
        //ToDo Modify this for looking around?
        agent.ResetPath();
        Invoke("MoveToNextWaypoint", 3);//return to patrol
        currentState = EnemyState.NullState;
    }

    private void DoSpotting()//Spotting State
    {
        //ToDo collision, timeout and lose condition
        agent.transform.LookAt(new Vector3(player.transform.position.x, agent.transform.position.y, player.transform.position.z), Vector3.up);
    }

    //

    
}
