using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] string collectId = "";
    [SerializeField] AudioSource collectSound;

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
                case "abilityflash":
                    if(col.gameObject.GetComponent<PlayerController>() != null)
                    {
                        col.gameObject.GetComponent<PlayerController>().flashUnlocked = true;
                    }
                    break;
                case "abilitybeep":
                    if(col.gameObject.GetComponent<PlayerController>() != null)
                    {
                        col.gameObject.GetComponent<PlayerController>().beepUnlocked = true;
                    }
                    break;
                default:
                    Debug.LogError("Invalid collectable id: " + collectId + " at " + transform.position);
                    break;
            }
            //disable collectable
            if(collectSound != null)
            {
                collectSound.Play();
            }
            //Destroy(gameObject);
            //gameObject.SetActive(false);
            transform.position -= Vector3.down * 1000;//send away object
        }
    }

    void FixedUpdate()
    {
        //do movement
        transform.localPosition = transform.localPosition + (Vector3.up * 0.1f * Mathf.Sin(5f * Time.timeSinceLevelLoad));
    }
}
