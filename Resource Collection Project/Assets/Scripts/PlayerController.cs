using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Building;

[RequireComponent(typeof(ResourceGatherer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private NavMeshAgent _agent;
    private bool _moveState;
    private bool _isDragging;
    private float _tapStartTime;
    private float _tapThreshold = 0.2f;
    private ResourceGatherer _resourceGatherer;
    private ResourceBuilding _targetBuilding;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _resourceGatherer = GetComponent<ResourceGatherer>();
    }

    private void OnEnable()
    {
        GameEventManager.OnPlayerMoverState += UpdateMoveState;
    }

    private void Update()
    { 
        if (IsPointerOverUI()) return;

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
                HandleClick();
        }
        
        animator.SetFloat("Speed", _agent.velocity.magnitude);

        if (_targetBuilding != null && _agent.remainingDistance <= _agent.stoppingDistance && !_agent.pathPending)
        {
            _resourceGatherer.StartGathering(_targetBuilding);
            _targetBuilding = null;
        }
    }

    private void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out ResourceBuilding building))
            {
                MoveToBuilding(building);
            }
            else
            {
                _resourceGatherer.StopGathering();
                _agent.SetDestination(hit.point);
            }
        }
    }

    private void MoveToBuilding(ResourceBuilding building)
    {
        _agent.SetDestination(building.GatherPoint.position);
        _targetBuilding = building;
    }

    private void UpdateMoveState(bool state) => _moveState = state;

    private void OnDisable()
    {
        GameEventManager.OnPlayerMoverState -= UpdateMoveState;
    }

    private bool IsPointerOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return true;

        if (Input.touchCount > 0)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.GetTouch(0).position
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }

        return false;
    }
}
