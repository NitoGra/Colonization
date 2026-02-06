using System;

[Serializable]
internal class ModelGoldKeeper
{
    private const int BotCost = 3;
    private const int BaseCost = 5;

    private int _gold = 0;
    public Action<int> GoldChanged;
    public Action OnCreateNewBot;
    public Action OnCreateNewBase;
    private ModelType _model;
    public Func<int> CheckBotsCount;

    public void IncreaseGold()
    {
        _gold++;
        int botCount = CheckBotsCount.Invoke();
        
        if (_model == ModelType.CreateBase && _gold >= BaseCost && botCount > 1)
            CreateNewBase();
        else if ((botCount <= 1 || _model == ModelType.CreateBot) && _gold >= BotCost)
            CreateNewBot();

        GoldChanged?.Invoke(_gold);
    }
    
    private void ChangeType(ModelType newType) => _model = newType;

    private void CreateNewBot()
    {
        _gold -= BotCost;
        OnCreateNewBot.Invoke();
    }

    private void CreateNewBase()
    {
        _gold -= BaseCost;
        OnCreateNewBase.Invoke();
        ChangeType(ModelType.CreateBot);
    }
    
    internal enum ModelType
    {
        CreateBot,
        CreateBase,
    }

    public void ChangeBaseType() => ChangeType(ModelType.CreateBase);
}