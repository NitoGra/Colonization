using System;
using TMPro;
using UnityEngine;

[Serializable]
internal class WeaverGold
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    public void GoldDisplay(int gold) => _textMeshProUGUI.text =$"{gold}g";
}