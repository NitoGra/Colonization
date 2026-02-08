using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

internal class Base : MonoBehaviour, IPointerClickHandler
{
    private const float ClearBaseRadius = 0.5f;
    private const int CreateBotCost = 3;
    private const int CreateBaseCost = 5;

    [SerializeField] private Scanner _scanner;
    [SerializeField] private BotListService _botListService;
    [SerializeField] private ModelGoldKeeper _modelGoldKeeper;
    [SerializeField] private GoldView _goldView;
    [SerializeField] private FlagService _flagService;

    private ModelType _modelType;

    private bool CanBuild => _modelType == ModelType.CreateBase && _modelGoldKeeper.Gold >= CreateBaseCost &&
                             _botListService.GetCount > 1;

    private bool CanCreateBot => (_botListService.GetCount <= 1 || _modelType == ModelType.CreateBot) &&
                                 _modelGoldKeeper.Gold >= CreateBotCost;

    private void Awake()
    {
        _botListService.Clear();

        ClearGold();
        _scanner.Scanned += FindGold;

        _modelGoldKeeper.Init(_goldView.GoldDisplay);
        _flagService.GotMoneyToNewBase += () => _modelType = ModelType.CreateBase;
        _flagService.SetMaterial(GetComponent<Renderer>().material);
        _flagService.Enable();

        for (int i = 0; i < _botListService.StartBotCount; i++)
            _botListService.CreateNewBot(this);

        _scanner.BackgroundScan(transform);
    }

    public void OnPointerClick(PointerEventData eventData) => _flagService.BaseClick(gameObject.GetEntityId());

    public void ReturnBot()
    {
        _modelGoldKeeper.IncreaseGold();

        if (CanBuild)
        {
            SendBotCreateNewBase(_flagService.GetFlagTransform);
            _modelType = ModelType.CreateBot;
        }
        else if (CanCreateBot)
        {
            _modelGoldKeeper.DecreaseGold(CreateBotCost);
            _botListService.CreateNewBot(this);
        }
    }

    public void AddBot(Bot bot) => _botListService.Add(bot);

    public void OnDrawGizmos() => _scanner.DisplayScanRadius(transform);

    private void FindGold(Collider[] colliders)
    {
        foreach (var goldCollider in colliders)
        {
            if (goldCollider.TryGetComponent(out Gold gold) == false)
                continue;

            if (gold.State != GoldState.Idle)
                continue;

            if ((gold.transform.position - transform.position).magnitude > _scanner.Radius)
                continue;

            SendBotToGold(gold);
        }
    }

    private void SendBotToGold(Gold gold)
    {
        if (_botListService.TryGetBot(out Bot bot))
        {
            gold.SetTarget();
            bot.MoveToGetGold(gold);
        }
    }

    private void ClearGold()
    {
        foreach (var goldCollider in _scanner.Scan(transform, ClearBaseRadius))
        {
            if (goldCollider.TryGetComponent(out Gold gold) == false)
                continue;

            if (gold.State != GoldState.Idle)
                continue;

            if ((gold.transform.position - transform.position).magnitude > _scanner.Radius)
                continue;

            gold.gameObject.SetActive(false);
        }
    }

    private async void SendBotCreateNewBase(Transform basePosition)
    {
        Bot newBot = await _botListService.GetBotToCreateBase();

        _modelGoldKeeper.DecreaseGold(CreateBaseCost);
        _botListService.Remove(newBot);
        newBot.MoveToCreateBase(basePosition, _flagService.BuildNewBase);

        _flagService.Disable();
        float cantMoveFlagInSeconds = (basePosition.position - newBot.transform.position).magnitude / newBot.GetSpeed;
        await UniTask.WaitForSeconds(cantMoveFlagInSeconds);
        _flagService.Enable();
    }
}