using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] string collectId = "";

    void Start()
    {
        collectId = collectId.ToLower();//make lowercase
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            //apply collectable
            switch(collectId)
            {
                case "disguisered":
                    if(col.gameObject.GetComponent<PlayerController>() != null)
                    {
                        col.gameObject.GetComponent<PlayerController>().disguisesOwned *= 2;
                    }
                    break;
                case "disguiseblue":
                    if(col.gameObject.GetComponent<PlayerController>() != null)
                    {
                        col.gameObject.GetComponent<PlayerController>().disguisesOwned *= 3;
                    }
                    break;
                case "disguisegreen":
                    if(col.gameObject.GetComponent<PlayerController>() != null)
                    {
                        col.gameObject.GetComponent<PlayerController>().disguisesOwned *= 5;
                    }
                    break;
                case "disguiseyellow":
                    if(col.gameObject.GetComponent<PlayerController>() != null)
                    {
                        col.gameObject.GetComponent<PlayerController>().disguisesOwned *= 7;
                    }
                    break;
                default:
                    Debug.LogError("Invalid collectable id: " + collectId + " at " + transform.position);
                    break;
            }
            //destroy collectable
            Destroy(gameObject);
        }
    }
}
