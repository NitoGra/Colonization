using System;
using DG.Tweening;
using UnityEngine;

internal class Bot : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    public BotState BotState { get; private set; } = BotState.Idle;
    private Gold _targetGold;

    private void OnTriggerEnter(Collider other)
    {    
        if (other.TryGetComponent(out Gold gold))
        {
            if (BotState == BotState.DontHaveGold)
            {
                if (gold.IsTaken == false && gold == _targetGold)
                {
                    BotState = BotState.HaveGold;
                    gold.Take(transform);
                }
            }
        }
        if (other.TryGetComponent(out Base findBase))
        {
            if (BotState == BotState.HaveGold)
            {
                ReturnToBase(findBase);
            }
        }
    }

    public void MoveToGetGold(Gold gold)
    {
        BotState = BotState.DontHaveGold;
        _targetGold = gold;
        transform.DOMove(_targetGold.transform.position, _speed).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }

    private void ReturnToBase(Base findBase)
    {
        _targetGold.gameObject.SetActive(false);
        Debug.Log(_targetGold.gameObject.GetInstanceID() + " Gold return");
        findBase.BotReturnedToBase(_targetGold);
        _targetGold = null;
        BotState = BotState.Idle;
    }
}

internal enum BotState 
{
    Idle,
    DontHaveGold,
    HaveGold
}