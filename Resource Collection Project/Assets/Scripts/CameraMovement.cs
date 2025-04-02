using Unity.Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private float speedScalePC = 20f; 
    [SerializeField] private float speedScaleTouch = 4f; 
    [SerializeField] private float clampArea;
    [SerializeField] private CinemachineCamera cinemachineVirtualCamera;
    
    private bool _dragMoveActive;
    private Vector2 _lastMousePosition;
    private float _dragSpeed;
    private float _fieldOfView;
    private float _startDragSpeed;
    private float _speedScale;
    private bool _moveBlock;
    private bool _zooming;
    private float _currentZoomVelocity;

    private void OnEnable() => GameEventManager.OnCameraMoverState += UpdateMoveBlock;
    private void OnDisable() => GameEventManager.OnCameraMoverState -= UpdateMoveBlock;

    private void Start()
    {
        UpdateDragSpeed();
        _fieldOfView = cinemachineVirtualCamera.Lens.FieldOfView;
    }

    private void Update()
    {
        if (!_moveBlock && !_zooming) Movement();
        Zoom();
    }

    private void UpdateDragSpeed()
    {
        _dragSpeed = 0.1f * (Screen.width * Screen.height) / (1920f * 1080f);
        _startDragSpeed = _dragSpeed;
    }

    private void UpdateMoveBlock(bool state) => _moveBlock = state;

    private void Zoom()
    {
        float zoomSpeed = 20f;  
        float smoothTime = 0.15f;

        if (Input.touchCount == 2)
        {
            Touch first = Input.GetTouch(0);
            Touch second = Input.GetTouch(1);
            
            float previousDistance = (first.position - first.deltaPosition - (second.position - second.deltaPosition)).magnitude;
            float currentDistance = (first.position - second.position).magnitude;

            float difference = Mathf.Clamp(currentDistance - previousDistance, -20f, 20f);
            float targetFov = _fieldOfView - difference * 0.2f;

            _fieldOfView = Mathf.SmoothDamp(_fieldOfView, targetFov, ref _currentZoomVelocity, smoothTime);
            _zooming = true;
            _speedScale = speedScaleTouch;
            _dragMoveActive = false; 
        }
        else if (Input.mouseScrollDelta.y != 0)
        {
            _fieldOfView -= Input.mouseScrollDelta.y * zoomSpeed;
            _zooming = true;
            _speedScale = speedScalePC;
        }
        else
        {
            _zooming = false;
        }

        _fieldOfView = Mathf.Clamp(_fieldOfView, 20, 60);
        cinemachineVirtualCamera.Lens.FieldOfView = Mathf.Lerp(
            cinemachineVirtualCamera.Lens.FieldOfView, 
            _fieldOfView, 
            Time.deltaTime * _speedScale
        );
    }
    
    private void Movement()
    {
        if (Input.touchCount == 2) return; 

        float newSpeed = speed;
        Vector3 inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        if (Input.touchCount == 1 && Input.GetMouseButtonDown(0))
        {
            _dragMoveActive = true;
            _lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0)) _dragMoveActive = false;

        _dragSpeed = _startDragSpeed;

        if (_dragMoveActive)
        {
            Vector2 delta = (Vector2)Input.mousePosition - _lastMousePosition;
            inputDir.x = -delta.x * _dragSpeed;
            inputDir.z = -delta.y * _dragSpeed;
            _lastMousePosition = Input.mousePosition;
            newSpeed = 5;
        }

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        Vector3 newPosition = transform.position + moveDir * newSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -clampArea, clampArea);
        newPosition.z = Mathf.Clamp(newPosition.z, -clampArea, clampArea);
        transform.position = newPosition;
    }
}
