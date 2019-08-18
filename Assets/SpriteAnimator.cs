using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpriteAnimator : MonoBehaviour
{
    public bool loop;
    public bool playOnStart;
    [ Range(10, 60) ]
    public float playbackSpeed = 30;
    public enum Type {
        centerguy,
        bass,
        other
    }
    public Type type;
    public List<Sprite> sprites;

    float Bpm { get { return taikoModule.Bpm; } }
    float Beat { get { return SoundModule.Instance.GetTiming * 60 / Bpm; } }

    Image dis;

    private TaikoModule taikoModule;

    void Awake()
    {
        taikoModule = FindObjectOfType<TaikoModule>();
    }
    
    void Init()
    {
        Debug.Assert(sprites.Count >= 1);
        dis = GetComponent<Image>();
        dis.sprite = sprites[0];
        if (playOnStart) StartAnimation();
    }

    bool inited = false;
    void Update()
    {
        if (inited == false) Init();
        inited = true;
    }


    Coroutine playingRoutine;
    public void StartAnimation()
    {
        if (playingRoutine != null) StopCoroutine(playingRoutine);
        playingRoutine = StartCoroutine(AnimationRoutine());
    }
    IEnumerator AnimationRoutine()
    {
        if (type == Type.other)
        {
            playbackSpeed = Bpm / 60 * sprites.Count / 4;
            do
            {
                foreach (var sprite in sprites)
                {
                    dis.sprite = sprite;
                    yield return new WaitForSeconds(1 / playbackSpeed);
                }
            } while (loop);
            playingRoutine = null;
        }
        if (type == Type.bass)
        {
            do
            {
                foreach (var sprite in sprites)
                {
                    dis.sprite = sprite;

                    yield return new WaitForSeconds(Random.Range(1 / playbackSpeed, 1));
                }
            } while (loop);
            playingRoutine = null;
        }
        if (type == Type.centerguy)
        {
            
            playbackSpeed = Bpm/60*30/4;
            do
            {
                foreach (var sprite in sprites)
                {
                    dis.sprite = sprite;

                    yield return new WaitForSeconds(1 / playbackSpeed);
                }
                for (int i = 0; i < 2; i++)
                {
                    dis.sprite = sprites[12];
                    yield return new WaitForSeconds(4 / playbackSpeed);
                    dis.sprite = sprites[13];
                    yield return new WaitForSeconds(4 / playbackSpeed);
                }
            } while (loop);
            playingRoutine = null;
        }
    }
}
