using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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
    

    [Header("Transition Components")]
    [SerializeField] private GameObject transitionScreen;
    [SerializeField] private Animator transAnim;



    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        tempElevator = FindObjectOfType<Elevator>();//find an elevator from this scene to use when restarting
    }

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
        StartCoroutine(LoadScene("SampleScene"));//ToDo edit level to load
    }

    IEnumerator NextLevelButtonActions()
    {
        HandleHideLevelWin();
        //ToDo Load next scene
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
        PatrolNavigation[] patrolEnemies = FindObjectsOfType<PatrolNavigation>();
        foreach (PatrolNavigation patroler in patrolEnemies)
        {
            patroler.enabled = false;
        }
        //Transition out
        TransitionToBlack();
        yield return new WaitForSecondsRealtime(1f);
        //Ensure timescale is normal(for paused menu)
        Time.timeScale = 1;
        //Hide Lose Menu AND pause menu if open
        HideLevelLoseScreen();
        HidePause();
        //Move Car to elevator with correct pose
        PlayerController playerController = FindObjectOfType<PlayerController>();//disable input
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
        StartCoroutine(LoadScene("SampleScene"));//ToDo edit level to load
    }

    IEnumerator ToMenuButtonActions()
    {
        //Disable enemy fuctionality to avoid loss triggering mid-transition
        PatrolNavigation[] patrolEnemies = FindObjectsOfType<PatrolNavigation>();
        foreach (PatrolNavigation patroler in patrolEnemies)
        {
            patroler.enabled = false;
        }
        //Transition out
        TransitionToBlack();
        yield return new WaitForSecondsRealtime(1f);
        //Ensure timescale is normal(for paused menu)
        Time.timeScale = 1;
        //ToDo Load Main Menu screen
        yield return new WaitForSecondsRealtime(1f);
        asyncOperation.allowSceneActivation = true;
        yield return new WaitForSecondsRealtime(1f);
        missedTheBusLevelLoading = true;
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
}
