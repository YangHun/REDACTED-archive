using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightModule : Singleton<HighlightModule>
{
    List<Note> c;
    public List<Transform> fireworkPoints;
    public GameObject fireworkPF;
    public void Init(List<Note> c)
    {
        this.c = c;
    }
    // Start is called before the first frame update
    void Start()
    {
        SpotlightAnimator.gLobalenalbeD = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (c != null)
        {
            if (c.Count == 0) c = null;
            if (c[0].timing <= SoundModule.Instance.GetTiming * 1000)
            {
                var q = c[0];c.RemoveAt(0);
                switch (q.charactor)
                {
                    case "펑":
                        foreach (var t in fireworkPoints)
                            Instantiate(fireworkPF, t.position, Quaternion.identity);
                        SpotlightAnimator.gLobalenalbeD = true;
                        break;
                    case "슉":
                        SpotlightAnimator.gLobalenalbeD = false;
                        break;
                }
            }
        }
    }
}
