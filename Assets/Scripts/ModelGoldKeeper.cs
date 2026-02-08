using System;

[Serializable]
internal class ModelGoldKeeper
{
    private const int CreateBotCost = 3;
    private const int CreateBaseCost = 5;

    private int _gold = 0;
    private ModelType _model;
    private Func<int> _checkBotsCount;

    private Action<int> _goldChanged;
    private Action _onCreateNewBot;
    private Action _onCreateNewBase;

    public void Init(Action<int> goldChanged, Func<int> checkBotsCount,
        Action onCreateNewBase,
        Action onCreateNewBot)
    {
        _checkBotsCount = checkBotsCount;
        _model = ModelType.CreateBot;
        _goldChanged += goldChanged;
        _onCreateNewBase += onCreateNewBase;
        _onCreateNewBot += onCreateNewBot;

        _gold = 0;
        _goldChanged.Invoke(_gold);
    }

    public void IncreaseGold()
    {
        _gold++;
        int botCount = _checkBotsCount.Invoke();

        if (_model == ModelType.CreateBase && _gold >= CreateBaseCost && botCount > 1)
            CreateNewBase();
        else if ((botCount <= 1 || _model == ModelType.CreateBot) && _gold >= CreateBotCost)
            CreateNewBot();

        _goldChanged?.Invoke(_gold);
    }

    private void ChangeType(ModelType newType) => _model = newType;

    private void CreateNewBot()
    {
        _gold -= CreateBotCost;
        _onCreateNewBot.Invoke();
    }

    private void CreateNewBase()
    {
        _gold -= CreateBaseCost;
        _onCreateNewBase.Invoke();
        ChangeType(ModelType.CreateBot);
    }

    internal enum ModelType
    {
        CreateBot,
        CreateBase,
    }

    public void ChangeBaseType() => ChangeType(ModelType.CreateBase);
}