using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents : MonoBehaviour
{
    [SerializeField] private GameObject[] headLights;

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
}
