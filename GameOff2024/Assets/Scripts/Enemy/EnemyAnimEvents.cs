using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvents : MonoBehaviour
{
    [Header("Sound Effects")]
    [SerializeField] private AudioSource leftStepSound;
    [SerializeField] private AudioSource rightStepSound;

    public void StepLeft()
    {
        leftStepSound.Play();
    }

    public void StepRight()
    {
        rightStepSound.Play();
    }
}
