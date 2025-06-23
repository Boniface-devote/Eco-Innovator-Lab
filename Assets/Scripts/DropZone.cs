using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // For LayoutRebuilder

public class DropZone : MonoBehaviour, IDropHandler
{
    public RecycleItemType acceptedItemType; // What type this zone accepts (mostly for recycling bins)

    [Header("Zone Type")]
    public bool isRecyclingBin = false; // Is this a bin that destroys items
    public bool isCraftingInputSlot = false; // Is this a slot for crafting inputs
    public bool isGenericInventorySlot = false; // Is this a general inventory grid slot

    // If it's a generic inventory slot, reference its container (the GridLayoutGroup parent)
    public Transform inventoryGridContainer;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item Dropped on " + gameObject.name);

        DraggableUIItem droppedItem = eventData.pointerDrag.GetComponent<DraggableUIItem>();

        if (droppedItem != null)
        {
            if (isRecyclingBin)
            {
                if (droppedItem.itemType == acceptedItemType)
                {
                    Debug.Log("Correct item (" + droppedItem.gameObject.name + " - " + droppedItem.itemType + ") dropped on " + gameObject.name + "!");
                    Destroy(droppedItem.gameObject); // Successfully recycled!
                }
                else
                {
                    Debug.Log("Incorrect item (" + droppedItem.gameObject.name + " - " + droppedItem.itemType + ") dropped on " + gameObject.name + ". Expected: " + acceptedItemType);
                    // Optionally: Return item to its original parent / position if incorrect
                    // droppedItem.SnapToParent(droppedItem.originalParent); // Would need original position stored
                }
            }
            else if (isCraftingInputSlot)
            {
                // Clear existing item in this slot before dropping new one
                if (transform.childCount > 0)
                {
                    Destroy(transform.GetChild(0).gameObject);
                }
                Debug.Log("Item (" + droppedItem.gameObject.name + ") dropped into crafting input slot.");
                droppedItem.SnapToParent(this.transform); // Snap to this slot's transform
            }
            else if (isGenericInventorySlot)
            {
                // For generic inventory grid slots (like the main inventory panel itself)
                Debug.Log("Item (" + droppedItem.gameObject.name + ") dropped into generic inventory.");
                droppedItem.SnapToParent(inventoryGridContainer != null ? inventoryGridContainer : this.transform);
            }
            // If no specific zone type is set, it behaves like a regular UI element not handling drops beyond initial drag
        }
    }
}