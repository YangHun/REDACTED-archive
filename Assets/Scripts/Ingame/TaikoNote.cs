using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaikoNote : MonoBehaviour
{
    public int timingHint;
    private Note data;
    private RectTransform rect;
    private float speed;
    private float offset;
    System.Action onDespawn;

    public TextMeshProUGUI text;


    private float barLength;
    
    public void Init(Note data, float barLength, System.Action onDespawn, float speed = 1000, float offset = 0) {
        this.rect = GetComponent<RectTransform>();
        this.data = data;
        this.timingHint = data.timing;
        this.onDespawn = onDespawn;
        this.barLength = barLength;
        this.transform.localPosition = Vector3.zero;
        this.transform.localScale = Vector3.one;

        this.gameObject.SetActive(true);
        
        this.text.text = this.data.charactor;

        this.speed = speed;
        this.offset = offset;
    }

    
    public void OnUpdate(float velocity) {
        Vector2 pos = this.rect.anchoredPosition;
        if (pos.x < GameConstant.JUDGE_OFFSET_ENTRY * -1) {
            onDespawn?.Invoke();
            onDespawn = null;
            return;
        }
        //this.rect.anchoredPosition = new Vector3( speed * (data.timing / 1000 - SoundModule.Instance.GetTiming) - offset, 0);
        this.rect.anchoredPosition = Vector2.right * ( ((float)data.timing/1000.0f) - SoundModule.Instance.BGM.time) * barLength * velocity;
    }

    public void PlayNormalTouchEffect() {
        //todo : normal score effect
    }

    public void PlayExactTouchEffect() {
        // todo : exact score effect
    }

    public void PlayMissTouchEffect() {

    }
}
