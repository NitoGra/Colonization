using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
internal class Scanner
{
    [SerializeField] private float _scanDelayInSeconds = 0.3f;
    [field: SerializeField] public float Radius { get; private set; } = 20;
    [SerializeField] private LayerMask _targetMask;
    public event Action<Collider[]> Scanned;
    
    public async void BackgroundScan(Transform transform)
    {
        while (transform.gameObject.activeSelf)
        {
            Scanned?.Invoke(Scan(transform));
            await UniTask.WaitForSeconds(_scanDelayInSeconds);
        }
    }
    
    public Collider[] Scan(Transform transform, float radius = 0) => 
        Physics.OverlapSphere(transform.position, radius == 0 ? Radius : radius, _targetMask);

    public void DisplayScanRadius(Transform transform)
    {   
    #if UNITY_EDITOR
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, Radius);
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, Radius);
    #endif
    }
}