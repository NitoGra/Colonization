using System;
using UnityEngine;
using Random = UnityEngine.Random;

internal class Gold : MonoBehaviour, ISpawnable
{
    private const float Radius = 1.5f;

    [SerializeField] private float _spawnPositionX;
    [SerializeField] private float _spawnPositionZ;
    [SerializeField] private float _spawnPositionY;

    private Vector3 SpawnPosition => new(Random.Range(-_spawnPositionX, _spawnPositionX), _spawnPositionY,
        Random.Range(-_spawnPositionZ, _spawnPositionZ));

    public GoldState State { get; private set; }
    public Action<Vector3> Released { get; set; }

    public void Take(Transform parent)
    {
        transform.parent = parent;
        State = GoldState.Taken;
    }

    public void SetTarget() => State = GoldState.OnTarget;

    public void Spawn(Vector3 position = default)
    {
        transform.parent = null;
        transform.position = GetSpawnPositionWithoutBase();
        gameObject.SetActive(true);
        State = GoldState.Idle;
    }

    private Vector3 GetSpawnPositionWithoutBase()
    {
        Vector3 position = SpawnPosition;

        while (CheckBase(position) == false)
            position = SpawnPosition;

        return position;
    }

    private bool CheckBase(Vector3 position)
    {
        foreach (var goldCollider in Physics.OverlapSphere(position, Radius))
            if (goldCollider.TryGetComponent(out Base @base))
                return false;

        return true;
    }

    internal enum GoldState
    {
        Idle,
        OnTarget,
        Taken
    }
}