using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = System.Diagnostics.Debug;

public class InputModule : MonoBehaviour
{
    //use these to assign click event functions 
    public static System.Action onLeftMouseClicked = null;
    public static System.Action onRightMouseClicked = null;

    private Coroutine coroutine = null;
    void Start() {
        DontDestroyOnLoad (this.gameObject);
        if (this.coroutine == null) this.coroutine = StartCoroutine (OnUpdate());
    }

    private IEnumerator OnUpdate() {
        while (true) {
            /*
            if ( EventSystem.current != null && EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null) {
                yield return null;
                continue;
            }
            */
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftArrow)) onLeftMouseClicked?.Invoke();        
            else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.RightArrow)) onRightMouseClicked?.Invoke();
            yield return null;
        }
    }

    void OnApplicationPause () {
        if (this.coroutine != null) {
            StopCoroutine (this.coroutine);
            this.coroutine = null;
        }
    }

    void OnApplicationFocus() {
        if (this.coroutine == null) this.coroutine = StartCoroutine (OnUpdate());
    }
}
