using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI problemStatementText;

    [Header("Quests")]
    public List<QuestData> quests;
    private int currentQuestIndex = -1;
    [HideInInspector] public bool allQuestsCompleted = false; // To let ScoreManager know game is over

    [Header("Manager References")]
    public CraftingManager craftingManager;
    public ScoreManager scoreManager; // NEW: Reference to ScoreManager

    private void Start()
    {
        if (problemStatementText == null)
        {
            Debug.LogError("ProblemStatement_Text is not assigned in QuestManager!", this);
        }
        if (craftingManager == null)
        {
            Debug.LogError("CraftingManager is not assigned in QuestManager!", this);
        }
        // ... (existing null checks)
        if (scoreManager == null) Debug.LogError("ScoreManager is not assigned in QuestManager!", this);

        StartNextQuest();
    }

    public void OnItemCrafted(GameObject craftedItemPrefab)
    {
        if (currentQuestIndex < 0 || currentQuestIndex >= quests.Count || allQuestsCompleted)
        {
            Debug.Log("No active quest or all quests complete to evaluate craft.");
            return;
        }

        QuestData currentQuest = quests[currentQuestIndex];
        if (craftedItemPrefab == currentQuest.requiredInventionPrefab)
        {
            Debug.Log($"Quest Completed! Crafted: {craftedItemPrefab.name}");
            problemStatementText.text = $"<color=green>QUEST SOLVED!</color>\n<size=75%>You crafted the {craftedItemPrefab.name.Replace("_UI", "")}!</size>";

            scoreManager.AddScore(true); // Notify ScoreManager of correct craft

            // Delay displaying next quest to allow player to see "Quest Solved" message
            Invoke("AdvanceToNextQuest", 3f);
        }
        else
        {
            Debug.Log($"Crafted {craftedItemPrefab.name}, but current quest requires {currentQuest.requiredInventionPrefab.name}.");
            problemStatementText.text = $"<color=red>INCORRECT!</color>\n<size=75%>That wasn't the solution. Try again!</size>";

            scoreManager.AddScore(false); // Notify ScoreManager of incorrect craft

            // Clear "INCORRECT" message after a short delay, but don't advance quest
            Invoke("ClearProblemStatementText", 2f);
        }
    }

    private void AdvanceToNextQuest()
    {
        ClearProblemStatementText(); // Clear the "QUEST SOLVED" message
        StartNextQuest(); // Proceed to the next quest
    }

    private void StartNextQuest()
    {
        currentQuestIndex++;
        if (currentQuestIndex < quests.Count)
        {
            QuestData nextQuest = quests[currentQuestIndex];
            problemStatementText.text = $"<color=yellow>NEW CHALLENGE:</color>\n<size=90%>{nextQuest.problemStatement}</size>";
            Debug.Log($"New Quest Started: {nextQuest.problemStatement}");
        }
        else
        {
            problemStatementText.text = "<color=green>ALL QUESTS COMPLETE!</color>\n<size=75%>You have solved all the problems. Good job!</size>";
            Debug.Log("All quests completed!");
            allQuestsCompleted = true; // Mark as complete for ScoreManager
            // The ScoreManager will handle the final game end state (win/lose)
        }
    }

    private void ClearProblemStatementText()
    {
        problemStatementText.text = "";
    }
}