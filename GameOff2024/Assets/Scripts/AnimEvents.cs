using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    [SerializeField] private GameObject[] headLights;
    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public void LightOn()
    {
        //show headlights
        foreach(GameObject g in headLights)
        {
            g.SetActive(true);
        }
    }
    
    public void LightOff()
    {
        Invoke("DelayedLightOff", 0.25f);
    }

    void DelayedLightOff()
    {
        //show headlights
        foreach(GameObject g in headLights)
        {
            g.SetActive(false);
        }
    }

    public void BeepStart()
    {
        //from players position, get all colliders within radius
        Collider[] hitCols = Physics.OverlapSphere(player.transform.position, 50);
        foreach(Collider col in hitCols)
        {
            if(col.GetComponent<PatrolNavigation>() != null)//if collider is a patrolling enemy
            {
                col.GetComponent<PatrolNavigation>().NavigationAlert(player.transform.position);//tell enemy to search
            }
        }
    }

    public void AbilityStart()//signal to player that they have started an ability
    {
        player.isMidAbility = true;
    }

    public void AbilityEnd(float delay)//signal to player that they have stopped an ability
    {
        Invoke("DelayedAbilityEnd", delay);
    }

    void DelayedAbilityEnd()
    {
        player.isMidAbility = false;
    }

    public void SetTryingToHide(int oneForTrue)
    {
        player.isTryingToHide = oneForTrue == 1 ? true:false;
    }
}
