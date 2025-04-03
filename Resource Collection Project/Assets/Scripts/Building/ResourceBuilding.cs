using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Building
{
    public class ResourceBuilding : MonoBehaviour
    {
        [SerializeField] private Transform gatherPoint;
        [SerializeField] private EResourceType eResourceType;
        [SerializeField] private float gatherInterval = 2f;
        [SerializeField] private int resourceAmount = 1;
        [SerializeField] private TextMeshProUGUI count;
        [SerializeField] private TextMeshProUGUI typeResource;
        
        private int _storedResources = 0;
        
        public Transform GatherPoint => gatherPoint;
        public EResourceType EResourceType => eResourceType;
        
        private void Start()
        {
            Init();
        }

        private void Init()
        {
            StartCoroutine(GenerateResources());

            string typeName = eResourceType switch
            {
                EResourceType.Wood => "WOOD",
                EResourceType.Stone => "STONE",
                EResourceType.Gold => "GOLD",
                EResourceType.Iron => "IRON",
                EResourceType.Crystal => "CRYSTAL",
                _ => throw new ArgumentOutOfRangeException()
            };

            typeResource.text = typeName;
            count.text = "0";
        }
        
        private IEnumerator GenerateResources()
        {
            while (true)
            {
                yield return new WaitForSeconds(gatherInterval);
                _storedResources += resourceAmount;
                //Debug.Log($"{gameObject.name} добыл {resourceAmount} {eResourceType}. Всего: {_storedResources}");
                count.text = _storedResources.ToString();
            }
        }

        public int CollectResource()
        {
            if (_storedResources > 0)
            {
                _storedResources--;
                count.text = _storedResources.ToString();
                return 1;
            }
            return 0;
        }
    }
}