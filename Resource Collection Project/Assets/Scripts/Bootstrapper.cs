using System.Collections;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(GameInit());
    }

    private IEnumerator GameInit()
    {
        Fader.Instance.HideFade(null);
        
        
        yield return null;
    }
}
