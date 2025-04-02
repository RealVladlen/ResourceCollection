using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private NavMeshAgent _agent;
    private bool _moveState;

    private float _tapStartTime;
    private float _tapThreshold = 0.2f; 

    private bool _isDragging;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        GameEventManager.OnPlayerMoverState += UpdateMoveState;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _moveState)
        {
            _tapStartTime = Time.time; 
            _isDragging = false;
        }

        if (Input.GetMouseButton(0))
        {
            if (Time.time - _tapStartTime > _tapThreshold)
                _isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!_isDragging)
                MovePlayer();
        }
        
        animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    private void MovePlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
            _agent.SetDestination(hit.point);
    }

    private void UpdateMoveState(bool state) => _moveState = state;

    private void OnDisable()
    {
        GameEventManager.OnPlayerMoverState -= UpdateMoveState;
    }
}