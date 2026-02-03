using System;
using UnityEngine;
using Random = UnityEngine.Random;

internal class Gold : MonoBehaviour, ISpawnable
{    
    [SerializeField] private float _spawnPositionX;
    [SerializeField] private float _spawnPositionZ;
    [SerializeField] private float _spawnPositionY;
    private Vector3 SpawnPosition => new (Random.Range(-_spawnPositionX, _spawnPositionX), _spawnPositionY, Random.Range(-_spawnPositionZ, _spawnPositionZ));
    public bool IsTaken { get; private set; } = false;
    public Action<Vector3> Released { get; set; }

    public void Take(Transform parent)
    {
        transform.parent = parent;
        IsTaken = true;
        Debug.Log(gameObject.GetInstanceID() + " Gold Take");
    }
    
    public void Spawn(Vector3 position = default)
    {
        gameObject.SetActive(true);
        IsTaken = false;
        transform.parent = null;
        transform.position = SpawnPosition;
    }
}