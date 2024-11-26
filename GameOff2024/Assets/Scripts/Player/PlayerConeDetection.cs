using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConeDetection : MonoBehaviour
{    
    void OnTriggerEnter(Collider col)//if enemy spotted, enter spotting mode
    {
        if(col.tag == "Enemy")
        {
            //if there is direct line of sight
            RaycastHit hit;
            if(Physics.Raycast(transform.position, (col.transform.position - transform.position).normalized, out hit, 50))
            {
                if(hit.collider.gameObject.Equals(col.gameObject))
                {
                    if(col.GetComponent<PatrolNavigation>() != null)//if is a patrolling enemy, stun them
                    {
                        col.GetComponent<PatrolNavigation>().BecomeStunned();
                    }
                }
            }
        }
    }

    void OnTriggerStay(Collider col)//if enemy spotted, enter spotting mode
    {
        if(col.tag == "Enemy")
        {
            //if there is direct line of sight
            RaycastHit hit;
            if(Physics.Raycast(transform.position, (col.transform.position - transform.position).normalized, out hit, 50))
            {
                if(hit.collider.gameObject.Equals(col.gameObject))
                {
                    if(col.GetComponent<PatrolNavigation>() != null)//if is a patrolling enemy, stun them
                    {
                        col.GetComponent<PatrolNavigation>().BecomeStunned();
                    }
                }
            }
        }
    }
}
