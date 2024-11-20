using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantManager : MonoBehaviour
{
    public static PersistantManager Instance;
    //
    public bool startWithoutMainMenu = false;//if when loading the main menu, the menu should be shown or skipped

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
