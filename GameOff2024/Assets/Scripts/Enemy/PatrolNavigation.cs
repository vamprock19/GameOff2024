using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolNavigation : MonoBehaviour
{
    enum EnemyState
    {
        NullState,      //0 - null(idle)
        Waiting,        //1 - transition between states
        Patrolling,     //2 - basic movement along route
        Navigating,     //3 - move to certain location
        Finding,        //4 - looking around for the player
        Spotting,       //5 - currently seeing the player
        Stunned        //6 - unable to move or detect the player
    }
    
    public NavMeshAgent agent;
    [SerializeField] private Animator enemyAnim;
    private EnemyState currentState = EnemyState.Patrolling;
    private GameObject player;
    public float suspicionMeter = 0;
    public float suspicionRate = 10;
    public GameObject suspicionIcon;
    private Camera playerCamera;
    private float startRot;
    private float startTime;


    [Header("Patrol Route")]
    [SerializeField] private Transform[] patrolPath;
    [SerializeField] private int patrolPointer = 0;
    private float defaultSpeed;

    [Header("Stun")]
    [SerializeField] private EnemyConeDetection torchLight;
    
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
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
                enemyAnim.SetBool("isWalking", true);
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
            case EnemyState.Stunned:
                DoStunned();
                break;  
            default:
                break; 
        }
    }

    void FixedUpdate()
    {
        //have icon always face player
        suspicionIcon.transform.LookAt(playerCamera.transform);
    }

    //State Functions++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private void DoWaiting()//Waiting State----------------------------------------------------------------------------------------------------
    {
        agent.ResetPath();
        CancelInvoke();
        agent.speed = defaultSpeed;
        Invoke("MoveToNextWaypoint", 1);//return to patrol
        enemyAnim.SetBool("isWalking", false);
        currentState = EnemyState.NullState;
    }

    private void MoveToNextWaypoint()
    {
        torchLight.gameObject.SetActive(true);
        currentState = EnemyState.Patrolling;
        agent.SetDestination(patrolPath[patrolPointer].position);//go to next place
        enemyAnim.SetBool("isWalking", true);
        enemyAnim.SetBool("isDizzy", false);
        patrolPointer = (patrolPointer + 1) % patrolPath.Length;
    }

    private void DoPatroling()//Patrolling State----------------------------------------------------------------------------------------------------
    {
        if(((!agent.pathPending) && (agent.remainingDistance < 0.5f)) || (agent.pathStatus == NavMeshPathStatus.PathInvalid))//if patrol destination reached or failed
        {
            currentState = EnemyState.Waiting;
        }
    }

    private void DoNavigating()//Navigating State----------------------------------------------------------------------------------------------------
    {
        //ToDo consider speed change
        if(((!agent.pathPending) && (agent.remainingDistance < 0.5f)) || (agent.pathStatus == NavMeshPathStatus.PathInvalid))//if navigation destination reached or failed
        {
            currentState = EnemyState.Finding;
            startRot = agent.transform.eulerAngles.y;//initial rotation for looking around
            startTime = Time.timeSinceLevelLoad;
        }
    }

    public void NavigationAlert(Vector3 searchLocation)//Tell enemy where to go to
    {
        CancelInvoke();
        agent.speed = defaultSpeed;
        currentState = EnemyState.Navigating;
        agent.SetDestination(searchLocation);//go to navigate location
        enemyAnim.SetBool("isWalking", true);
        suspicionMeter += 10;//set suspicion when alerted
        suspicionIcon.SetActive(true);
        CheckForEndGame();
    }

    public void DelayedNavigation(Vector3 searchLocation)
    {
        agent.speed = 0;
        currentState = EnemyState.Navigating;
        agent.SetDestination(searchLocation);//go to navigate location
        enemyAnim.SetBool("isWalking", false);
        CancelInvoke();
        Invoke("EndDelay", 2);//start navigating
    }

    private void EndDelay()
    {
        agent.speed = defaultSpeed;
        enemyAnim.SetBool("isWalking", true);
    }

    private void DoFinding()//Finding State----------------------------------------------------------------------------------------------------
    {
        agent.ResetPath();
        enemyAnim.SetBool("isWalking", false);
        CancelInvoke();
        agent.speed = defaultSpeed;
        suspicionMeter = suspicionMeter - (Time.deltaTime * suspicionRate);//decrease suspicion while searching
        //rotate enemy to search
        agent.transform.eulerAngles = new Vector3(agent.transform.eulerAngles.x, startRot + (60 * Mathf.Sin(Time.timeSinceLevelLoad - startTime)), agent.transform.eulerAngles.z);
        //if suspicion gone, continue patrol
        if(suspicionMeter <= 0)
        {
            suspicionMeter = 0;
            suspicionIcon.SetActive(false);
            Invoke("MoveToNextWaypoint", 1);//return to patrol after 1 second
            currentState = EnemyState.NullState;
        }
    }

    private void DoSpotting()//Spotting State----------------------------------------------------------------------------------------------------
    {
        //turn to face player
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = new Vector3(player.transform.position.x, agent.transform.position.y, player.transform.position.z);
        Vector3 direVect = (endPos - startPos).normalized;
        if(Mathf.Abs(Vector3.SignedAngle(agent.transform.forward, direVect, Vector3.up)) > 10)
        {
            if(Vector3.SignedAngle(agent.transform.forward, direVect, Vector3.up) > 0)
            {
                agent.transform.Rotate(new Vector3(0, 0.75f, 0));
            }
            else
            {
                agent.transform.Rotate(new Vector3(0, -0.75f, 0));
            }
        }
        //increment suspicion meter
        suspicionMeter += (Time.deltaTime * suspicionRate);
        suspicionIcon.SetActive(true);
        CheckForEndGame();
    }

    public void SpotPlayer()
    {
        agent.ResetPath();
        enemyAnim.SetBool("isWalking", false);
        CancelInvoke();
        agent.speed = defaultSpeed;
        currentState = EnemyState.Spotting;
    }

    private void DoStunned()//Stunned State----------------------------------------------------------------------------------------------------
    {
        agent.ResetPath();
        enemyAnim.SetBool("isWalking", false);
        enemyAnim.SetBool("isDizzy", true);
        CancelInvoke();
        agent.speed = defaultSpeed;
        torchLight.TurnOffTorch();
        torchLight.gameObject.SetActive(false);//turn off torch
        Invoke("MoveToNextWaypoint", 5);//return to patrol
        currentState = EnemyState.NullState;
    }

    public void BecomeStunned()
    {
        agent.ResetPath();
        enemyAnim.SetBool("isWalking", false);
        enemyAnim.SetBool("isDizzy", true);
        suspicionMeter = 0;
        suspicionIcon.SetActive(false);
        CancelInvoke();
        agent.speed = defaultSpeed;
        currentState = EnemyState.Stunned;
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    private void CheckForEndGame()
    {
        if(suspicionMeter >= 100)
        {
            suspicionMeter = 100;
            //currentState = EnemyState.NullState;
            //ToDo Lose Game Here
            
        }
    }
    
}
