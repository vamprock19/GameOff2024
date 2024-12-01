using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConeDetection : MonoBehaviour
{
    [SerializeField] private PatrolNavigation patrolNavigation;
    private bool isLookingAtPlayer = false;
    private GameObject player;
    private PlayerController playerCont;

    // Start is called before the first frame update
    void Start()
    {
        playerCont = FindObjectOfType<PlayerController>();
        player = playerCont.gameObject;
    }

    void FixedUpdate()
    {
        //if looking at player, check until no longer visible
        if(isLookingAtPlayer)
        {
            //if there is no direct line of sight
            RaycastHit hit;
            if(Physics.Raycast(patrolNavigation.transform.position, (player.transform.position - patrolNavigation.transform.position).normalized, out hit, 50))
            {
                if(!hit.collider.gameObject.Equals(player.gameObject))//if not in LOS
                {
                    //if player no longer seen, investigate last seen location
                    patrolNavigation.DelayedNavigation(player.transform.position);
                    isLookingAtPlayer = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider col)//if player spotted, enter spotting mode
    {
        if(col.tag == "Player")
        {
            //if there is direct line of sight
            RaycastHit hit;
            if(Physics.Raycast(patrolNavigation.transform.position, (col.transform.position - patrolNavigation.transform.position).normalized, out hit, 50))
            {
                if(hit.collider.gameObject.Equals(col.gameObject))
                {
                    //if player is not hiding, OR trying to hide but doesnt have valid disguise for enemy OR enemy already suspicious
                    if((!playerCont.isTryingToHide) || ((playerCont.isTryingToHide) && (playerCont.disguisesOwned % (int)patrolNavigation.disguiseNeeded != 0)) || (patrolNavigation.suspicionMeter >= 1))
                    {
                        patrolNavigation.SpotPlayer();
                        isLookingAtPlayer = true;
                    }
                }
            }
        }
    }

    void OnTriggerStay(Collider col)//catch edge cases where player is in torchlight before in LOS
    {
        if(!isLookingAtPlayer)//if not already looking at player
        {
            if(col.tag == "Player")
            {
                //if there is direct line of sight
                RaycastHit hit;
                if(Physics.Raycast(patrolNavigation.transform.position, (col.transform.position - patrolNavigation.transform.position).normalized, out hit, 50))
                {
                    if(hit.collider.gameObject.Equals(col.gameObject))
                    {
                        //if player is not hiding, OR trying to hide but doesnt have valid disguise for enemy OR enemy already suspicious
                        if((!playerCont.isTryingToHide) || ((playerCont.isTryingToHide) && (playerCont.disguisesOwned % (int)patrolNavigation.disguiseNeeded != 0)) || (patrolNavigation.suspicionMeter >= 1))
                        {
                            patrolNavigation.SpotPlayer();
                            isLookingAtPlayer = true;
                        }
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider col)//if player no longer spotted, enter investigating mode
    {
        if(isLookingAtPlayer)
        {
            if(col.tag == "Player")
            {
                //if player no longer seen, investigate last seen location
                patrolNavigation.DelayedNavigation(player.transform.position);
                isLookingAtPlayer = false;
            }
        }
    }

    public void TurnOffTorch()
    {
        isLookingAtPlayer = false;
    }
}
