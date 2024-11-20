using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Cinemachine;
using UnityEngine.AI;

public class UIScripts : MonoBehaviour
{
    private AsyncOperation asyncOperation;//scene loading operation
    private bool missedTheBusLevelLoading = false;//fix failed attempts to load levels
    private Elevator tempElevator;
    public EventSystem eveSys;

    [Header("Win Screen Components")]
    public GameObject levelWinScreen;
    [SerializeField] private TextMeshProUGUI winTitle;
    [SerializeField] private TextMeshProUGUI winTime;
    [SerializeField] private TextMeshProUGUI winRecord;
    [SerializeField] private Button winNextLevel;
    [SerializeField] private Button winRetry;
    [SerializeField] private Button winHome;


    [Header("Lose Screen Components")]
    public GameObject levelLoseScreen;
    [SerializeField] private Button loseRetry;
    [SerializeField] private Button loseHome;


    [Header("Pause Components")]
    public GameObject pauseScreen;
    private bool isPaused;
    [SerializeField] private Button pauseRetry;
    [SerializeField] private Button pauseHome;

    
    [Header("Main Menu Components")]
    public GameObject mainMenuScreen;
    public bool isMainMenuLevel;
    [SerializeField] private Button mainStartGame;
    //[SerializeField] private Button mainLevelSelect;
    [SerializeField] private CinemachineVirtualCamera mainMenuVCam;
    private PlayerController playerController;
    

    [Header("Transition Components")]
    [SerializeField] private GameObject transitionScreen;
    [SerializeField] private Animator transAnim;



    void Start()
    {
        tempElevator = FindObjectOfType<Elevator>();//find an elevator from this scene to use when restarting
        //If starting from main menu level
        playerController = FindObjectOfType<PlayerController>();
        if(isMainMenuLevel)//if elevator not at level start
        {
            playerController.AllowMovement(true);
            //Disable enemy fuctionality
            SetAllEnemiesEnableState(false);
            //Transition in
            TransitionFromBlack();
            //cut to main menu camera
            if(mainMenuVCam != null)
            {
                mainMenuVCam.enabled = true;
                //disable player controls
                playerController.ToggleInputOn(false);//disable input
                FindObjectOfType<CinemachineInputProvider>().enabled = false;//disable camera controls
            }
            //if using main menu or not
            PersistantManager pm = FindObjectOfType<PersistantManager>();
            if(pm.startWithoutMainMenu)
            {
                Cursor.lockState = CursorLockMode.Locked;
                //Coroutine to fade in before playing
                StartCoroutine(MainMenuSkipActions());
            }
            else
            {
                //Show Menu
                ShowMain();
            }
        }
        else//for all other levels
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //Menu showing and hiding function (done like this to make adding transition animations easier) //ToDo Get rid of unneeded coroutines
    //-----------------------------------------------------------------------------------------
    public void ShowLevelWinScreen()
    {
        StartCoroutine(HandleShowLevelWin());
    }

    IEnumerator HandleShowLevelWin()
    {
        yield return new WaitForSecondsRealtime(1f);
        levelWinScreen.SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        winTitle.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        winTime.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        winRecord.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        winNextLevel.gameObject.SetActive(true);
        winNextLevel.Select();
        Cursor.lockState = CursorLockMode.None;
        yield return new WaitForSecondsRealtime(0.2f);
        winRetry.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.2f);
        winHome.gameObject.SetActive(true);
    }

    //-----------------------------------------------------------------------------------------
    public void HideLevelWinScreen()
    {
        StartCoroutine(HandleHideLevelWin());
    }

    IEnumerator HandleHideLevelWin()
    {
        yield return null;
        Cursor.lockState = CursorLockMode.Locked;
        levelWinScreen.SetActive(false);
        eveSys.SetSelectedGameObject(null);
    }

    //-----------------------------------------------------------------------------------------
    public void ShowLevelLoseScreen()
    {
        StartCoroutine(HandleShowLevelLose());
    }

    IEnumerator HandleShowLevelLose()
    {
        yield return new WaitForSecondsRealtime(2f);
        levelLoseScreen.SetActive(true);
        //yield return new WaitForSecondsRealtime(0.5f);
        Cursor.lockState = CursorLockMode.None;
        loseRetry.gameObject.SetActive(true);
        loseRetry.Select();
        loseHome.gameObject.SetActive(true);
    }

    //-----------------------------------------------------------------------------------------
    public void HideLevelLoseScreen()
    {
        StartCoroutine(HandleHideLevelLose());
    }

    IEnumerator HandleHideLevelLose()
    {
        yield return null;
        Cursor.lockState = CursorLockMode.Locked;
        levelLoseScreen.SetActive(false);
        eveSys.SetSelectedGameObject(null);
    }

