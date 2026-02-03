using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
internal class Scanner
{
    [SerializeField] private float _scanDelayInSeconds = 0.3f;
    [SerializeField] private float _radius = 20;
    [SerializeField] private LayerMask _goldLayerMask;
    
    private HashSet<Gold> _goldColliders;

    public async void Scan(Transform transform, Action<Gold> goldFind)
    {
        _goldColliders = new ();
        
        while (transform.gameObject.activeSelf)
        {
            foreach (var goldCollider in Physics.OverlapSphere(transform.position, _radius, _goldLayerMask))
            {
                if (goldCollider.TryGetComponent(out Gold gold))
                {
                    if(_goldColliders.TryGetValue(gold, out var goldPosition))
                        continue;
                
                    goldFind.Invoke(gold);
                }
            }
            
            await UniTask.WaitForSeconds(_scanDelayInSeconds);
        }
    }
    
    public void SetTargetGold(Gold gold) => _goldColliders.Add(gold);

    public void GoldTaken(Gold gold) => _goldColliders.Remove(gold);
}