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

    public async void Scan(Transform transform, Action<Gold> goldFind)
    {
        while (transform.gameObject.activeSelf)
        {
            foreach (var goldCollider in Physics.OverlapSphere(transform.position, _radius, _goldLayerMask))
            {
                if (goldCollider.TryGetComponent(out Gold gold) == false)
                    continue;

                if (gold.State != Gold.GoldState.Idle)
                    continue;

                if ((gold.transform.position - transform.position).magnitude > _radius)
                    continue;

                goldFind.Invoke(gold);
            }
            
            await UniTask.WaitForSeconds(_scanDelayInSeconds);
        }
    }
    
    public void DisplayScanRadius(Transform transform)
    {   
    #if UNITY_EDITOR
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, _radius);
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, _radius);
    #endif
    }
}