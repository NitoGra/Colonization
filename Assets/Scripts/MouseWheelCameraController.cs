using UnityEngine;
using UnityEngine.InputSystem;

public class MouseWheelCameraController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _smoothTime = 0.15f;
    [SerializeField] private bool _invertScroll = false;

    [SerializeField] private float _minY = 10;
    [SerializeField] private float _maxY = 100;

    private Vector3 _velocity = Vector3.zero;
    private float _targetYPosition;

    private void Start()
    {
        _targetYPosition = transform.position.y;
    }

    private void Update()
    {
        float scrollInput = Mouse.current.scroll.y.ReadValue();

        if (scrollInput != 0)
        {
            float direction = _invertScroll ? -1f : 1f;
            float positionChange = scrollInput * direction * _moveSpeed;
            _targetYPosition = Mathf.Clamp(_targetYPosition + positionChange, _minY, _maxY);
        }

        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = new Vector3(currentPosition.x, _targetYPosition, currentPosition.z);
        transform.position = Vector3.SmoothDamp(currentPosition, targetPosition, ref _velocity, _smoothTime);
    }
}