using System;
using DG.Tweening;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public static Fader Instance;

    private CanvasGroup _canvasGroup;
    private Tween _fadeAnimation;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 1;
    }

    /// <summary>
    /// Запуск фейда, затемнение экрана
    /// </summary>
    /// <param name="method"> можно передать метод, который должен выполниться после того, как произойдет затемнение экрана</param>
    /// <param name="speed"> скорость затемнения экрана</param>
    public void ShowFade(Action method, float speed = 0.5f)
    {
        _fadeAnimation?.Kill();
        _fadeAnimation = _canvasGroup.DOFade(1, speed).OnComplete(() => method?.Invoke());
    }

    /// <summary>
    /// Запуск фейда, убирается затемнение экрана
    /// </summary>
    /// <param name="method"> можно передать метод, который должен выполниться после того, как произойдет возврат экрана</param>
    /// <param name="speed"> скорость затемнения экрана</param>
    public void HideFade(Action method, float speed = 0.5f)
    {
        _fadeAnimation?.Kill();
        Debug.Log("HideFade");
        _fadeAnimation = _canvasGroup.DOFade(0, speed).OnComplete(() => method?.Invoke());
    }

    private void OnDisable()
    {
        _fadeAnimation?.Kill();
        _fadeAnimation = null;
    }
}
