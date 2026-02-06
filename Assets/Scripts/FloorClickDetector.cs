using UnityEngine;

internal static class FloorClickDetector
{
    public static Vector3 ClickPosition {get; private set; }
    public static int BaseId {get; private set; }

    public static void FloorClick(Vector3 clickPosition) => ClickPosition = clickPosition;

    public static void BaseClick(int baseId)
    {
        BaseId = baseId;
        ClickPosition = Vector3.one;
        ClickPosition = Vector3.zero;
    }
}