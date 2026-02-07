using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

internal class Base : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private BotListService _botListService;
    [SerializeField] private ModelGoldKeeper _modelGoldKeeper;
    [SerializeField] private WeaverGold _weaverGold;
    [SerializeField] private FlagService _flagService;

    private void Awake()
    {
        _botListService.Clear();
        _scanner.Scan(transform, SendBotOnGold);
        
        _modelGoldKeeper.GoldChanged += _weaverGold.GoldDisplay;
        _modelGoldKeeper.OnCreateNewBase += () => SendBotCreateNewBase(_flagService.GetFlagTransform);
        _modelGoldKeeper.OnCreateNewBot += () => _botListService.CreateNewBot(this);
        _modelGoldKeeper.CheckBotsCount += () => _botListService.GetCount;
        
        _flagService.GetMoneyToNewBase += () => _modelGoldKeeper.ChangeBaseType();
        _flagService.SetMaterial(GetComponent<Renderer>().material);
        _flagService.Enable();

        for (int i = 0; i < _botListService.StartBotCount; i++)
            _botListService.CreateNewBot(this);
    }

    public void OnPointerClick(PointerEventData eventData) => _flagService.BaseClick(gameObject.GetEntityId());

    public void BotReturnedToBase() => _modelGoldKeeper.IncreaseGold();

    public void AddBot(Bot bot) => _botListService.Add(bot);

    public void OnDrawGizmos() => _scanner.DisplayScanRadius(transform);

    private void SendBotOnGold(Gold gold)
    {
        if (_botListService.TryGetBot(out Bot bot))
        {
            gold.SetTarget();
            bot.MoveToGetGold(gold);
        }
    }

    private async void SendBotCreateNewBase(Transform basePosition)
    {
        Bot newBot = await _botListService.GetBotToCreateBase();
        _botListService.Remove(newBot);
        newBot.MoveToCreateBase(basePosition, _flagService.BuildNewBase);
        _flagService.Disable();
        float timeCantMoveFlag = (basePosition.position - newBot.transform.position).magnitude / newBot.GetSpeed;
        await UniTask.WaitForSeconds(timeCantMoveFlag);
        _flagService.Enable();
    }
}