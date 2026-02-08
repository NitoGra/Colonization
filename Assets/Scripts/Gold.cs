using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

internal class Gold : MonoBehaviour, ISpawnable
{
    private const float SpawnCheckRadius = 0.5f;

    [SerializeField] private float _spawnPositionX;
    [SerializeField] private float _spawnPositionZ;
    [SerializeField] private float _spawnPositionY;

    private Vector3 SpawnRandomPosition => new(Random.Range(-_spawnPositionX, _spawnPositionX), _spawnPositionY,
        Random.Range(-_spawnPositionZ, _spawnPositionZ));

    public GoldState State { get; private set; }
    public Action<Vector3> Released { get; set; }

    private void OnDisable() => Released?.Invoke(transform.position);

    public void WeTaken(Transform parent)
    {
        transform.parent = parent;
        State = GoldState.Taken;
    }

    public void SetTarget() => State = GoldState.OnTarget;

    public async void Spawn(Vector3 position = default)
    {
        transform.parent = null;
        transform.position = await GenerateSpawnPosition();
        gameObject.SetActive(true);
        State = GoldState.Idle;
    }

    private async UniTask<Vector3> GenerateSpawnPosition()
    {
        Vector3 position = Vector3.zero;
        await UniTask.WaitUntil(() => WaitPosition(out position));
        return position;
    }

    private bool WaitPosition(out Vector3 position)
    {
        do
            position = SpawnRandomPosition;
        while (CheckBaseExistence(position));
        
        return true;
    }

    private bool CheckBaseExistence(Vector3 position)
    {
        foreach (var goldCollider in Physics.OverlapSphere(position, SpawnCheckRadius))
            if (goldCollider.TryGetComponent(out Base @base))
                return true;

        return false;
    }
}