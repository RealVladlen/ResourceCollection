using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]

    public class WindowView : MonoBehaviour
    {
        [SerializeField] private EWindow window;

        public EWindow GetWindowType() => window;

        private CanvasGroup _canvasGroup;
        private Tween _fadeAnimation;

        public virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Show(Action method = null)
        {
            _canvasGroup.alpha = 0;
            _fadeAnimation?.Kill();
            _fadeAnimation = _canvasGroup.DOFade(1,0.25f).OnComplete(() => method?.Invoke());
        }

        public virtual void ExpressShow()
        {
            _canvasGroup.alpha = 1;
        }
        
        public virtual void Hide(Action method = null)
        {
            _fadeAnimation?.Kill();
            _fadeAnimation = _canvasGroup.DOFade(0,0.25f).OnComplete(() =>
            {
                method?.Invoke();
                gameObject.SetActive(false);
            });
        }
        
        public virtual void ExpressHide()
        {
            _canvasGroup.alpha = 0;
        }
    }
}