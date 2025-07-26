using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Eco Innovator Lab/Quest Data", order = 2)]
public class QuestData : ScriptableObject
{
    [TextArea(3, 5)] // Makes the string field a multi-line text area in the Inspector
    public string problemStatement; // The text presented to the player

    public GameObject requiredInventionPrefab; // The prefab of the crafted item needed to solve this quest
}