using UnityEngine;
using UnityEngine.EventSystems;

internal class Floor: MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        FloorClickDetector.FloorClick(eventData.pointerCurrentRaycast.worldPosition);
    }
}