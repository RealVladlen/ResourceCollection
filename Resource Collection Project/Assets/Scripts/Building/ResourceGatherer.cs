using System.Collections;
using UnityEngine;
using Building;

public class ResourceGatherer : MonoBehaviour
{
    private ResourceBuilding _currentBuilding;
    private Coroutine _gatheringCoroutine;
    private bool _isGathering;

    public void StartGathering(ResourceBuilding building)
    {
        if (_isGathering) StopGathering();

        _currentBuilding = building;
        _isGathering = true;
        _gatheringCoroutine = StartCoroutine(GatherResources());
    }

    public void StopGathering()
    {
        if (!_isGathering) return;

        _isGathering = false;
        _currentBuilding = null;

        if (_gatheringCoroutine != null)
            StopCoroutine(_gatheringCoroutine);
    }

    private IEnumerator GatherResources()
    {
        while (_isGathering)
        {
            yield return new WaitForSeconds(_currentBuilding.GatherInterval);

            if (_isGathering)
            {
                Debug.Log($"Игрок добыл {_currentBuilding.ResourceAmount} {_currentBuilding.EResourceType}");
                GameDataManager.Instance.Resources(_currentBuilding.EResourceType, _currentBuilding.ResourceAmount);
            }
        }
    }
}