    //-----------------------------------------------------------------------------------------
    public void TogglePause()
    {
        if(isPaused)
        {
            HidePause();
        }
        else
        {
            ShowPause();
        }
    }

    public void ShowPause()
    {
        StartCoroutine(HandleShowPause());
    }

    IEnumerator HandleShowPause()
    {
        yield return null;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        pauseScreen.SetActive(true);
        pauseRetry.Select();
        Time.timeScale = 0;
    }

    //-----------------------------------------------------------------------------------------
    public void HidePause()
    {
        StartCoroutine(HandleHidePause());
    }

    IEnumerator HandleHidePause()
    {
        yield return null;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseScreen.SetActive(false);
        eveSys.SetSelectedGameObject(null);
        Time.timeScale = 1;
    }

    //-----------------------------------------------------------------------------------------
    public void ShowMain()
    {
        StartCoroutine(HandleShowMain());
    }

    IEnumerator HandleShowMain()
    {
        yield return null;
        mainMenuScreen.SetActive(true);
    }

    //-----------------------------------------------------------------------------------------
    public void HideMain()
    {
        StartCoroutine(HandleHideMain());
    }

    IEnumerator HandleHideMain()
    {
        yield return null;
        mainMenuScreen.SetActive(false);
    }

    //-----------------------------------------------------------------------------------------
    public void TransitionToBlack()
    {
        transAnim.SetTrigger("ToBlack");
    }

    public void TransitionFromBlack()
    {
        transAnim.SetTrigger("FromBlack");
    }

