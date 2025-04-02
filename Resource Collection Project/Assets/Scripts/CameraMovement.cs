using Unity.Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private float speedScalePC = 10f;
    [SerializeField] private float speedScaleTouch = 2f;
    [SerializeField] private float clampArea;
    [SerializeField] private CinemachineCamera cinemachineVirtualCamera;
    
    private bool _dragMoveActive;
    private Vector2 _lastMousePosition;
    private float _dragSpeed;
    private float _fieldOfView;
    private float _startDragSpeed;
    private float _speedScale;
    private bool _moveBlock;

    private void OnEnable()
    {
        GameEventManager.OnCameraMoverState += UpdateMoveBlock;
    }

    private void Start()
    {
        UpdateDragSpeed();
        _fieldOfView = 60f;
    }

    private void Update()
    {
        if(!_moveBlock) Movement();
        Zoom();
    }

    private void UpdateDragSpeed()
    {
        Vector2 baseScreen = new Vector2(1920, 1080);
        Vector2 screen = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        float baseDragSpeed = 0.1f;
        _dragSpeed = baseDragSpeed * (screen.x * screen.y) / (baseScreen.x * baseScreen.y);
        _startDragSpeed = _dragSpeed;
    }

    private void UpdateMoveBlock(bool state) => _moveBlock = state;

    private void Zoom()
    {
        // Mobile 
        if (Input.touchCount == 2)
        {
            Touch first = Input.GetTouch(0);
            Touch second = Input.GetTouch(1);
            Vector2 firstTouch = first.position - first.deltaPosition;
            Vector2 secondTouch = second.position - second.deltaPosition;
            float distanceTouch = (firstTouch - secondTouch).magnitude;
            float currentDistanceTouch = (first.position - second.position).magnitude;
            float difference = currentDistanceTouch - distanceTouch;
            _fieldOfView += difference;
            _speedScale = speedScaleTouch;
        }
        // PC
        else if (Input.mouseScrollDelta.y > 0)
        {
            _fieldOfView -= 5;
            _speedScale = speedScalePC;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            _fieldOfView += 5;
            _speedScale = speedScalePC;
        }

        _fieldOfView = Mathf.Clamp(_fieldOfView, 20, 60);
        Mathf.Lerp(cinemachineVirtualCamera.Lens.FieldOfView, _fieldOfView, Time.deltaTime * _speedScale);
        cinemachineVirtualCamera.Lens.FieldOfView = Mathf.Lerp(cinemachineVirtualCamera.Lens.FieldOfView, _fieldOfView, Time.deltaTime * _speedScale);
    }
    
    private void Movement()
    {
        float newSpeed = speed;
        Vector3 inputDir = Vector3.zero;
        inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (Input.touchCount == 1)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _dragMoveActive = true;
                _lastMousePosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0)) _dragMoveActive = false;
        }

        _dragSpeed = _startDragSpeed * _fieldOfView / 60;
        
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

    private void OnDisable()
    {
        GameEventManager.OnCameraMoverState -= UpdateMoveBlock;
    }
}
