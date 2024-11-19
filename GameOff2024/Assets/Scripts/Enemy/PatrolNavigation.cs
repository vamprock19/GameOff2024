using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public enum DisguiseGroups
{
    None = 1,
    Red = 2,
    Blue = 3,
    Green = 5,
    Yellow = 7
}

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
    private Camera playerCamera;
    private float startRot;
    private float startTime;

    [Header("Suspicion")]
    public float suspicionMeter = 0;
    public float suspicionRate = 20;
    public GameObject suspicionIcon;
    [SerializeField] private SpriteRenderer suspicionIconRenderer;
    [SerializeField] private GameObject suspicionMask;
    [HideInInspector] public static PatrolNavigation gameOverEnemy;//a variable shared between PatrolNavigation instances to identify which enemy ended the game
    [SerializeField] private CinemachineVirtualCamera personalVCam;

    [Header("Patrol Route")]
    [SerializeField] private Transform[] patrolPath;
    [SerializeField] private int patrolPointer = 0;
    private float defaultSpeed;

    [Header("Stun")]
    [SerializeField] private EnemyConeDetection torchLight;

    [Header("Disguise")]//success of diguise calculated using prime numbers
    public DisguiseGroups disguiseNeeded = DisguiseGroups.None;//id of disguise needed by player to avoid detection by this when still.  1 - none, 2 - red, 3 - blue, 5 - green, 7 - yellow
    

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
        //act based on current state
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
        //set mask height
        suspicionMask.transform.localPosition = new Vector3(0, (Mathf.Min(suspicionMeter, 100) * 0.09f) + 0.5f, 0);
        suspicionIconRenderer.color = Color.HSVToRGB((60 - (Mathf.Min(suspicionMeter, 100) * 0.6f))/360f, 0.95f, 0.95f);
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
        NavigationStart(searchLocation);
        suspicionMeter = Mathf.Max(20, suspicionMeter);//set suspicion when alerted
        CheckForEndGame();
    }

    public void NavigationStart(Vector3 searchLocation)//Tell enemy where to go to
    {
        CancelInvoke();
        agent.speed = defaultSpeed;
        currentState = EnemyState.Navigating;
        agent.SetDestination(searchLocation);//go to navigate location
        enemyAnim.SetBool("isWalking", true);
        suspicionIcon.SetActive(true);
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
        TurnTowardsPlayer();
        //if too far, move closer
        Vector3 endPos = new Vector3(player.transform.position.x, agent.transform.position.y, player.transform.position.z);
        if(Vector3.Distance(agent.transform.position, endPos) > 15)
        {
            agent.SetDestination(endPos);
            enemyAnim.SetBool("isWalking", true);
        }
        else
        {
            agent.ResetPath();
            enemyAnim.SetBool("isWalking", false);
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

    private void TurnTowardsPlayer()
    {
        TurnTowardsPlayer(0.75f);
    }

    private void TurnTowardsPlayer(float turnSpeed)
    {
        //turn to face player
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = new Vector3(player.transform.position.x, agent.transform.position.y, player.transform.position.z);
        Vector3 direVect = (endPos - startPos).normalized;
        if(Mathf.Abs(Vector3.SignedAngle(agent.transform.forward, direVect, Vector3.up)) > 10)
        {
            if(Vector3.SignedAngle(agent.transform.forward, direVect, Vector3.up) > 0)
            {
                agent.transform.Rotate(new Vector3(0, turnSpeed, 0));
            }
            else
            {
                agent.transform.Rotate(new Vector3(0, -turnSpeed, 0));
            }
        }
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
            if(gameOverEnemy == null)//stop multiple enemies from running this script at the same time
            {
                gameOverEnemy = this;
                enemyAnim.SetBool("isWalking", false);
                enemyAnim.SetBool("isDizzy", false);
                currentState = EnemyState.NullState;
                CancelInvoke();
                agent.ResetPath();
                player.GetComponent<PlayerController>().ToggleInputOn(false);//disable input
                FindObjectOfType<CinemachineInputProvider>().enabled = false;//disable camera controls
                //pan camera to player
                if(personalVCam != null)
                {
                    personalVCam.enabled = true;
                }
                else
                {
                    EndGamePostCameraPan();//even if no pan, run function anyway
                }
            }
        }
    }

    public void EndGamePostCameraPan()//the engame functionality after the camera pan
    {
        //Lose Game
        FindObjectOfType<UIScripts>().ShowLevelLoseScreen();
    }
    
}
