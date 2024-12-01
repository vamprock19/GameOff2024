using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera personalVCamOutside;
    [SerializeField] private CinemachineVirtualCamera personalVCamInside;
    [SerializeField] private Animator elevatorAnim;
    [SerializeField] private Transform outdoorLocation;
    [SerializeField] private GameObject invisWall;
    private PlayerController playerController;
    public bool isStartgameElevator = false;//is this elevator for the start or end of a level
    private float playerYDiff = 1.08f;//approx height of player above floor. (Not calculated live to avoid some bugs)

    [Header("Sound Effects")]
    [SerializeField] private AudioSource elevatorCloseSound;


    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        if(isStartgameElevator)//if elevator at level start
        {
            //cut camera to inside
            if(personalVCamInside != null)
            {
                personalVCamInside.enabled = true;
                personalVCamOutside.enabled = true;
                //set player position inside lift
                PlaceCarInLift();
                //disable trigger outside
                GetComponent<BoxCollider>().enabled = false;
                //disable player controls
                playerController.ToggleInputOn(false);//disable input
                FindObjectOfType<CinemachineInputProvider>().enabled = false;//disable camera controls
                StartGameCameraPan();
            }
        }
        else//level ending elevator
        {
            //start by opening door
            elevatorAnim.SetTrigger("ToggleElevatorState");
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            if(PatrolNavigation.gameOverEnemy == null)//stop clash of win and lose condition (only try to win if not losing)
            {
                GameObject temp = new GameObject();
                PatrolNavigation pn = temp.AddComponent<PatrolNavigation>();
                pn.enabled = false;
                PatrolNavigation.gameOverEnemy = pn;//set static value to stop game losses happening afterwards
                //remove enemy fuctionality to avoid loss triggering
                PatrolNavigation[] patrolEnemies = FindObjectsOfType<PatrolNavigation>();
                foreach (PatrolNavigation patroller in patrolEnemies)
                {
                    patroller.NullifyState();
                    patroller.enabled = false;
                }
                //disable movement
                playerController = col.GetComponent<PlayerController>();
                playerController.ToggleInputOn(false);//disable input
                FindObjectOfType<CinemachineInputProvider>().enabled = false;//disable camera controls
                playerController.AllowMovement(false);
                FindObjectOfType<UIScripts>().StopTimer();//stop timing
                FindObjectOfType<UIScripts>().HUDOut();//turn off hud
                //pan camera to outside lift
                if(personalVCamOutside != null)
                {
                    personalVCamOutside.enabled = true;
                    EndGamePostCameraPan();
                }
                else
                {
                    EndGamePostCameraPan();//even if no pan, run function anyway
                }
            }
        }
    }

    //Game End Functions--------------------------------------------------------------------------------------------------------------------

    public void EndGamePostCameraPan()//the endgame functionality after the camera pan
    {
        if(!isStartgameElevator)
        {
            //Player walk in
            playerController.isAutomoving = true;
            playerController.autoDestinationQueue.Add(outdoorLocation.transform.position);
            playerController.autoDestinationQueue.Add((transform.position - outdoorLocation.transform.position).normalized * 0.15f + transform.position);
            playerController.autoDestinationQueue.Add(transform.position);
            StartCoroutine(EndGamePostCarAnimation());
        }
    }

    IEnumerator EndGamePostCarAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        playerController.AllowMovement(true);
        //wait until car inside
        float dist = Vector3.Distance(new Vector3(playerController.transform.position.x, 0, playerController.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));
        while(dist > 1)
        {
            yield return null;
            dist = Vector3.Distance(new Vector3(playerController.transform.position.x, 0, playerController.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));
        }
        //Animate door close
        yield return new WaitForSeconds(0.5f);
        elevatorAnim.SetTrigger("ToggleElevatorState");
        elevatorCloseSound.Play();
        yield return new WaitForSeconds(1f);
        //fix position and rotation
        PlaceCarInLift();
        yield return new WaitForSeconds(1f);
        //cut camera to inside
        if(personalVCamInside != null)
        {
            personalVCamInside.enabled = true;
            PersistantManager pm = FindObjectOfType<PersistantManager>();
            pm.StartElevatorMusic();
        }
        playerController.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        //Win Game Here
        FindObjectOfType<UIScripts>().ShowLevelWinScreen();
    }

    //Game Start Functions--------------------------------------------------------------------------------------------------------------------

    public void StartGameCameraPan()//the endgame functionality after the camera pan
    {
        if(isStartgameElevator)
        {
            PlaceCarInLift();
            StartCoroutine(StartGamePostCarAnimation());
        }
    }

    IEnumerator StartGamePostCarAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        playerController.AllowMovement(true);
        yield return new WaitForSeconds(0.5f);
        //Player walk out after doors open
        playerController.isAutomoving = true;
        elevatorAnim.SetTrigger("ToggleElevatorState");
        playerController.autoDestinationQueue.Add((outdoorLocation.transform.position - transform.position).normalized * 10.0f + outdoorLocation.transform.position);
        //move camera
        if(personalVCamOutside != null)
        {
            personalVCamInside.enabled = false;
            PersistantManager pm = FindObjectOfType<PersistantManager>();
            pm.StartGameplayMusic();
        }
        //wait until car outside
        float dist = Vector3.Distance(new Vector3(playerController.transform.position.x, 0, playerController.transform.position.z), new Vector3(outdoorLocation.transform.position.x, 0, outdoorLocation.transform.position.z));
        while(dist > 1)
        {
            yield return null;
            dist = Vector3.Distance(new Vector3(playerController.transform.position.x, 0, playerController.transform.position.z), new Vector3(outdoorLocation.transform.position.x, 0, outdoorLocation.transform.position.z));
        }
        //close doors
        elevatorAnim.SetTrigger("ToggleElevatorState");
        //wait until at destination
        Vector3 destination = (outdoorLocation.transform.position - transform.position).normalized * 10.0f + outdoorLocation.transform.position;
        dist = Vector3.Distance(new Vector3(playerController.transform.position.x, 0, playerController.transform.position.z), new Vector3(destination.x, 0, destination.z));
        while(dist > 1)
        {
            yield return null;
            dist = Vector3.Distance(new Vector3(playerController.transform.position.x, 0, playerController.transform.position.z), new Vector3(destination.x, 0, destination.z));
        }
        playerController.CancelAutoMovement();
        //move to gameplay camera
        if(personalVCamOutside != null)
        {
            playerController.SetCameraYAngleManual(transform.eulerAngles.y);
            personalVCamOutside.enabled = false;
        }
        //fix position and rotation
        playerController.transform.position = destination;
        playerController.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        FindObjectOfType<UIScripts>().HUDIn();//turn on hud
        yield return new WaitForSeconds(1.1f);
        //create collision on doors
        invisWall.GetComponent<BoxCollider>().enabled = true;
        //re-enable movement
        playerController = FindObjectOfType<PlayerController>();
        playerController.ToggleInputOn(true);//enble input
        playerController.AllowMovement(true);
        FindObjectOfType<CinemachineInputProvider>().enabled = true;//enable camera controls
        FindObjectOfType<UIScripts>().StartTimer();//start timing
    }

    //--------------------------------------------------------------------------------------------------------------------------------------
    public void PlaceCarInLift()
    {
        if(playerController != null)
        {
            //playerController.transform.position = new Vector3(transform.position.x, playerController.transform.position.y, transform.position.z);
            playerController.transform.position = new Vector3(transform.position.x, playerYDiff + transform.position.y, transform.position.z);
            playerController.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
    }

    public void ActivateIndoorCamera()
    {
        personalVCamInside.enabled = true;
    }
}
