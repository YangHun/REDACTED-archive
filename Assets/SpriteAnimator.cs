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
    public List<Sprite> sprites;

    Image dis;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(sprites.Count >= 1);
        dis = GetComponent<Image>();
        dis.sprite = sprites[0];
        if (playOnStart) StartAnimation();
    }

    Coroutine playingRoutine;
    public void StartAnimation()
    {
        if (playingRoutine != null) StopCoroutine(playingRoutine);
        playingRoutine = StartCoroutine(AnimationRoutine());
    }
    IEnumerator AnimationRoutine()
    {
        do
        {
            foreach (var sprite in sprites)
            {
                dis.sprite = sprite;

                yield return new WaitForSeconds(1 / playbackSpeed);
            }
            dis.sprite = sprites[0];
            yield return new WaitForSeconds(1);
        } while (loop);
        dis.sprite = sprites[0];
        playingRoutine = null;
    }
}
