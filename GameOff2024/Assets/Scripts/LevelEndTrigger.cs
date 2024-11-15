using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera personalVCam;
    [SerializeField] private Animator elevatorAnim;

    void Start()
    {
        //start by opening door
        elevatorAnim.SetTrigger("ToggleElevatorState");
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            if(PatrolNavigation.gameOverEnemy == null)//stop clash of win and lose condition (only win if not losing)
            {
                PatrolNavigation.gameOverEnemy = new PatrolNavigation();//set value to stop losses happening afterwards
                col.GetComponent<PlayerController>().ToggleInputOn(false);//disable input
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
        //ToDo Player walk in
        //ToDo Animate door close
        elevatorAnim.SetTrigger("ToggleElevatorState");
        //ToDo Win Game Here
        Debug.Log("ToDo Won Game");
    }
}
