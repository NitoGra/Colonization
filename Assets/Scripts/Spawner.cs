using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Spawner<T> where T : MonoBehaviour, ISpawnable
{
    private readonly float _spawnDelayInSeconds;
    private readonly Pool _items;
    private readonly Action<Vector3> _itemSpawned;

    public Spawner(T prefab, float spawnDelayInSeconds = 0.01f, Action<Vector3> itemSpawned = null)
    {
        _items = new Pool(prefab);
        _spawnDelayInSeconds = spawnDelayInSeconds;
        _itemSpawned = itemSpawned;
    }

    public async void SpawnObjects()
    {
        while (true)
        {
            Spawn();
            await UniTask.WaitForSeconds(_spawnDelayInSeconds);
        }
    }

    public void Spawn(Vector3 position = default)
    {
        (T item, bool isCreated) result = ((T, bool))_items.Get();
        
        result.item.Spawn(position);
        result.item.Released = _itemSpawned;
    }
}