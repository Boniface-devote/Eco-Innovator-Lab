using UnityEngine;
using TMPro; // For TextMeshProUGUI
using System.Collections; // For Coroutines

public class ScoreManager : MonoBehaviour
{
    [Header("Score Settings")]
    public int correctCraftPoints = 5;
    public int incorrectCraftPenalty = -2;
    public int totalQuestsToComplete = 4; // Target number of quests to win

    [Header("Time Settings")]
    public float timeLimitPerQuest = 60f; // Time in seconds for each quest
    private float currentTimeRemaining;
    private bool timerIsRunning = false;

    [Header("UI References")]
    public TextMeshProUGUI scoreText; // To display current score
    public TextMeshProUGUI timerText; // To display time remaining
    public TextMeshProUGUI gameEndText; // To display win/lose message

    private int currentScore = 0;
    private int questsCompleted = 0;

    [Header("Manager References")]
    public QuestManager questManager;
    public CraftingManager craftingManager;
    public InventoryManager inventoryManager;

    private void Awake()
    {
        // Initialize UI
        UpdateScoreUI();
        gameEndText.gameObject.SetActive(false); // Hide game over text initially
    }

    private void Start()
    {
        // Ensure all managers are assigned
        if (questManager == null) Debug.LogError("QuestManager not assigned to ScoreManager!", this);
        if (craftingManager == null) Debug.LogError("CraftingManager not assigned to ScoreManager!", this);
        if (inventoryManager == null) Debug.LogError("InventoryManager not assigned to ScoreManager!", this);

        // Start the game by initializing the first quest's timer
        StartQuestTimer();
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (currentTimeRemaining > 0)
            {
                currentTimeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                currentTimeRemaining = 0;
                timerIsRunning = false;
                UpdateTimerUI();
                Debug.Log("Time's Up! Game Over.");
                EndGame(false); // Game Over - Lost
            }
        }
    }

    public void AddScore(bool correctCraft)
    {
        if (correctCraft)
        {
            currentScore += correctCraftPoints;
            questsCompleted++;
            Debug.Log($"Correct Craft! Score: {currentScore}");
            // Check for win condition after a correct craft
            if (questsCompleted >= totalQuestsToComplete)
            {
                EndGame(true); // Game Over - Won
            }
            else
            {
                // If not all quests are completed, restart timer for next quest
                StartQuestTimer();
            }
        }
        else
        {
            currentScore += incorrectCraftPenalty;
            Debug.Log($"Incorrect Craft! Score: {currentScore}");
            // No timer restart for incorrect crafts, player keeps trying on current quest
        }
        UpdateScoreUI();
    }

    private void StartQuestTimer()
    {
        currentTimeRemaining = timeLimitPerQuest;
        timerIsRunning = true;
        UpdateTimerUI(); // Update immediately to show full time
        Debug.Log($"Starting new quest timer for {timeLimitPerQuest} seconds.");
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // Format time as MM:SS
            int minutes = Mathf.FloorToInt(currentTimeRemaining / 60);
            int seconds = Mathf.FloorToInt(currentTimeRemaining % 60);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    private void EndGame(bool win)
    {
        timerIsRunning = false; // Stop the timer
        craftingManager.craftButton.interactable = false; // Disable craft button

        if (win)
        {
            gameEndText.text = $"<color=green>SUCCESS!</color>\n<size=75%>You solved {totalQuestsToComplete} problems!\nFinal Score: {currentScore}</size>";
            Debug.Log("Game Won!");
        }
        else
        {
            gameEndText.text = $"<color=red>GAME OVER!</color>\n<size=75%>Time ran out or quests not completed.\nFinal Score: {currentScore}</size>";
            Debug.Log("Game Lost!");
        }
        gameEndText.gameObject.SetActive(true); // Show game over text

        // Optionally, disable further interactions or offer restart
    }
}