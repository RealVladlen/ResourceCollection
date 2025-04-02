using UnityEngine;

namespace Building
{
    public class ResourceBuilding : MonoBehaviour
    {
        [SerializeField] private Transform gatherPoint; 
        [SerializeField] private EResourceType eResourceType; 
        [SerializeField] private float gatherInterval = 2f; 
        [SerializeField] private int resourceAmount = 1; 

        public Transform GatherPoint => gatherPoint;
        public EResourceType EResourceType => eResourceType;
        public float GatherInterval => gatherInterval;
        public int ResourceAmount => resourceAmount;
    }
}