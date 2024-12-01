using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Cinemachine;
using UnityEngine.AI;
using UnityEngine.Audio;

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
    [SerializeField] private Slider sensSlide;
    [SerializeField] private Slider volSlide;

    
    [Header("Main Menu Components")]
    public GameObject mainMenuScreen;
    public bool isMainMenuLevel;
    [SerializeField] private Button mainStartGame;
    [SerializeField] private CinemachineVirtualCamera mainMenuVCam;
    private PlayerController playerController;
    [SerializeField] private Slider sensSlideSettings;
    [SerializeField] private Slider volSlideSettings;
    [SerializeField] private List<TextMeshProUGUI> levelTimeRecords = new List<TextMeshProUGUI>();
    

    [Header("Transition Components")]
    [SerializeField] private GameObject transitionScreen;
    [SerializeField] private Animator transAnim;


    [Header("HUD Components")]
    [SerializeField] private Animator hudAnim;
    [SerializeField] private TextMeshProUGUI hudTimer;
    public int timePassed = 0;
    public GameObject abilityFlashBox;
    public Image abilityFlashImage;
    public GameObject abilityBeepBox;
    public Image abilityBeepImage;
    public Slider staminaBar;


    [Header("Sound Effects")]
    
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] private AudioSource elevatorOpenSound;


    void Start()
    {
        //Set level times values
        for(int i = 0; i < levelTimeRecords.Count; i++)
        {
            if(PersistantManager.levelTimes.Count > i)
            {
                //set time to show on records in correct format if it exists
                int newVal = PersistantManager.levelTimes[i];
                levelTimeRecords[i].text = newVal == -1 ? "--:--:--" : string.Format("{0:00}:{1:00}:{2:00}", (newVal / 3600), (newVal % 3600) /60, (newVal % 60));;
            }
        }
        if(levelTimeRecords.Count != PersistantManager.levelTimes.Count)
        {
            Debug.LogError("Mismatch in number of levels and assigned record displays: Please make sure all records are shown on menu: " + levelTimeRecords.Count + " displays - " + PersistantManager.levelTimes.Count + " records");
        }
        //Set slider value
        sensSlide.value = PersistantManager.sensValue;
        sensSlideSettings.value = PersistantManager.sensValue;
        UpdateSens();
        //Set volume value
        volSlide.value = PersistantManager.volValue;
        volSlideSettings.value = PersistantManager.volValue;
        UpdateVol();
        //find an elevator from this scene to use when restarting
        tempElevator = FindObjectOfType<Elevator>();
        //If starting from main menu level
        playerController = FindObjectOfType<PlayerController>();
        if(isMainMenuLevel)//if elevator not at level start (first level)
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
            if(PersistantManager.startWithoutMainMenu)
            {
                Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;
                //Coroutine to fade in before playing
                StartCoroutine(MainMenuSkipActions());
                PersistantManager pm = FindObjectOfType<PersistantManager>();
                if(pm != null)
                {
                    pm.StartGameplayMusic();
                }
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
            //Cursor.visible = false;
            PersistantManager pm = FindObjectOfType<PersistantManager>();
            if(pm != null)
            {
                pm.StopAllMusic();
            }
            elevatorOpenSound.Play();
        }
    }

    //Menu showing and hiding function (done like this to make adding transition animations easier)
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
        if((timePassed < PersistantManager.levelTimes[SceneManager.GetActiveScene().buildIndex]) || (PersistantManager.levelTimes[SceneManager.GetActiveScene().buildIndex] == -1))
        {
            PersistantManager.levelTimes[SceneManager.GetActiveScene().buildIndex] = timePassed;
            winRecord.gameObject.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(0.2f);
        winNextLevel.gameObject.SetActive(true);
        winNextLevel.Select();
        Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
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
        //Cursor.visible = false;
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
        //Cursor.visible = true;
        loseRetry.gameObject.SetActive(true);
        loseRetry.Select();
        loseHome.gameObject.SetActive(true);
    }

    //-----------------------------------------------------------------------------------------
    public void HideLevelLoseScreen()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
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
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
        pauseScreen.SetActive(true);
        pauseRetry.Select();
        Time.timeScale = 0;
        PersistantManager pm = FindObjectOfType<PersistantManager>();
        if(pm != null)
        {
            pm.OpenPauseMenuMusic();
        }
    }

    //-----------------------------------------------------------------------------------------
    public void HidePause()
    {
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        pauseScreen.SetActive(false);
        eveSys.SetSelectedGameObject(null);
        Time.timeScale = 1;
        PersistantManager pm = FindObjectOfType<PersistantManager>();
        if(pm != null)
        {
            pm.ClosePauseMenuMusic();
        }
    }

    //-----------------------------------------------------------------------------------------
    public void ShowMain()
    {
        mainMenuScreen.SetActive(true);
        mainStartGame.Select();
        //playerController.AnimSleep(true);
        PersistantManager pm = FindObjectOfType<PersistantManager>();
        if(pm != null)
        {
            pm.StartMainMenuMusic();
        }
    }

    //-----------------------------------------------------------------------------------------
    public void HideMain()
    {
        mainMenuScreen.SetActive(false);
    }

    //-----------------------------------------------------------------------------------------
    public void TransitionToBlack()
    {
        transAnim.SetTrigger("ToBlack");
        PersistantManager pm = FindObjectOfType<PersistantManager>();
        if(pm != null)
        {
            pm.FadeOutAllMusic(1);
        }
    }

    public void TransitionFromBlack()
    {
        transAnim.SetTrigger("FromBlack");
        PersistantManager pm = FindObjectOfType<PersistantManager>();
        if(pm != null)
        {
            pm.FadeInAllMusic(1);
        }
    }

    //-----------------------------------------------------------------------------------------
    public void HUDOut()
    {
        hudAnim.SetTrigger("HudOut");
    }

    public void HUDIn()
    {
        hudAnim.SetTrigger("HudIn");
    }

    //-----------------------------------------------------------------------------------------Button Functions
    public void NextLevelButtonPress()//go to next level after winning
    {
        eveSys.enabled = false;//disable inputs
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
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
        //Cursor.visible = false;
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
        PersistantManager.startWithoutMainMenu = true;
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
        //Cursor.visible = false;
        StartCoroutine(RetryLostLevelButtonActions());
        StartCoroutine(LoadScene(gameObject.scene.name));
    }

    IEnumerator RetryLostLevelButtonActions()
    {
        //Disable enemy fuctionality to avoid loss triggering mid-transition
        SetAllEnemiesEnableState(false);
        //Transition out
        TransitionToBlack();
        HUDOut();//turn off hud
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
                PersistantManager pm = FindObjectOfType<PersistantManager>();
                if(pm != null)
                {
                        pm.StartElevatorMusic();

                }
            }
            //Transition in
            TransitionFromBlack();
            yield return new WaitForSecondsRealtime(1f);
        }
        //in case of loading first level, set whether to show the main menu
        PersistantManager.startWithoutMainMenu = true;
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
        //Cursor.visible = false;
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
        PersistantManager.startWithoutMainMenu = false;
        //Load Main Menu screen
        yield return new WaitForSecondsRealtime(1f);
        Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
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
        //Cursor.visible = false;
        StartCoroutine(MainToStartButtonActions());
    }

    IEnumerator MainToStartButtonActions()//Start playing main menu level
    {
        yield return null;
        PersistantManager pm = FindObjectOfType<PersistantManager>();
        pm.StartGameplayMusic();
        //Hide Menu
        HideMain();
        HUDIn();//turn on hud
        //Wake Up
        //playerController.AnimSleep(false);
        yield return new WaitForSecondsRealtime(0.025f);
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
        StartTimer();//start timing
        //Enable enemy fuctionality
        SetAllEnemiesEnableState(true);
    }

    public void MainToLevelButtonPress(int levelIndex)//load a level from the main menu
    {
        eveSys.enabled = false;//disable inputs
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
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
                PersistantManager pm = FindObjectOfType<PersistantManager>();
                pm.StartElevatorMusic();
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
            //Cursor.visible = true;
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
    
    //-------------------------------------------------------------------------------------------Level Loading
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

    //-------------------------------------------------------------------------------------------HUD Functionality
    public void StartTimer()
    {
        timePassed = 0;
        InvokeRepeating("ClockTickUp", 1, 1);
    }

    public void StopTimer()
    {
        CancelInvoke("ClockTickUp");
        winTime.text = string.Format("{0:00}:{1:00}:{2:00}", (timePassed / 3600), (timePassed % 3600) /60, (timePassed % 60));
    }
    
    void ClockTickUp()
    {
        timePassed++;
        hudTimer.text = string.Format("{0:00}:{1:00}:{2:00}", (timePassed / 3600), (timePassed % 3600) /60, (timePassed % 60));
    }

    public void HudButtonPressPulse(GameObject button)//when the button for an ability is pressed
    {
        if(button.GetComponent<Animator>() != null)
        {
            button.GetComponent<Animator>().SetTrigger("Pulse");
        }
    }

    public void HudButtonPressFail(GameObject button)//when the button for an ability is pressed but fails
    {
        if(button.GetComponent<Animator>() != null)
        {
            button.GetComponent<Animator>().SetTrigger("Red");
        }
    }

    //-------------------------------------------------------------------------------------------Settings
    public void UpdateSens()//update the camera sensitivity
    {
        int newVal = 10;
        if(mainMenuScreen.activeSelf)
        {
            newVal = (int)sensSlideSettings.value;
            sensSlide.value = newVal;
        }
        else
        {
            newVal = (int)sensSlide.value;
            sensSlideSettings.value = newVal;
        }
        PersistantManager.sensValue = newVal;
        if(playerController != null)
        {
            if(playerController.mainVCam != null)
            {
                playerController.mainVCam.m_XAxis.m_MaxSpeed = newVal * 0.02f;
                playerController.mainVCam.m_YAxis.m_MaxSpeed = newVal * 0.0002f;
            }
        }
    }

    public void UpdateVol()//update the game volume
    {
        int newVal = 10;
        //make sure both sliders have correct value
        if(mainMenuScreen.activeSelf)
        {
            newVal = (int)volSlideSettings.value;
            volSlide.value = newVal;
        }
        else
        {
            newVal = (int)volSlide.value;
            volSlideSettings.value = newVal;
        }
        PersistantManager.volValue = newVal;
        //apply volume slider change
        SetMasterVolume(newVal);
    }

    private void SetMasterVolume(int value)
    {
        //calculate desired volume
        float newVolume = value <= 0 ? 0.0001f : (value / 5f);
        //apply volume slider change
        audioMixer.SetFloat("masterVolume", Mathf.Log10(newVolume) * 20);
    }
}
