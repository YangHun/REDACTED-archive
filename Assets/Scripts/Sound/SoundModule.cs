using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundModule : Singleton<SoundModule>
{   

    [SerializeField]
    private List <AudioClip> bgmClips;

    [SerializeField]
    private List <AudioClip> sfxClips;

    private AudioSource bgm = null;
    private AudioSource sfx = null;

    public float GetTiming {
        get { return bgm.time; }
    }

    public AudioSource BGM {
        get { return this.bgm; }
        private set {}
    }

    void Awake() {
        //DontDestroyOnLoad(this.gameObject);
        Init();
    }

    private void Init() {
        this.bgm = this.gameObject.AddComponent<AudioSource>();
        this.bgm.clip = null;
        this.bgm.playOnAwake = false;
        
        this.sfx = this.gameObject.AddComponent<AudioSource>();
        this.sfx.clip = null;
        this.sfx.playOnAwake = false;        
    }

    ///<param name="index">difficulty of each level</param>
    public void PlayBGM (int index, System.Action onStop = null) {
        if (bgmClips.Count <= index) return;
        if (this.bgm.isPlaying) this.bgm.Stop();
        this.bgm.clip = this.bgmClips [index];
        this.bgm.Play();
        StartCoroutine (DelayedCall(this.bgm, onStop));  
    }

    public void PlayBGM(AudioClip clip, System.Action onStop = null)
    {
        if (this.bgm.isPlaying) this.bgm.Stop();
        this.bgm.clip = clip;
        this.bgm.Play();
        StartCoroutine(DelayedCall(this.bgm, onStop));
    }


    public void SetBGMVolume (float value) {
        this.bgm.volume = Mathf.Clamp (value,0.0f, 1.0f);
    }

    public void StopBGM () {
        this.bgm.Stop();
    }

    private IEnumerator DelayedCall (AudioSource audio, System.Action onStop = null) {
        while (audio.isPlaying) {
            yield return null;
        }
        onStop?.Invoke();
    }

    ///<param name="index">sfx index</param>
    public void PlaySFX (int index, System.Action onStop = null) {
        this.sfx.PlayOneShot(this.sfxClips [index]);
        
        StartCoroutine (DelayedCall(this.sfx, onStop));  
    }    
}
