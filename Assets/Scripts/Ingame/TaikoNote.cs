using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaikoNote : MonoBehaviour
{
    private Note data;
    private RectTransform rect;

    System.Action onDespawn;

    public void Init (Transform parent, Note data, System.Action onDespawn) {
        this.rect = GetComponent<RectTransform>();
        this.data = data;
        this.onDespawn = onDespawn;
        this.transform.SetParent(parent);
        this.gameObject.SetActive(true);
        this.transform.localPosition = Vector3.zero;
    }
    
    public void OnUpdate(float velocity) {
        Vector2 pos = this.rect.anchoredPosition;
        if (pos.x < -1500f) {
            onDespawn?.Invoke();
            onDespawn = null;
            return;
        }
        pos.x -= 1280 * Time.fixedDeltaTime * velocity;
        this.rect.anchoredPosition = pos;
    }
}
