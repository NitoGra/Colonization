using System;

[Serializable]
internal class ModelGoldKeeper
{
    private event Action<int> GoldChanged;
    public int Gold { get; private set; }

    public void Init(Action<int> onGoldChanged)
    {
        Gold = 0;
        GoldChanged += onGoldChanged;
        GoldChanged?.Invoke(Gold);
    }

    public void IncreaseGold(int value = 1)
    {
        Gold += value;
        GoldChanged?.Invoke(Gold);
    }

    public void DecreaseGold(int value)
    {
        Gold -= value;
        GoldChanged?.Invoke(Gold);
    }
}