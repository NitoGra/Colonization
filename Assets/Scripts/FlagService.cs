using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
internal class FlagService
{
    [SerializeField] private Flag _flag;
    [SerializeField] private Base _basePrefab;
    
    public Transform GetFlagTransform => _flag.transform;
    public Action GetMoneyToNewBase;
    
    private Material _material;
    private Color _clickedColor = Color.blue;
    private Color _normalColor = Color.darkSlateGray;

    public void SetMaterial(Material material) => _material = material;
    
    public async void BaseClick(int iD)
    {
        FloorClickDetector.BaseClick(iD);
        _material.color = _clickedColor;
        await UniTask.WaitUntil(() => FloorClickDetector.ClickPosition != Vector3.zero);
        _material.color = _normalColor;

        if (FloorClickDetector.BaseId != iD)
            return;

        _flag.gameObject.SetActive(true);
        
        _flag.transform.position = new Vector3(
            FloorClickDetector.ClickPosition.x, 
            0, 
            FloorClickDetector.ClickPosition.z);
        
        GetMoneyToNewBase.Invoke();
    }

    public Base BuildNewBase()
    {  
        _flag.gameObject.SetActive(false);
        return MonoBehaviour.Instantiate(_basePrefab, _flag.transform.position, _flag.transform.rotation);
    }
}