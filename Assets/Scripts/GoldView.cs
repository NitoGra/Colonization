using System;
using TMPro;
using UnityEngine;

[Serializable]
internal class GoldView
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    public void GoldDisplay(int gold) => _textMeshProUGUI.text =$"{gold}g";
}