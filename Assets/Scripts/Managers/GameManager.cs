using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }

    public GameState currentState;
    public GameState previousState;

    [SerializeReference]
    public UpgradeLevelsConfig upgradeConfig;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;

    [Header("Current Stat Displays")]
    public TMP_Text currentHealthDisplay;
    public TMP_Text currentRegenDisplay;
    public TMP_Text currentMoveSpeedDisplay;
    public TMP_Text currentMightDisplay;
    public TMP_Text currentGoldDisplay;
    public TMP_Text currentMagnetDisplay;

    [Header("Result Screen Displays")]
    public Image chosenCharacterImage;
    public TMP_Text chosenCharacterName;
    public TMP_Text levelReachedDisplay;
    public TMP_Text timeSurvivedDisplay;
    public List<Image> chosenWeaponsUI = new();
    public List<Image> chosenPassiveItemsUI = new();

    [Header("Stopwatch")]
    public float timeLimit;
    private float stopwatchTime;
    public TMP_Text stopwatchDisplay;


    public bool isGameOver = false;

    public bool isChoosingUpgrades;

    public GameObject playerObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Extra " + this + " deleted");
        }

        DisableScreens();

        upgradeConfig.Initialize();
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                UpdateStopWatch();
                break;
            case GameState.Paused:
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    Debug.Log("Game over");
                    DisplayResults();
                }
                break;
            case GameState.LevelUp:
                if (!isChoosingUpgrades)
                {
                    isChoosingUpgrades = true;
                    Time.timeScale = 0f;
                    Debug.Log("Upgrading screen activated");
                    levelUpScreen.SetActive(true);
                }
                break;
            default:
                Debug.LogWarning("State doesn't exist");
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            Debug.Log("Game is paused");
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            Debug.Log("Game is resumed");
        }
    }

    private void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);
    }

    private void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.CharacterName;
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignSpriteUI(List<Image> list, AYellowpaper.SerializedCollections.SerializedDictionary<Image, string> dictionary)
    {
        if (dictionary.Count != list.Count)
        {
            Debug.Log("Count of weapon dictionary and list is different");
            return;
        }

        int slotIndex = 0;

        foreach (var pair in dictionary)
        {
            if (slotIndex >= list.Count)
            {
                break;
            }
            if (pair.Key.sprite != null)
            {
                list[slotIndex].sprite = pair.Key.sprite;
                list[slotIndex].enabled = true;
                slotIndex++;
            }
        }
    }

    private void UpdateStopWatch()
    {
        stopwatchTime += Time.deltaTime;

        UpdateStopWatchDisplay();

        if (stopwatchTime > timeLimit)
        {
            playerObject.SendMessage("Kill");
        }
    }

    private void UpdateStopWatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        isChoosingUpgrades = true;
        levelUpScreen.SetActive(true);
        Time.timeScale = 0f;
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp()
    {
        isChoosingUpgrades = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }
}
