using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // For LayoutRebuilder

public class DropZone : MonoBehaviour, IDropHandler
{
    public RecycleItemType acceptedItemType; // What type this zone accepts (mostly for recycling bins)

    [Header("Zone Type")]
    public TextMeshProUGUI ItemName;
    public bool isRecyclingBin = false; // Is this a bin that destroys items
    public bool isCraftingInputSlot = false; // Is this a slot for crafting inputs
    public bool isGenericInventorySlot = false; // Is this a general inventory grid slot

    // If it's a generic inventory slot, reference its container (the GridLayoutGroup parent)
    public Transform inventoryGridContainer; // This will likely be the InventoryPanel

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
                    // The DraggableUIItem's OnEndDrag will handle snapping back if it came from inventory.
                    // If it came from a crafting slot, it will snap back to that crafting slot.
                }
            }
            else if (isCraftingInputSlot)
            {
                Debug.Log("Item (" + droppedItem.gameObject.name + ") dropped into crafting input slot.");
                ItemName.text= droppedItem.gameObject.name;
                // --- NEW LOGIC FOR SWAPPING ---
                if (transform.childCount > 0)
                {
                    // There's already an item in this slot.
                    DraggableUIItem existingItem = transform.GetChild(0).GetComponent<DraggableUIItem>();
                    if (existingItem != null)
                    {
                        Debug.Log($"Crafting slot {name} already contains {existingItem.gameObject.name}. Swapping it out.");

                        // Find the InventoryPanel's transform to send the existing item back to.
                        // We need to ensure we have this reference.
                        Transform inventoryPanelTransform = GameObject.Find("InventoryPanel")?.transform;

                        if (inventoryPanelTransform != null)
                        {
                            existingItem.SnapToParent(inventoryPanelTransform);
                        }
                        else
                        {
                            Debug.LogWarning("InventoryPanel not found for swapping. Existing item will remain in slot.");
                            // If we can't send it back, don't let the new item replace it.
                            return; // Abort the drop of the new item
                        }
                    }
                }
                // --- END NEW LOGIC ---

                // Now, place the new item into this slot.
                droppedItem.SnapToParent(this.transform); // Snap to this slot's transform
            }
            else if (isGenericInventorySlot)
            {
                // For generic inventory grid slots (like the main inventory panel itself)
                Debug.Log("Item (" + droppedItem.gameObject.name + ") dropped into generic inventory.");
                // Check if the slot is full (if it's a single slot in an inventory, not the grid container itself)
                if (transform.childCount > 0 && this.GetComponent<GridLayoutGroup>() == null) // If it's a single slot that might have an item
                {
                    // For a proper inventory system with individual slots, you'd handle swapping here too.
                    // For our current GridLayoutGroup where items just go into the parent, this might not be needed.
                    Debug.LogWarning("Generic inventory slot already has an item. Current implementation allows stacking within GridLayoutGroup.");
                    // In a true slot-based inventory, you'd send the existing item back or prevent drop.
                }
                droppedItem.SnapToParent(inventoryGridContainer != null ? inventoryGridContainer : this.transform);
            }
            // If no specific zone type is set, it behaves like a regular UI element not handling drops beyond initial drag
        }
    }
}