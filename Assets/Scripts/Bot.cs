using System;
using DG.Tweening;
using UnityEngine;

internal class Bot : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    public float GetSpeed => _speed;
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
                    TakeGold(gold);
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
        _targetObject = gold.gameObject;
        MoveTo(gold.transform);
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

    private void MoveTo(Transform target)
    {
        transform.DOMove(target.transform.position, _speed).SetSpeedBased().SetEase(Ease.Linear);
#if UNITY_EDITOR
        Debug.DrawLine(transform.position, target.transform.position, Color.black,
            (target.transform.position - transform.position).magnitude / _speed);
#endif
    }

    private void TakeGold(Gold gold)
    {
        if (gold.State == GoldState.OnTarget && gold.gameObject.GetEntityId() == _targetObject.GetEntityId())
        {
            BotState = BotState.TookGold;
            gold.WeTaken(transform);
            MoveTo(_baseTransform);
        }
    }

    private void ReturnToBase(Base findBase)
    {
        findBase.ReturnBot();
        _targetObject.SetActive(false);
        _targetObject = null;
        BotState = BotState.Idle;
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