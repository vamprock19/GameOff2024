using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            Debug.Log("ToDo Trigger End");
        }
    }
}
