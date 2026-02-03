using System;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private readonly Func<MonoBehaviour> _create;
    private readonly List<MonoBehaviour> _items;
        
    public Pool(MonoBehaviour prefab)
    {
        _items = new List<MonoBehaviour>();
        _create = () => Create(prefab);
    }

    private MonoBehaviour Create(MonoBehaviour prefab)
    {
        MonoBehaviour item = MonoBehaviour.Instantiate(prefab);
        _items.Add(item);
        return item;
    }

    public (MonoBehaviour, bool) Get()
    {
        var find = _items.Find(item => item.gameObject.activeSelf == false);
        return find is not null ? (find, false) : (_create.Invoke(), true);
    }
}