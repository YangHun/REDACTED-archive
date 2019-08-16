using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaikoNote : MonoBehaviour
{
    public int timingHint;
    private Note data;
    private RectTransform rect;
    private float speed;
    private float offset;
    System.Action onDespawn;


    public void Init(Transform parent, Note data, System.Action onDespawn, float speed = 1000, float offset = 0) {
        this.rect = GetComponent<RectTransform>();
        this.data = data;
        this.timingHint = data.timing;
        this.onDespawn = onDespawn;
        this.transform.SetParent(parent);
        this.gameObject.SetActive(true);
        this.transform.localPosition = Vector3.zero;
        this.speed = speed;
        this.offset = offset;
    }
    
    public void OnUpdate(float velocity) {
        Vector2 pos = this.rect.anchoredPosition;
        if (pos.x < -1500f) {
            onDespawn?.Invoke();
            onDespawn = null;
            return;
        }
        this.rect.anchoredPosition = new Vector3( speed * (data.timing / 1000 - SoundModule.Instance.GetTiming) - offset, 0);
    }
}
