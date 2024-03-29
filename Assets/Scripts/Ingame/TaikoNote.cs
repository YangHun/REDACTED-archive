﻿using System.Collections;
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
    System.Action onMiss;
    System.Action onDespawn;

    public TextMeshProUGUI text;

    private bool judged = false;
    public bool IsJudged {
        get { return this.judged; }
    }
 
    

#region Effects

    [SerializeField]
    Image bgCircle;

    [SerializeField]
    ParticleSystem circle;

    [SerializeField]
    ParticleSystem burst;

#endregion

    private float barLength;

    public int channel = -1;
    
    public void Init(Note data, float barLength, System.Action onMiss, System.Action onDespawn, int channel, float speed = 1000, float offset = 0) {
        this.rect = GetComponent<RectTransform>();
        this.data = data;
        this.timingHint = data.timing;
        this.onMiss = onMiss;
        this.onDespawn = onDespawn;
        this.barLength = barLength;
        this.transform.localPosition = Vector3.zero;
        this.transform.localScale = Vector3.one;
        this.channel = channel;

        this.gameObject.SetActive(true);
        this.judged = false;

        this.text.color = new Color32 (0,0,0,255);
        this.bgCircle.color = GameConstant.NOTE_COLOR_TINT;
        
        this.text.text = this.data.charactor;

        this.speed = speed;
        this.offset = offset;
    }

    
    public void OnUpdate(float velocity) {
        Vector2 pos = this.rect.anchoredPosition;
        if (onMiss != null && pos.x < GameConstant.JUDGE_OFFSET_ENTRY * -1) {
            onMiss?.Invoke();
            PlayAutoMissTouchEffect();
            onMiss = null;
        }
        else if (pos.x < Screen.width / 2.0f * -1f) {
            onDespawn?.Invoke();
            onDespawn = null;
            return;
        }
        //this.rect.anchoredPosition = new Vector3( speed * (data.timing / 1000 - SoundModule.Instance.GetTiming) - offset, 0);
        this.rect.anchoredPosition = Vector2.right * ( ((float)data.timing/1000.0f) - SoundModule.Instance.BGM.time) * barLength * velocity;
    }

    public void SetJudged () {
        this.onMiss = null;
        this.judged = true;
    }

    public void PlayNormalTouchEffect() {
        //todo : normal score effect
        Vector3 pos = transform.position - Vector3.forward;
        //this.circle.transform.position = pos;
        this.circle.Play();

        this.bgCircle.color = new Color (1,1,1,0);
        this.text.color = new Color32 (255,255,255,255);
    }

    public void PlayExactTouchEffect() {
        // todo : exact score effect
        
        Vector3 pos = transform.position - Vector3.forward;
        //this.circle.transform.position = pos;
        //this.burst.transform.position = pos;
        this.circle.Play();
        this.burst.Play();

        
        this.bgCircle.color = new Color (1,1,1,0);
        this.text.color = new Color32 (255,255,255,255);
    }

    public void ChangeText() {
        this.text.text = "*";
    }

    public void PlayMissTouchEffect() {
        this.bgCircle.color = new Color (1,1,1,0);
        this.text.color = new Color32 (255,0,0,255);
    }

    public void PlayAutoMissTouchEffect() {
        this.bgCircle.color = new Color (1,1,1,0);
        this.text.color = new Color32 (111,111,111,255);
    }
}
