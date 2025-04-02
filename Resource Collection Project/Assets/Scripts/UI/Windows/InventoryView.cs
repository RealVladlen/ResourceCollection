using System.Collections.Generic;
using System.Linq;
using Building;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class InventoryView : WindowView
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private List<ResourcesView> resourcesViews;

        private void OnEnable()
        {
            Show();
            
            closeButton.onClick.AddListener(CloseWindow);
            
            GameEventManager.CameraMoverStateMethod(true);
            GameEventManager.PlayerMoverStateMethod(false);

            GameEventManager.OnUpdateResourcesCount += UpdateResourcesView;
        }

        private void UpdateResourcesView(EResourceType type, int count)
        {
            foreach (var t in resourcesViews.Where(t => t.resourceType == type))
                t.UpdateCount(count);
        }
        
        private void CloseWindow()
        {
            Hide(() =>
            {
                GameEventManager.CameraMoverStateMethod(false);
                GameEventManager.PlayerMoverStateMethod(true);
            });
        }
        
        private void OnDisable()
        {
            closeButton.onClick.RemoveAllListeners();
            
            GameEventManager.OnUpdateResourcesCount -= UpdateResourcesView;

        }
    }
}
