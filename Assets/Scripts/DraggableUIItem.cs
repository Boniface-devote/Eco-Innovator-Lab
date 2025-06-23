using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableUIItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    [HideInInspector] public Transform originalParent;
    public Vector2 originalPosition; // NEW: To store the starting position

    public RecycleItemType itemType;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag: " + gameObject.name);
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition; // Store current position
        transform.SetParent(transform.root); // Set parent to Canvas root
        transform.SetAsLastSibling();

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.root.localScale.x; // Use root scale for consistency
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag: " + gameObject.name);
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        IDropHandler dropHandler = eventData.pointerEnter?.GetComponent<IDropHandler>();

        if (dropHandler == null) // No valid drop handler found where it was released
        {
            // Option 1: Snap back immediately
            rectTransform.anchoredPosition = originalPosition;
            transform.SetParent(originalParent); // Return to original parent
            Debug.Log("No valid drop zone. Snapping back.");
        }
     }
    public void SnapToParent(Transform newParent, Vector2? localPosition = null)
    {
        transform.SetParent(newParent);
        if (localPosition.HasValue)
        {
            rectTransform.anchoredPosition = localPosition.Value;
        }
        else
        {
            // If no specific position given, set to zero, letting the layout group handle it
            rectTransform.anchoredPosition = Vector2.zero;
        }
        // Force a layout update if it's a layout group, to immediately re-arrange
        LayoutRebuilder.ForceRebuildLayoutImmediate(newParent.GetComponent<RectTransform>());
    }
}