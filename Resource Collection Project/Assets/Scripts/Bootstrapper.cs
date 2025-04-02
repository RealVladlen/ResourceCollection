using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    private bool _sceneActivationTriggered;

    private void Start()
    {
        StartCoroutine(GameInit());
    }

    private IEnumerator GameInit()
    {
        _sceneActivationTriggered = false; 
        GameEventManager.CameraMoverStateMethod(false);
        
        yield return new WaitUntil(WindowsManager.Instance.Init);
        yield return new WaitUntil(GameDataManager.Instance.Init);
        
        WindowsManager.Instance.ShowWindow(EWindow.Loading);
        
        Fader.Instance.HideFade(null);
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene", LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false; 
        
        yield return new WaitForSeconds(1);
        
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f && !_sceneActivationTriggered) 
            {
                _sceneActivationTriggered = true;
                
                Fader.Instance.ShowFade(() => StartCoroutine(ActivateScene(asyncLoad)));
            }
            yield return null;
        }
    }

    private IEnumerator ActivateScene(AsyncOperation asyncLoad)
    {
        yield return new WaitForSeconds(1);
        
        WindowsManager.Instance.HideWindow(EWindow.Loading);
        asyncLoad.allowSceneActivation = true;
        
        WindowsManager.Instance.ShowWindow(EWindow.GamePlay);
        
        
        yield return new WaitForSeconds(0.5f);
        
        Fader.Instance.HideFade(() =>
        {
            GameEventManager.CameraMoverStateMethod(false);
            GameEventManager.PlayerMoverStateMethod(true);
        });
        
    }
}
