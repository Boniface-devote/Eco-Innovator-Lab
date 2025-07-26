using System.Collections.Generic; // For List
using TMPro; // Required for TextMeshProUGUI
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI problemStatementText; // Assign your ProblemStatement_Text here

    [Header("Quests")]
    public List<QuestData> quests; // A list of all possible quests
    private int currentQuestIndex = -1; // To track the current quest

    [Header("Manager References")]
    public CraftingManager craftingManager; // Reference to your CraftingManager

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

        // Start the first quest
        StartNextQuest();
    }

    // Call this method whenever a new item is crafted by the CraftingManager
    public void OnItemCrafted(GameObject craftedItemPrefab)
    {
        if (currentQuestIndex < 0 || currentQuestIndex >= quests.Count)
        {
            Debug.Log("No active quest to complete.");
            return;
        }

        QuestData currentQuest = quests[currentQuestIndex];
        if (craftedItemPrefab == currentQuest.requiredInventionPrefab)
        {
            Debug.Log($"Quest Completed! Crafted: {craftedItemPrefab.name}");
            problemStatementText.text = $"<color=green>Quest Complete!</color>\n<size=75%>You crafted the {craftedItemPrefab.name.Replace("_UI", "")} and solved the problem!</size>";
            Invoke("ClearText", 10f); // Clear text after 10 seconds
            Invoke("StartNextQuest", 3f); // Start next quest after 3 seconds
        }
        else
        {
            Debug.Log($"Crafted {craftedItemPrefab.name}, but current quest requires {currentQuest.requiredInventionPrefab.name}.");
        }
    }

    private void StartNextQuest()
    {
        currentQuestIndex++;
        if (currentQuestIndex < quests.Count)
        {
            QuestData nextQuest = quests[currentQuestIndex];
            problemStatementText.text = $"<color=red>NEW CHALLENGE:</color>\n<size=100%>{nextQuest.problemStatement}</size>";
            Debug.Log($"New Quest Started: {nextQuest.problemStatement}");
            Invoke("ClearText", 10f); // Clear text after 10 seconds
        }
        else
        {
            problemStatementText.text = "<color=green>ALL QUESTS COMPLETE!</color>\n<size=75%>You have solved all the problems. Great job!</size>";
            Debug.Log("All quests completed!");
            Invoke("ClearText", 10f); // Clear text after 10 seconds
        }
    }

    private void ClearText()
    {
        problemStatementText.text = "";
    }
}