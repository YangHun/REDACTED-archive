using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputModule : MonoBehaviour
{
    //use these to assign click event functions 
    public static System.Action onLeftMouseClicked = null;
    public static System.Action onRightMouseClicked = null;

    private Coroutine coroutine = null;
    void Awake() {
        DontDestroyOnLoad (this.gameObject);
        //if (EventSystem.current == null) this.gameObject.AddComponent<EventSystem>();
        this.coroutine = StartCoroutine (OnUpdate());
    }

    private IEnumerator OnUpdate() {
        while (true) {
            if (Input.GetMouseButtonDown(0)) onLeftMouseClicked?.Invoke();
            else if (Input.GetMouseButtonDown(1)) onRightMouseClicked?.Invoke();
            
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
