using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//an object for storing persistant data and controlling game music
public class PersistantManager : MonoBehaviour
{
    public static PersistantManager Instance;
    public bool startWithoutMainMenu = false;//if when loading the main menu, the menu should be shown or skipped
    
    public List<int> levelTimes = new List<int>();//record times for (potential) levels
    public int sensValue = 5;
    public int volValue = 5;

    [Header("Music")]
    [SerializeField] private AudioSource mainMenuMusic;
    [SerializeField] private AudioSource gameplayMusic;
    [SerializeField] private AudioSource pauseMenuMusic;
    [SerializeField] private AudioSource elevatorMusic;


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



    //Music Functions-----------------------------------------------------------------------------------------------------------
    public void StartMainMenuMusic()
    {
        mainMenuMusic.Play();
        gameplayMusic.Stop();
        pauseMenuMusic.Stop();
        elevatorMusic.Stop();
    }

    public void StartGameplayMusic()
    {
        gameplayMusic.Play();
        pauseMenuMusic.Play();
        //smoothly transition into gameplay music
        gameplayMusic.time = mainMenuMusic.time + 4;
        pauseMenuMusic.time = mainMenuMusic.time + 4;
        mainMenuMusic.Stop();
        elevatorMusic.Stop();
    }

    public void OpenPauseMenuMusic()
    {
        gameplayMusic.volume = 0;
        pauseMenuMusic.volume = 1;
    }

    public void ClosePauseMenuMusic()
    {
        gameplayMusic.volume = 1;
        pauseMenuMusic.volume = 0;
    }

    public void StartElevatorMusic()
    {
        elevatorMusic.Play();
        mainMenuMusic.Stop();
        gameplayMusic.Stop();
        pauseMenuMusic.Stop();
    }

    public void StopAllMusic()
    {
        elevatorMusic.Stop();
        mainMenuMusic.Stop();
        gameplayMusic.Stop();
        pauseMenuMusic.Stop();
    }

    public void FadeOutAllMusic(float duration)
    {
        StartCoroutine(FadeOutMusic(duration));
    }

    public void FadeInAllMusic(float duration)//does not fade in pause menu music
    {
        StartCoroutine(FadeInMusic(duration));
    }

    IEnumerator FadeOutMusic(float duration)
    {
        float newDuration = (duration > 0 ? duration : 1);//ensure duration is valid
        float timer = 0;
        while(timer < newDuration)//progressively change volumes
        {
            float newVal = 1 - (timer / newDuration);
            mainMenuMusic.volume = Mathf.Min(mainMenuMusic.volume, newVal);
            gameplayMusic.volume = Mathf.Min(gameplayMusic.volume, newVal);
            pauseMenuMusic.volume = Mathf.Min(pauseMenuMusic.volume, newVal);
            elevatorMusic.volume = Mathf.Min(elevatorMusic.volume, newVal);
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
        }
        mainMenuMusic.volume = 0;
        gameplayMusic.volume = 0;
        pauseMenuMusic.volume = 0;
        elevatorMusic.volume = 0;
    }

    IEnumerator FadeInMusic(float duration)
    {
        float newDuration = (duration > 0 ? duration : 1);//ensure duration is valid
        float timer = 0;
        while(timer < newDuration)//progressively change volumes
        {
            float newVal = timer / newDuration;
            mainMenuMusic.volume = Mathf.Max(mainMenuMusic.volume, newVal);
            gameplayMusic.volume = Mathf.Max(gameplayMusic.volume, newVal);
            elevatorMusic.volume = Mathf.Max(elevatorMusic.volume, newVal);
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
        }
        mainMenuMusic.volume = 1;
        gameplayMusic.volume = 1;
        elevatorMusic.volume = 1;
    }
}
