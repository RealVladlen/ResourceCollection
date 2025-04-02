using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class WindowsManager : MonoBehaviour
    {
        public static WindowsManager Instance;
        
        [SerializeField] private Transform windowsList;

        private List<WindowView> _windowsList = new List<WindowView>();
        private EWindow _currentType;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
        
            Instance = this;
        }
        
        public bool Init()
        {
            return  FillWindows();
        }
        
        private bool FillWindows()
        {
            _windowsList.Clear();
            _windowsList.AddRange(windowsList.GetComponentsInChildren<WindowView>(true));

            foreach (var window in _windowsList)
                window.gameObject.SetActive(false);

            return true;
        }
        
        /// <summary>
        /// Показать требуемое окно
        /// </summary>
        /// <param name="type"></param>
        public void ShowWindow(EWindow type)
        {
            foreach (var t in _windowsList)
            {
                if (t.GetWindowType() == type)
                {
                    t.gameObject.SetActive(true);
                    _currentType = type;
                    break;
                }
            }
        }
        
        /// <summary>
        /// Выключить требуемое окно
        /// </summary>
        /// <param name="type"></param>
        public void HideWindow(EWindow type)
        {
            for (int i = 0; i < _windowsList.Count; i++)
            {
                if (_windowsList[i].GetWindowType() == type)
                {
                    _windowsList[i].gameObject.SetActive(false);
                    break;
                }
            }
        }
    }
}
