using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorArrowScript : MonoBehaviour
{
    
    private Camera playerCamera;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    void FixedUpdate()
    {
        //have icon always face player
        transform.LookAt(playerCamera.transform);
        //set opacity based on distance
        float dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(playerCamera.transform.position.x, playerCamera.transform.position.z));
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Mathf.Min(1, dist / 250f));
        //do movement
        transform.localPosition = transform.localPosition + (Vector3.up * 0.1f * Mathf.Sin(5f * Time.timeSinceLevelLoad));
    }
}
