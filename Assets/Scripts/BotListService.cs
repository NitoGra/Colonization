using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
internal class BotListService
{
    [field: SerializeField] public int StartBotCount {get; private set; }
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private List<Bot> _botsList;
    
    public bool TryGetBot(out Bot bot)
    {
        bot = _botsList.Find(t => t.BotState == BotState.Idle);
        return bot != null;
    }

    public async UniTask<Bot> GetBotToCreateBase()
    {
        Bot bot = null;
        await UniTask.WaitUntil(() => TryGetBot(out bot));
        return bot;
    }

    public int GetCount => _botsList.Count;
    
    public void Clear() => _botsList.Clear();
    public void Add(Bot bot) => _botsList.Add(bot);
    public void Remove(Bot bot) => _botsList.Remove(bot);

    public void CreateNewBot(Base newBase)
    {
        var newBot = MonoBehaviour.Instantiate(_botPrefab);
        newBot.SetBase(newBase);
    }
}