using UnityEngine;
using System.Collections;
using Building;

public class ResourceGatherer : MonoBehaviour
{
    private ResourceBuilding _currentBuilding;
    private Coroutine _collectingCoroutine;

    public void StartGathering(ResourceBuilding building)
    {
        if (_currentBuilding == building) return;
        StopGathering();
        _currentBuilding = building;
        _collectingCoroutine = StartCoroutine(CollectResourcesOverTime());
    }

    public void StopGathering()
    {
        if (_collectingCoroutine != null)
            StopCoroutine(_collectingCoroutine);
        _currentBuilding = null;
    }

    private IEnumerator CollectResourcesOverTime()
    {
        while (_currentBuilding != null)
        {
            yield return new WaitForSeconds(0.5f);
            int collected = _currentBuilding.CollectResource();
            if (collected > 0)
            {
                Debug.Log($"Игрок собрал 1 {_currentBuilding.EResourceType}");
                GameDataManager.Instance.Resources(_currentBuilding.EResourceType, 1);
            }
            else
            {
                Debug.Log("В здании нет ресурсов для сбора.");
                //StopGathering();
            }
        }
    }
}