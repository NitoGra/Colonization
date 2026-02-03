using System;
using Vector3 = UnityEngine.Vector3;

public interface ISpawnable
{
    public Action<Vector3> Released { get; set; }
    public void Spawn(Vector3 position = default);
}