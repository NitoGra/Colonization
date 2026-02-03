using Unity.VisualScripting;
using UnityEngine;

public class Boostrap : MonoBehaviour
{
    [SerializeField] private float _spawnDelayInSeconds = 0.1f;
    [Space]
    [SerializeField] private Bot _prefabBot;
    [SerializeField] private Gold _prefabGold;
    [SerializeField] private Base _prefabBase;
    [Space]
    [SerializeField] private Base _startBase;
    private Spawner<Gold> _spawnerGold;

    private void Awake()
    {
        _spawnerGold = new Spawner<Gold>(_prefabGold, _spawnDelayInSeconds);
        _spawnerGold.SpawnObjects();
    }
}