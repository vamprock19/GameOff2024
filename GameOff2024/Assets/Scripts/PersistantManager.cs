using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistantManager : MonoBehaviour
{
    public static PersistantManager Instance;
    //
    public bool startWithoutMainMenu = false;//if when loading the main menu, the menu should be shown or skipped
    
    public List<int> levelTimes = new List<int>();//record times for (potential) levels
    public int sensValue = 5;
    public int volValue = 10;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        //create empty list of player times
        Debug.Log("Scene Count: " + SceneManager.sceneCountInBuildSettings);//ToDo Remove
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            levelTimes.Add(-1);
        }
    }
}
