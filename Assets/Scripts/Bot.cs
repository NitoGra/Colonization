using System;
using DG.Tweening;
using UnityEngine;

internal class Bot : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    public BotState BotState { get; private set; } = BotState.Idle;
    private GameObject _targetObject;
    private Func<Base> _createBase;
    private Transform _baseTransform;
    
    private void OnTriggerEnter(Collider other)
    {

        switch (BotState)
        {
            case BotState.Idle:
                break;
            
            case BotState.TakesGold:
                if (other.TryGetComponent(out Gold gold))
                    GetGold(gold);
                break;
            
            case BotState.TookGold:
                if (other.TryGetComponent(out Base findBase))
                    ReturnToBase(findBase);
                break;
            
            case BotState.BaseCreator:
                if (other.TryGetComponent(out Flag flag))
                    CreateNewBase(flag);
                break;
        }
    }

    public void MoveToGetGold(Gold gold)
    {
        BotState = BotState.TakesGold;
        _targetObject =gold.gameObject;
        MoveTo(gold.transform);
    }

    private void MoveTo(Transform target)
    {
        transform.DOMove(target.transform.position, _speed).SetSpeedBased().SetEase(Ease.Linear);
    }

    private void GetGold(Gold gold)
    {
        if (gold.State == Gold.GoldState.OnTarget && gold.gameObject.GetEntityId() == _targetObject.GetEntityId())
        {
            BotState = BotState.TookGold;
            gold.Take(transform);
            MoveTo(_baseTransform);
        }
    }
    
    private void ReturnToBase(Base findBase)
    {
        findBase.BotReturnedToBase();
        _targetObject.SetActive(false);
        _targetObject = null;
        BotState = BotState.Idle;
    }
    
    public void MoveToCreateBase(Transform flagPosition, Func<Base> createBase)
    {
        BotState = BotState.BaseCreator;
        _createBase = createBase;
        _targetObject = flagPosition.gameObject;
        MoveTo(flagPosition);
    }

    public void SetBase(Base setBase)
    {
        transform.position = setBase.transform.position;
        _baseTransform = setBase.transform;
        setBase.AddBot(this);
    }
    
    private void CreateNewBase(Flag flag)
    {
        if (flag.gameObject.GetEntityId() == _targetObject.GetEntityId())
        {
            var newBase = _createBase.Invoke();
            BotState = BotState.Idle;
            SetBase(newBase);
        }
    }
}

internal enum BotState 
{
    Idle,
    TakesGold,
    TookGold,
    BaseCreator
}