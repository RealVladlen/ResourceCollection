using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class GamePlayView : WindowView
    {
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button inventoryButton;

        private void OnEnable()
        {
            settingsButton.onClick.AddListener(OpenSettings);
            inventoryButton.onClick.AddListener(OpenInventory);
        }

        private void OpenSettings()
        {
            WindowsManager.Instance.ShowWindow(EWindow.Settings);
        }

        private void OpenInventory()
        {
            WindowsManager.Instance.ShowWindow(EWindow.Inventory);

        }
        
        private void OnDisable()
        {
            settingsButton.onClick.RemoveAllListeners();
            inventoryButton.onClick.RemoveAllListeners();
        }
    }
}