    //-----------------------------------------------------------------------------------------Button Functions
    public void NextLevelButtonPress()//go to next level after winning
    {
        eveSys.enabled = false;//disable inputs
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(NextLevelButtonActions());
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));//level to load
    }

    IEnumerator NextLevelButtonActions()
    {
        HandleHideLevelWin();
        //Load next scene
        yield return new WaitForSecondsRealtime(1f);
        asyncOperation.allowSceneActivation = true;
        yield return new WaitForSecondsRealtime(1f);
        missedTheBusLevelLoading = true;
    }

    public void RetryWonLevelButtonPress()//retry level after winning
    {
        eveSys.enabled = false;//disable inputs
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(RetryWonLevelButtonActions());
        StartCoroutine(LoadScene(gameObject.scene.name));
    }

    IEnumerator RetryWonLevelButtonActions()
    {
        HandleHideLevelWin();
        if(isMainMenuLevel)//if the main menu level
        {
            //Transition in
            TransitionToBlack();
            yield return new WaitForSecondsRealtime(1f);
        }
        //in case of loading first level, set whether to show the main menu
        PersistantManager pm = FindObjectOfType<PersistantManager>();
        pm.startWithoutMainMenu = true;
        //Reload this scene
        yield return new WaitForSecondsRealtime(1f);
        asyncOperation.allowSceneActivation = true;
        yield return new WaitForSecondsRealtime(1f);
        missedTheBusLevelLoading = true;
    }

    public void RetryLostLevelButtonPress()//retry level after losing (or from pause menu)
    {
        eveSys.enabled = false;//disable inputs
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(RetryLostLevelButtonActions());
        StartCoroutine(LoadScene(gameObject.scene.name));
    }

    IEnumerator RetryLostLevelButtonActions()
    {
        //Disable enemy fuctionality to avoid loss triggering mid-transition
        SetAllEnemiesEnableState(false);
        //Transition out
        TransitionToBlack();
        yield return new WaitForSecondsRealtime(1f);
        //Ensure timescale is normal(for paused menu)
        Time.timeScale = 1;
        //Hide Lose Menu AND pause menu if open
        HideLevelLoseScreen();
        HidePause();
        if(!isMainMenuLevel)//if not the main menu level
        {
            //Move Car to elevator with correct pose
            playerController.AllowMovement(false);
            playerController.ToggleInputOn(false);
            if(tempElevator != null)
            {
                tempElevator.PlaceCarInLift();
                tempElevator.ActivateIndoorCamera();
            }
            //Transition in
            TransitionFromBlack();
            yield return new WaitForSecondsRealtime(1f);
        }
        //in case of loading first level, set whether to show the main menu
        PersistantManager pm = FindObjectOfType<PersistantManager>();
        pm.startWithoutMainMenu = true;
        //Reload this scene
        yield return new WaitForSecondsRealtime(1f);
        asyncOperation.allowSceneActivation = true;
        yield return new WaitForSecondsRealtime(1f);
        missedTheBusLevelLoading = true;
    }

    public void ToMenuButtonPress()//return to main menu
    {
        eveSys.enabled = false;//disable inputs
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(ToMenuButtonActions());
        StartCoroutine(LoadScene(0));//level to load
    }

    IEnumerator ToMenuButtonActions()
    {
        //Disable enemy fuctionality to avoid loss triggering mid-transition
        SetAllEnemiesEnableState(false);
        //Transition out
        TransitionToBlack();
        yield return new WaitForSecondsRealtime(1f);
        //Ensure timescale is normal(for paused menu)
        Time.timeScale = 1;
        //set to show the main menu
        PersistantManager pm = FindObjectOfType<PersistantManager>();
        pm.startWithoutMainMenu = false;
        //Load Main Menu screen
        yield return new WaitForSecondsRealtime(1f);
        Cursor.lockState = CursorLockMode.None;
        asyncOperation.allowSceneActivation = true;
        yield return new WaitForSecondsRealtime(1f);
        missedTheBusLevelLoading = true;
    }

    IEnumerator MainMenuSkipActions()//skip the menu navigation of the main menu
    {
        //Hide Menu
        HideMain();
        yield return new WaitForSecondsRealtime(2f);
        //Play level start animation
        MainToStartButtonPress();
    }

    public void MainToStartButtonPress()
    {
        eveSys.enabled = false;//disable inputs
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(MainToStartButtonActions());
    }

    IEnumerator MainToStartButtonActions()//Start playing main menu level
    {
        yield return null;
        //Hide Menu
        HideMain();
        //handle cameras
        if(mainMenuVCam != null)
        {
            mainMenuVCam.enabled = false;
            yield return new WaitForSecondsRealtime(1.5f);
        }
        //re-enable movement
        playerController = FindObjectOfType<PlayerController>();
        playerController.ToggleInputOn(true);//enble input
        playerController.AllowMovement(true);
        FindObjectOfType<CinemachineInputProvider>().enabled = true;//enable camera controls
        eveSys.enabled = true;//re-enable ui inputs
        //Enable enemy fuctionality
        SetAllEnemiesEnableState(true);
    }

    public void MainToLevelButtonPress(int levelIndex)//load a level from the main menu
    {
        eveSys.enabled = false;//disable inputs
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(MainToLevelButtonActions(levelIndex));
    }

    IEnumerator MainToLevelButtonActions(int levelIndex)
    {
        yield return null;
        //if loading this level, continue as if play was pressed
        if(levelIndex == 0)
        {
            //Play level start animation
            MainToStartButtonPress();
        }
        else
        {
            StartCoroutine(LoadScene(levelIndex));//level to load
            //Transition out
            TransitionToBlack();
            yield return new WaitForSecondsRealtime(1f);
            //Move Car to elevator with correct pose
            playerController.AllowMovement(false);
            playerController.ToggleInputOn(false);
            if(tempElevator != null)
            {
                tempElevator.PlaceCarInLift();
                tempElevator.ActivateIndoorCamera();
            }
            mainMenuVCam.enabled = false;
            //hide menu
            HideMain();
            //Transition in
            TransitionFromBlack();
            yield return new WaitForSecondsRealtime(1f);
            //Load level
            yield return new WaitForSecondsRealtime(1f);
            Cursor.lockState = CursorLockMode.None;
            asyncOperation.allowSceneActivation = true;
            yield return new WaitForSecondsRealtime(1f);
            missedTheBusLevelLoading = true;
        }
        eveSys.enabled = true;//re-enable ui inputs
    }

    //-------------------------------------------------------------------------------------------Enemy Enable/Disabling
    private void SetAllEnemiesEnableState(bool newState)
    {
        //Set enemy fuctionality to avoid loss triggering mid-transition
        PatrolNavigation[] patrolEnemies = FindObjectsOfType<PatrolNavigation>();
        foreach (PatrolNavigation patroller in patrolEnemies)
        {
            patroller.enabled = newState;
            //patroller.GetComponent<NavMeshAgent>().enabled = newState;
        }
    }
    
    //-----------------------------------------------------------------------------------------Level Loading
    IEnumerator LoadScene(string sceneName)
    {
        yield return null;
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);//select level to load
        //dont activate scene until told
        asyncOperation.allowSceneActivation = false;
        //while its not done
        while (!asyncOperation.isDone)
        {
            if(missedTheBusLevelLoading)//if a bug occured due to timing, make sure the next level can still be loaded
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        yield return null;
        asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);//select level to load
        //dont activate scene until told
        asyncOperation.allowSceneActivation = false;
        //while its not done
        while (!asyncOperation.isDone)
        {
            if(missedTheBusLevelLoading)//if a bug occured due to timing, make sure the next level can still be loaded
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
