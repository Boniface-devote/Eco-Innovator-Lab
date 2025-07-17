using UnityEngine;
using UnityEngine.EventSystems; // Required for IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
using UnityEngine.UI; // Needed for LayoutRebuilder

public class DraggableUIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    [HideInInspector] public Transform originalParent;
    public Vector2 originalPosition; // To store the starting position for snapping back

    public RecycleItemType itemType;

    // --- NEW for Double-Click ---
    private float lastClickTime = 0f;
    private const float DOUBLE_CLICK_TIME_THRESHOLD = 0.3f; // Time in seconds between clicks for a double-click

    private Transform inventoryPanelTransform; // Reference to the InventoryPanel
    // ----------------------------

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // --- Find the InventoryPanel dynamically ---
        GameObject inventoryPanelGO = GameObject.Find("InventoryPanel"); // Use the exact name of your InventoryPanel GameObject
        if (inventoryPanelGO != null)
        {
            inventoryPanelTransform = inventoryPanelGO.transform;
        }
        else
        {
            Debug.LogError("InventoryPanel not found! Double-click return will not work.", this);
        }
        // ------------------------------------------
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag: " + gameObject.name);
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition; // Store current position before dragging
        transform.SetParent(transform.root); // Set parent to Canvas root (so it draws on top)
        transform.SetAsLastSibling(); // Ensure it's the last sibling to be drawn on top of others

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Use eventData.delta and the Canvas's scaleFactor for consistent dragging across resolutions
        rectTransform.anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag: " + gameObject.name);
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Check if the item originated from the InventoryPanel
        bool originatedFromInventory = (originalParent == inventoryPanelTransform);

        // Get the object we released the drag over
        IDropHandler dropHandler = eventData.pointerEnter?.GetComponent<IDropHandler>();
        DropZone dropZone = dropHandler as DropZone; // Try to cast it to our specific DropZone type

        bool wasDroppedOnValidCraftingSlot = false;

        if (dropZone != null)
        {
            // If originated from inventory, ONLY allow dropping into a crafting input slot
            if (originatedFromInventory)
            {
                if (dropZone.isCraftingInputSlot)
                {
                    wasDroppedOnValidCraftingSlot = true;
                    Debug.Log($"Item {gameObject.name} (from Inventory) dropped on valid Crafting Input Slot: {dropZone.name}");
                }
                else
                {
                     Debug.Log($"Item {gameObject.name} (from Inventory) dropped on invalid zone ({dropZone.name}). Returning to inventory.");
                    SnapToParent(inventoryPanelTransform);
                }
            }
            else // Item originated from somewhere else (e.g., a crafting slot, or not inventory)
            {
                 Debug.Log($"Item {gameObject.name} (from {originalParent.name}) dropped on {dropZone.name}. Let DropZone handle.");
                }
        }
 if (originatedFromInventory && !wasDroppedOnValidCraftingSlot)
        {
            // Only snap back if it's still parented to the canvas root (meaning it wasn't handled by a DropZone already)
            if (transform.parent == transform.root)
            {
                Debug.Log($"Item {gameObject.name} (from Inventory) was not dropped on a valid crafting slot or any other handler. Snapping back to Inventory.");
                SnapToParent(inventoryPanelTransform);
            }
        }
        else if (!originatedFromInventory && transform.parent == transform.root)
        {
             Debug.Log($"Item {gameObject.name} (from {originalParent.name}) was not dropped on a valid zone. Snapping back to original parent.");
            SnapToParent(originalParent); // Snap back to its original parent (which would be a crafting slot)
        }
         }

    // --- NEW METHOD: OnPointerClick for Double-Click Detection ---
    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if it's a double-click
        if ((Time.time - lastClickTime) < DOUBLE_CLICK_TIME_THRESHOLD)
        {
            Debug.Log("Double Clicked: " + gameObject.name);
            ReturnToInventory();
        }
        lastClickTime = Time.time;
    }

    // --- EXISTING SnapToParent Method ---
    public void SnapToParent(Transform newParent, Vector2? localPosition = null)
    {
        transform.SetParent(newParent);
        if (localPosition.HasValue)
        {
            rectTransform.anchoredPosition = localPosition.Value;
        }
        else
        {
             rectTransform.anchoredPosition = Vector2.zero;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(newParent.GetComponent<RectTransform>());
    }

    // --- NEW METHOD: To handle returning the item to inventory ---
    private void ReturnToInventory()
    {
          DropZone currentParentDropZone = transform.parent?.GetComponent<DropZone>();

        if (currentParentDropZone != null && currentParentDropZone.isCraftingInputSlot)
        {
            if (inventoryPanelTransform != null)
            {
                Debug.Log("Returning " + gameObject.name + " to Inventory.");
                SnapToParent(inventoryPanelTransform); // Use our existing SnapToParent method
            }
            else
            {
                Debug.LogWarning("InventoryPanel reference missing, cannot return item.");
            }
        }
        else if (inventoryPanelTransform != null && transform.parent != inventoryPanelTransform)
        {
             Debug.Log("Item not in a crafting input slot, but not in inventory. Returning to inventory.");
            SnapToParent(inventoryPanelTransform);
        }
        else
        {
            Debug.Log("Item is already in inventory or not in a place from which it should double-click return.");
        }
    }
}