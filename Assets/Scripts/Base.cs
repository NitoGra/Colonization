using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal class Base: MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private BotListService _botListService;
    [SerializeField] private ModelGoldKeeper _modelGoldKeeper;
    [SerializeField] private WeaverGold _weaverGold;

    private void Start()
    {
        _scanner.Scan(transform,SendBotOnGold);
        _modelGoldKeeper.GoldChanged += _weaverGold.GoldDisplay;
    }

    private void SendBotOnGold(Gold gold)
    {
        if (_botListService.TryGetBot(out Bot bot))
        {
            _scanner.SetTargetGold(gold);
            bot.MoveToGetGold(gold);
        }
    }

    public void BotReturnedToBase(Gold gold)
    {
        _scanner.GoldTaken(gold);
        _modelGoldKeeper.IncreaseGold();
    }
}

[Serializable]
internal class WeaverGold
{
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    public void GoldDisplay(int gold)
    {
        _textMeshProUGUI.text =$"{gold}g";
    }
}

[Serializable]
internal class ModelGoldKeeper
{
    private int _gold = 0;
    public Action<int> GoldChanged;

    public void IncreaseGold()
    {
        _gold++;
        GoldChanged?.Invoke(_gold);
    }
}

[Serializable]
internal class BotListService
{
    [SerializeField] private List<Bot> _botsList;

    public bool TryGetBot(out Bot bot)
    {
        bot = _botsList.Find(t => t.BotState == BotState.Idle);
        return bot !=  null;
    }
}