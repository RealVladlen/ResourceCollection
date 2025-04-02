using Building;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ResourcesView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textCount;

    public EResourceType resourceType;
    
    private Tween _scaleAnimation;

    private void OnEnable()
    {
        UpdateCount(GameDataManager.Instance.Resources(resourceType));
    }

    public void UpdateCount(int count)
    {
        textCount.text = count.ToString();
    }
}
